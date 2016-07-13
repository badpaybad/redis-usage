using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using badpaybad.redis.repository;
using NUnit.Framework;

namespace badpaybad.redis.repositorytest
{
    [TestFixture]
   public class RedisWithFullnumberSampleTest
    {
        RedisWithFullnumberSample _rfs;

        [SetUp]
        public void Init()
        {
            _rfs=new RedisWithFullnumberSample();
        }

        [Test]
        public void Run()
        {
            Console.WriteLine("init val = 1.001 ");
            var key = "fullnumbersample";

            _rfs.Set(key, 1.001);
            var val = _rfs.Get(key);
            Console.WriteLine("stored in redis = {0}", _rfs.GetValueStored(key));
            Assert.AreEqual(val,1.001);
            
            Console.WriteLine("increase by 0.004");
            _rfs.Increase(key,0.004);
            val = _rfs.Get(key);
            Console.WriteLine("stored in redis = {0}", _rfs.GetValueStored(key));
            Assert.AreEqual(val, 1.005);
            
            Console.WriteLine("increase by -0.002");
            _rfs.Increase(key, -0.002);
            val = _rfs.Get(key);
            Console.WriteLine("stored in redis = {0}", _rfs.GetValueStored(key));
            Assert.AreEqual(val, 1.003);
        }
    }
}
