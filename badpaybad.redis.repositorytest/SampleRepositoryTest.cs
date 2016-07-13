using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using badpaybad.redis.repository;
using NUnit.Framework;

namespace badpaybad.redis.repositorytest
{
    [TestFixture]
    public class SampleRepositoryTest
    {
        [Test]
        public void CleanAllChickChat()
        {
           // new ChickChatServices().CleanAll();
            var x = 3;
            Increase(x);
            Assert.IsTrue(x==3);

            Console.WriteLine(x);
        }

        void Increase(int i)
        {
            i = i + 1;
        }

        private SampleRepository _repo;

        [SetUp]
        public void Init()
        {
            _repo = new SampleRepository();
        }

        [Test]
        public void Run()
        {
            _repo.AddOrUpdate(new Sample()
            {
                Id = 1,
                Version = "ver 1"
            });

            _repo.AddOrUpdate(new Sample()
            {
                Id = 2,
                Version = "ver 2"
            });
            Console.WriteLine("select all");
            var list = _repo.SelectAll();
            Console.WriteLine(new JavaScriptSerializer().Serialize(list));

            Console.WriteLine("select 1 ");
            var x1 = _repo.Select(1);
            Console.WriteLine(new JavaScriptSerializer().Serialize(x1));

            _repo.Delete(1);

            Console.WriteLine("after delete");
            var listafterdel = _repo.SelectAll();

            Console.WriteLine(new JavaScriptSerializer().Serialize(listafterdel));
        }

        [Test]
        public void StressTest()
        {

            for (int i = 1; i < 150; i++)
            {
                var begin = i * 100000;
                var end = begin + 500001;
                new Thread(() => { InsertThenDelete(begin, end); }).Start();
            }

            while (true)
            {
                Thread.Sleep(100);
            }
        }

        void InsertThenDelete(int begin = 0, int end = 10000)
        {
            for (int id = begin; id < end; id++)
            {
                //_repo.AddOrUpdate(new Sample()
                //{
                //    Id = id,
                //    Version = "version " + id
                //});

                _repo.Delete(id);
            }

            Console.WriteLine("complete from {0} to {1}", begin, end);
        }
    }
}
