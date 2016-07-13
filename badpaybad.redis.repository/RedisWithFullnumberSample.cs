using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace badpaybad.redis.repository
{
    /// <summary>
    /// fullnumber mean: if you have decimal value 0.001 , 1.002
    /// so fullnumber will be 0.001*1000 , 1.002*1000 , mean that we store this value 1 and 1002
    /// depend on your system the maximum number and precision (after '.' dot )
    /// </summary>
    public class RedisWithFullnumberSample
    {
        public RedisWithFullnumberSample()
        {
            RedisConnectionPool.Init(new RedisConfig()
            {
                Host = "badpaybad.info",
                Port = 6379,
                Pwd = "badpaybad.info"
            });
        }

        double precision = 1000; //store exact to 3 number after dot

        public void Set(string key, double val)
        {
            var db = RedisConnectionPool.CurrentConnection.GetDatabase();
            long fullVal = (long)Math.Round((precision * val), 3);
            db.StringSet(key, fullVal);
           //db.StringSet(key, val);
        }

        public void Increase(string key, double val)
        {
            var db = RedisConnectionPool.CurrentConnection.GetDatabase();
            long fullVal = (long)Math.Round((precision * val), 3);
            db.StringIncrement(key, fullVal);
            //db.StringIncrement(key, val);
        }

        public double Get(string key)
        {
            var db = RedisConnectionPool.CurrentConnection.GetDatabase();
            var temp = (long)db.StringGet(key);
            return Math.Round(temp / precision, 3);
            //return (double) db.StringGet(key);
        }

        public string GetValueStored(string key)
        {
            var db = RedisConnectionPool.CurrentConnection.GetDatabase();
            return db.StringGet(key);
            
        }

        
    }
}