using System.Collections.Generic;
using StackExchange.Redis;

namespace badpaybad.redis.repository
{

    public class RedisConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Pwd { get; set; }
    }


    public static class RedisConnectionPool
    {
        static readonly Dictionary<RedisConfig, ConnectionMultiplexer> _pool;
        static readonly SocketManager _socketManager;
        static object _lock = new object();
        public static ConnectionMultiplexer CurrentConnection { get; private set; }
        public static IServer CurrentServer { get; private set; }
        public static RedisConfig CurrentConfig { get; private set; }

        static RedisConnectionPool()
        {
            _pool = new Dictionary<RedisConfig, ConnectionMultiplexer>();
            _socketManager = new SocketManager();
        }

        public static void Init(RedisConfig config)
        {
            CurrentConfig = config;
            CurrentConnection = GetConnection(config);
            CurrentServer = CurrentConnection.GetServer(config.Host, config.Port);
        }

        static ConnectionMultiplexer GetConnection(RedisConfig config)
        {
            var options = new ConfigurationOptions
            {
                EndPoints =
                {
                    {config.Host, config.Port}
                },
                Password = config.Pwd,
                AllowAdmin = false,
                SyncTimeout = 5 * 1000,
                SocketManager = _socketManager,
                AbortOnConnectFail = false,
                ConnectTimeout = 5 * 1000,
            };

            lock (_lock)
            {
                ConnectionMultiplexer c;
                if (_pool.TryGetValue(config, out c) && c.IsConnected)
                {
                    return c;
                }

                if (c != null) c.Dispose();

                c = ConnectionMultiplexer.Connect(options);

                _pool[config] = c;

                return c;
            }
        }
    }
}