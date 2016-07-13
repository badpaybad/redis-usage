using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using StackExchange.Redis;

namespace badpaybad.redis.repository
{
    public class RedisAsCacheSample
    {
        public RedisAsCacheSample()
        {
            RedisConnectionPool.Init(new RedisConfig()
            {
                Host = "badpaybad.info",
                Port = 6379,
                Pwd = "badpaybad.info"
            });
        }

        public void AddOrUpdate<T>(string key, T val, TimeSpan? expiredIn = null)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key is empty");
            if (val == null) throw new ArgumentNullException("val is null");

            var db = RedisConnectionPool.CurrentConnection.GetDatabase();
            var data = new JavaScriptSerializer().Serialize(val);
            db.StringSet(key, data, expiredIn, When.Always, CommandFlags.FireAndForget);
        }

        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key is empty");
            var db = RedisConnectionPool.CurrentConnection.GetDatabase();
            db.KeyDelete(key,CommandFlags.FireAndForget);
        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key is empty");
            var db = RedisConnectionPool.CurrentConnection.GetDatabase();
            var temp = db.StringGet(key);

            if ( !temp.HasValue) return default(T);
            
            return  new JavaScriptSerializer().Deserialize<T>(temp);
        }
        
    }
}