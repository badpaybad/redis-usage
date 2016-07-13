using System;
using System.Threading;
using badpaybad.redis.repository;
using NUnit.Framework;

namespace badpaybad.redis.repositorytest
{

    public class Program: OverLoadMethodTest
    {
        [STAThread]
        public static void Main()
        {
            OverLoadMethodTest t=new OverLoadMethodTest();

            t.Method1("Du", 31);
          
            t.Method1( 31,"age then name");
            t.Method1("Du", 31, "127 van cao");

            t.Method1(name:"xxx", age:23);

            Console.ReadLine();
        }
    }

    public class OverLoadMethodTest
    {
        public void Method1(string name, int age, string address="")
        {
            Console.WriteLine("Method with 3 param, name:{0}, age:{1}, address:{2}", name, age, address);
        }

        //public void Method1(string name, int age)
        //{
        //    Console.WriteLine("Method with 2 param, name:{0}, age:{1}", name, age);
        //}

        public void Method1(int age, string name)
        {
            Console.WriteLine("Method 2 param age name , age:{0}, name:{1}", age, name);

        }
    }

    [TestFixture]
    public class CustomTaskScheduleTest
    {
        OverLoadMethodTest _overLoadMethodTest=new OverLoadMethodTest();

        [Test]
        public void TestMethodOverload()
        {
            _overLoadMethodTest.Method1("Du",31);
        }

        CustomTaskScheduler _customTaskScheduler=new CustomTaskScheduler();

        [Test]
        public void Run()
        {
            _customTaskScheduler.Add("xxx",()=> {});
        }
    }

    [TestFixture]
    public class RedisAsCacheSampleTest
    {
        private RedisAsCacheSample _rac;

        [SetUp]
        public void Init()
        {
            _rac = new RedisAsCacheSample();
        }

        [Test]
        public void Run()
        {
            _rac.AddOrUpdate("exp_in_1_secs", "hello exp in 1 sec", new TimeSpan(0, 0, 1));
            _rac.AddOrUpdate("exp_in_5_secs", "hello exp in 5 sec", new TimeSpan(0, 0, 5));
            _rac.AddOrUpdate("exp_no", "no exp", null);

            Thread.Sleep(2000);
            Console.WriteLine("after 2 sec : exp_in_1_secs will null, exp_in_5_secs lived, exp_no lived");

            var exp1 = _rac.Get<string>("exp_in_1_secs");
            var exp5_1 = _rac.Get<string>("exp_in_5_secs");
            var expno = _rac.Get<string>("exp_no");

            Assert.IsTrue(exp1 == null);
            Assert.IsTrue(exp5_1 != null);
            Assert.IsTrue(expno != null);

            Thread.Sleep(6000);

            Console.WriteLine("after 5 sec : exp_in_1_secs  null, exp_in_5_secs  null, exp_no lived");

            exp1 = _rac.Get<string>("exp_in_1_secs");
            exp5_1 = _rac.Get<string>("exp_in_5_secs");
            expno = _rac.Get<string>("exp_no");

            Assert.IsTrue(exp1 == null);
            Assert.IsTrue(exp5_1 == null);
            Assert.IsTrue(expno != null);
        }
    }
}