namespace Cezzi.Caching.Redis;

using global::StackExchange.Redis;
using System;
using System.Net.Sockets;
using System.Threading;

/// <summary>
/// The connection to the Azure Cache for Redis is managed by the ConnectionMultiplexer class.
/// This class should be shared and reused throughout your client application.
/// Do not create a new connection for each operation.
/// </summary>
public class RedisConnection
{
    private readonly object reconnectLock;
    private readonly TimeSpan reconnectMinFrequency;
    private readonly TimeSpan reconnectErrorThreshold;
    private readonly int retryMaxAttempts;
    private readonly string connectionString;
    private long lastReconnectTicks;
    private DateTimeOffset firstErrorTime;
    private DateTimeOffset previousErrorTime;
    private Lazy<ConnectionMultiplexer> lazyConnection;

    /// <summary>Initializes a new instance of the <see cref="RedisConnection" /> class.</summary>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="reconnectMinFrequency">The reconnect minimum frequency.</param>
    /// <param name="reconnectErrorThreshold">The reconnect error threshold.</param>
    /// <param name="retryMaxAttempts">The retry maximum attempts.</param>
    public RedisConnection(
        string connectionString,
        int? reconnectMinFrequency = 60,
        int? reconnectErrorThreshold = 30,
        int? retryMaxAttempts = 5)
    {
        this.connectionString = connectionString;
        this.reconnectLock = new object();
        this.lastReconnectTicks = DateTimeOffset.MinValue.UtcTicks;
        this.firstErrorTime = DateTimeOffset.MinValue;
        this.previousErrorTime = DateTimeOffset.MinValue;
        this.reconnectMinFrequency = TimeSpan.FromSeconds(reconnectMinFrequency ?? 60);
        this.reconnectErrorThreshold = TimeSpan.FromSeconds(reconnectErrorThreshold ?? 30);
        this.retryMaxAttempts = retryMaxAttempts ?? 5;

        this.lazyConnection = this.CreateConnection();
    }

    /// <summary>
    /// Gets the database.
    /// </summary>
    /// <returns></returns>
    public IDatabase GetDatabase() => this.BasicRetry(() => this.GetConnection().GetDatabase());

    private ConnectionMultiplexer GetConnection()
    {
        if (this.lazyConnection.Value.IsConnected)
        {
            return this.lazyConnection.Value;
        }

        this.lazyConnection = this.CreateConnection();
        return this.lazyConnection.Value;
    }

    private T BasicRetry<T>(Func<T> func)
    {
        var reconnectRetry = 0;
        var disposedRetry = 0;

        while (true)
        {
            try
            {
                return func();
            }
            catch (Exception ex) when (ex is RedisConnectionException or SocketException)
            {
                reconnectRetry++;
                if (reconnectRetry > this.retryMaxAttempts)
                {
                    throw;
                }

                this.ForceReconnect();
            }
            catch (ObjectDisposedException)
            {
                disposedRetry++;
                if (disposedRetry > this.retryMaxAttempts)
                {
                    throw;
                }
            }
        }
    }

    private Lazy<ConnectionMultiplexer> CreateConnection() => new(() => ConnectionMultiplexer.Connect(this.connectionString));

    private void ForceReconnect()
    {
        var utcNow = DateTimeOffset.UtcNow;
        var previousTicks = Interlocked.Read(ref this.lastReconnectTicks);
        var previousReconnectTime = new DateTimeOffset(previousTicks, TimeSpan.Zero);
        var elapsedSinceLastReconnect = utcNow - previousReconnectTime;

        // If multiple threads call ForceReconnect at the same time, we only want to honor one of them.
        if (elapsedSinceLastReconnect < this.reconnectMinFrequency)
        {
            return;
        }

        lock (this.reconnectLock)
        {
            utcNow = DateTimeOffset.UtcNow;
            elapsedSinceLastReconnect = utcNow - previousReconnectTime;

            if (this.firstErrorTime == DateTimeOffset.MinValue)
            {
                // We haven't seen an error since last reconnect, so set initial values.
                this.firstErrorTime = utcNow;
                this.previousErrorTime = utcNow;
                return;
            }

            if (elapsedSinceLastReconnect < this.reconnectMinFrequency)
            {
                return; // Some other thread made it through the check and the lock, so nothing to do.
            }

            var elapsedSinceFirstError = utcNow - this.firstErrorTime;
            var elapsedSinceMostRecentError = utcNow - this.previousErrorTime;

            var shouldReconnect =
                elapsedSinceFirstError >= this.reconnectErrorThreshold // Make sure we gave the multiplexer enough time to reconnect on its own if it could.
                && elapsedSinceMostRecentError <= this.reconnectErrorThreshold; // Make sure we aren't working on stale data (e.g. if there was a gap in errors, don't reconnect yet).

            // Update the previousErrorTime timestamp to be now (e.g. this reconnect request).
            this.previousErrorTime = utcNow;

            if (!shouldReconnect)
            {
                return;
            }

            this.firstErrorTime = DateTimeOffset.MinValue;
            this.previousErrorTime = DateTimeOffset.MinValue;

            var oldConnection = this.lazyConnection;
            this.CloseConnection(oldConnection);

            this.lazyConnection = this.CreateConnection();
            Interlocked.Exchange(ref this.lastReconnectTicks, utcNow.UtcTicks);
        }
    }

    private void CloseConnection(Lazy<ConnectionMultiplexer> oldConnection)
    {
        if (oldConnection == null)
        {
            return;
        }

        try
        {
            oldConnection.Value.Close();
        }
        catch (Exception)
        {
            // Example error condition: if accessing oldConnection.Value causes a connection attempt and that fails.
        }
    }
}
