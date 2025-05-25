//Copyright (c) 2007-2008 Henrik Schröder, Oliver Kofoed Pedersen

//Permission is hereby granted, free of charge, to any person
//obtaining a copy of this software and associated documentation
//files (the "Software"), to deal in the Software without
//restriction, including without limitation the rights to use,
//copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the
//Software is furnished to do so, subject to the following
//conditions:

//The above copyright notice and this permission notice shall be
//included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
//HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//OTHER DEALINGS IN THE SOFTWARE.

using System.Diagnostics;
using System.Net;


namespace Cezzi.Caching.Memcached
{
    /// <summary>
    /// The SocketPool encapsulates the list of PooledSockets against one specific host, and contains methods for 
    /// acquiring or returning PooledSockets.
    /// </summary>
    [DebuggerDisplay("[ Host: {Host} ]")]
    internal class SocketPool
    {
        private static readonly LogAdapter logger = LogAdapter.GetLogger(typeof(SocketPool));

        /// <summary>
        /// If the host stops responding, we mark it as dead for this amount of seconds, 
        /// and we double this for each consecutive failed retry. If the host comes alive
        /// again, we reset this to 1 again.
        /// </summary>
        private int deadEndPointSecondsUntilRetry = 1;
        private const int maxDeadEndPointSecondsUntilRetry = 60 * 10; //10 minutes
        private readonly ServerPool owner;
        private readonly IPEndPoint endPoint;
        private readonly Queue<PooledSocket> queue;

        //Debug variables and properties
        private int newsockets = 0;
        private int failednewsockets = 0;
        private int reusedsockets = 0;
        private int deadsocketsinpool = 0;
        private int deadsocketsonreturn = 0;
        private int dirtysocketsonreturn = 0;
        private int acquired = 0;
        public int NewSockets => newsockets;
        public int FailedNewSockets => failednewsockets;
        public int ReusedSockets => reusedsockets;
        public int DeadSocketsInPool => deadsocketsinpool;
        public int DeadSocketsOnReturn => deadsocketsonreturn;
        public int DirtySocketsOnReturn => dirtysocketsonreturn;
        public int Acquired => acquired;
        public int Poolsize => queue.Count;

        //Public variables and properties
        public readonly string Host;

        public bool IsEndPointDead { get; private set; } = false;

        public DateTime DeadEndPointRetryTime { get; private set; }

        internal SocketPool(ServerPool owner, string host)
        {
            Host = host;
            this.owner = owner;
            endPoint = getEndPoint(host);
            queue = new Queue<PooledSocket>();
        }

        /// <summary>
        /// This method parses the given string into an IPEndPoint.
        /// If the string is malformed in some way, or if the host cannot be resolved, this method will throw an exception.
        /// </summary>
        private static IPEndPoint getEndPoint(string host)
        {
            //Parse port, default to 11211.
            int port = 11211;
            if (host.Contains(":"))
            {
                string[] split = host.Split([':']);
                if (!int.TryParse(split[1], out port))
                {
                    throw new ArgumentException("Unable to parse host: " + host);
                }
                host = split[0];
            }

            //Parse host string.
            if (IPAddress.TryParse(host, out IPAddress address))
            {
                //host string successfully resolved as an IP address.
            }
            else
            {
                //See if we can resolve it as a hostname
                try
                {
                    address = Dns.GetHostEntry(host).AddressList[0];
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Unable to resolve host: " + host, e);
                }
            }

            return new IPEndPoint(address, port);
        }

        /// <summary>
        /// Gets a socket from the pool.
        /// If there are no free sockets, a new one will be created. If something goes
        /// wrong while creating the new socket, this pool's endpoint will be marked as dead
        /// and all subsequent calls to this method will return null until the retry interval
        /// has passed.
        /// </summary>
        internal PooledSocket Acquire()
        {
            //Do we have free sockets in the pool?
            //if so - return the first working one.
            //if not - create a new one.
            _ = Interlocked.Increment(ref acquired);
            lock (queue)
            {
                while (queue.Count > 0)
                {
                    PooledSocket socket = queue.Dequeue();
                    if (socket != null && socket.IsAlive)
                    {
                        _ = Interlocked.Increment(ref reusedsockets);
                        return socket;
                    }
                    _ = Interlocked.Increment(ref deadsocketsinpool);
                }
            }

            _ = Interlocked.Increment(ref newsockets);
            //If we know the endpoint is dead, check if it is time for a retry, otherwise return null.
            if (IsEndPointDead)
            {
                if (DateTime.Now > DeadEndPointRetryTime)
                {
                    //Retry
                    IsEndPointDead = false;
                }
                else
                {
                    //Still dead
                    return null;
                }
            }

            //Try to create a new socket. On failure, mark endpoint as dead and return null.
            try
            {
                PooledSocket socket = new(this, endPoint, owner.SendReceiveTimeout, owner.ConnectTimeout);
                //Reset retry timer on success.
                deadEndPointSecondsUntilRetry = 1;
                return socket;
            }
            catch (Exception e)
            {
                _ = Interlocked.Increment(ref failednewsockets);
                logger.Error("Error connecting to: " + endPoint.Address, e);
                //Mark endpoint as dead
                IsEndPointDead = true;
                //Retry in 2 minutes
                DeadEndPointRetryTime = DateTime.Now.AddSeconds(deadEndPointSecondsUntilRetry);
                if (deadEndPointSecondsUntilRetry < maxDeadEndPointSecondsUntilRetry)
                {
                    deadEndPointSecondsUntilRetry *= 2; //Double retry interval until next time
                }

                return null;
            }
        }

        /// <summary>
        /// Returns a socket to the pool.
        /// If the socket is dead, it will be destroyed.
        /// If there are more than MaxPoolSize sockets in the pool, it will be destroyed.
        /// If there are less than MinPoolSize sockets in the pool, it will always be put back.
        /// If there are something inbetween those values, the age of the socket is checked. 
        /// If it is older than the SocketRecycleAge, it is destroyed, otherwise it will be 
        /// put back in the pool.
        /// </summary>
        internal void Return(PooledSocket socket)
        {
            //If the socket is dead, destroy it.
            if (!socket.IsAlive)
            {
                _ = Interlocked.Increment(ref deadsocketsonreturn);
                socket.Close();
            }
            else
            {
                //Clean up socket
                if (socket.Reset())
                {
                    _ = Interlocked.Increment(ref dirtysocketsonreturn);
                }

                //Check pool size.
                if (queue.Count >= owner.MaxPoolSize)
                {
                    //If the pool is full, destroy the socket.
                    socket.Close();
                }
                else if (queue.Count > owner.MinPoolSize && DateTime.Now - socket.Created > owner.SocketRecycleAge)
                {
                    //If we have more than the minimum amount of sockets, but less than the max, and the socket is older than the recycle age, we destroy it.
                    socket.Close();
                }
                else
                {
                    //Put the socket back in the pool.
                    lock (queue)
                    {
                        queue.Enqueue(socket);
                    }
                }
            }
        }
    }
}
