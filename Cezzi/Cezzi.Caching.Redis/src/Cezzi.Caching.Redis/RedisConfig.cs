namespace Cezzi.Caching.Redis;

/// <summary>
/// 
/// </summary>
public class RedisConfig
{
    /// <summary>Gets or sets the connection string.</summary>
    /// <value>The connection string.</value>
    public string ConnectionString { get; set; }

    /// <summary>Gets or sets the password.</summary>
    /// <value>The password.</value>
    public string Password { get; set; }

    /// <summary>Gets or sets the reconnect minimum frequency.</summary>
    /// <value>The reconnect minimum frequency.</value>
    public int? ReconnectMinFrequency { get; set; }

    /// <summary>Gets or sets the reconnect error threshold.</summary>
    /// <value>The reconnect error threshold.</value>
    public int? ReconnectErrorThreshold { get; set; }

    /// <summary>Gets or sets the retry maximum attempts.</summary>
    /// <value>The retry maximum attempts.</value>
    public int? RetryMaxAttempts { get; set; }

    /// <summary>Gets the connection string.</summary>
    /// <returns></returns>
    public string GetConnectionString()
    {
        return !string.IsNullOrWhiteSpace(this.Password)
            ? $"{this.ConnectionString},password={this.Password}"
            : this.ConnectionString;
    }
}
