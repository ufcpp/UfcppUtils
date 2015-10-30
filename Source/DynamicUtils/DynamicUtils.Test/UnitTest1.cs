using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicUtils.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAccessorForClass1()
        {
            var p = new Class1(1, DateTime.Now, 10, 20);
            var d = p.AsDictionary();

            Assert.AreEqual(p.Id, d[nameof(p.Id)]);
            Assert.AreEqual(p.Time, d[nameof(p.Time)]);
            Assert.AreEqual(p.X, d[nameof(p.X)]);
            Assert.AreEqual(p.Y, d[nameof(p.Y)]);
        }

        [TestMethod]
        public void TestAccessorForClass2()
        {
            var p = new Class2();
            var d = p.AsDictionary();

            d[nameof(p.Id)] = 1;
            d[nameof(p.Time)] = DateTime.Now;
            d[nameof(p.X)] = 10.0;
            d[nameof(p.Y)] = 20.0;

            Assert.AreEqual(p.Id, d[nameof(p.Id)]);
            Assert.AreEqual(p.Time, d[nameof(p.Time)]);
            Assert.AreEqual(p.X, d[nameof(p.X)]);
            Assert.AreEqual(p.Y, d[nameof(p.Y)]);
        }

        [TestMethod]
        public void TestUtilForClass1()
        {
            var p = new Class1(1, DateTime.Now, 10, 20);
            var getter = ReflectionUtil.GetGetter<Class1>();

            Assert.AreEqual(p.Id, getter(p, nameof(p.Id)));
            Assert.AreEqual(p.Time, getter(p, nameof(p.Time)));
            Assert.AreEqual(p.X, getter(p, nameof(p.X)));
            Assert.AreEqual(p.Y, getter(p, nameof(p.Y)));
        }

        [TestMethod]
        public void TestUtilForClass2()
        {
            var p = new Class2();
            var getter = ReflectionUtil.GetGetter<Class2>();
            var setter = ReflectionUtil.GetSetter<Class2>();

            setter(p, nameof(p.Id), 1);
            setter(p, nameof(p.Time), DateTime.Now);
            setter(p, nameof(p.X), 10.0);
            setter(p, nameof(p.Y), 20.0);

            Assert.AreEqual(p.Id, getter(p, nameof(p.Id)));
            Assert.AreEqual(p.Time, getter(p, nameof(p.Time)));
            Assert.AreEqual(p.X, getter(p, nameof(p.X)));
            Assert.AreEqual(p.Y, getter(p, nameof(p.Y)));
        }
    }
}
