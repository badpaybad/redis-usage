using System.Collections.Generic;
using StackExchange.Redis;

namespace badpaybad.redis.repository
{
    /*scenario: we have object Sample(Id,Version) 
     * we use redis to store data : )))) and CRUD on object Sample */

    public class Sample
    {
        public int Id { get; set; }
        public string Version { get; set; }
    }

    public class SampleRepository
    {
        public SampleRepository()
        {
            RedisConnectionPool.Init(new RedisConfig()
            {
                Host = "badpaybad.info",
                Port = 6379,
                Pwd = "badpaybad.info"
            });
        }


        public void AddOrUpdate(Sample obj)
        {
            var key = "sample_" + obj.Id;
            var db = RedisConnectionPool.CurrentConnection.GetDatabase();

            db.HashSet(key, "id", obj.Id, When.Always, CommandFlags.FireAndForget);
            db.HashSet(key, "version", obj.Version, When.Always, CommandFlags.FireAndForget);

            db.HashSet("sample_list", key, obj.Id, When.Always, CommandFlags.FireAndForget);
        }

        public Sample Select(int id)
        {
            var db = RedisConnectionPool.CurrentConnection.GetDatabase();
            return BuildSample(id, db);
        }

        private Sample BuildSample(int id, IDatabase db)
        {
            var key = "sample_" + id;
            var all = db.HashGetAll(key);
            //should not use _db.HashGet
            // cause need open socket to get each
            // we should get all in one time open socket
            var props = new Dictionary<string, HashEntry>();
            foreach (var hashEntry in all)
            {
                props[hashEntry.Name] = hashEntry;
            }

            var sample = new Sample();
            HashEntry idtemp;
            if (props.TryGetValue("id", out idtemp))
            {
                if (idtemp.Value.HasValue) sample.Id = (int)idtemp.Value;
            }

            HashEntry vertemp;
            if (props.TryGetValue("version", out vertemp))
            {
                if (vertemp.Value.HasValue) sample.Version = (string)vertemp.Value;
            }

            return sample;
        }

        public void Delete(int id)
        {
            var db = RedisConnectionPool.CurrentConnection.GetDatabase();
            var key = "sample_" + id;
            db.HashDelete(key, "id", CommandFlags.FireAndForget);
            db.HashDelete(key, "version", CommandFlags.FireAndForget);
            db.HashDelete("sample_list", key, CommandFlags.FireAndForget);
            db.KeyDelete(key, CommandFlags.FireAndForget);
        }

        public List<Sample> SelectAll()
        {
            var db = RedisConnectionPool.CurrentConnection.GetDatabase();
            var all = db.HashGetAll("sample_list");

            List<Sample> temp = new List<Sample>();

            foreach (var he in all)
            {
                if (he.Value.HasValue)
                    temp.Add(BuildSample((int)he.Value, db));
            }

            return temp;
        }
    }
}