using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace badpaybad.redis.repository
{
    public class RedisPubSubSample
    {
        private ISubscriber _subscriber;
        public RedisPubSubSample()
        {
            RedisConnectionPool.Init(new RedisConfig()
            {
                Host = "badpaybad.info",
                Port = 6379,
                Pwd = "badpaybad.info"
            });

            _subscriber = RedisConnectionPool.CurrentConnection.GetSubscriber();
            
        }

        public void PushMessage(string msg)
        {
           
            _subscriber.Publish("samplechannel", msg, CommandFlags.FireAndForget);
        }

        public void SubcribeMessage(Action<string> action)
        {
            _subscriber.Subscribe("samplechannel", (c, v) =>
            {
                action((string) v);
            });
        }
    }
}
