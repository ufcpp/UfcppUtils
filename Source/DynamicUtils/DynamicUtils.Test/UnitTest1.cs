using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DynamicUtils.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAccessorForRecursive()
        {
            var m = new MutableSample();
            {
                var d = m.AsDictionary();

                d["Id"] = 1;
                d["Time"] = DateTime.Now;
                d["X"] = 10.0;
                d["Y"] = 20.0;
            }

            var r1 = new RecursiveSample
            {
                X = new ImmutableSample(2, DateTime.Now.AddDays(1), 11, 21),
                Y = m,
            };
            var r2 = new RecursiveSample
            {
                X = new ImmutableSample(3, DateTime.Now.AddDays(2), 12, 22),
            };

            var d1 = r1.AsDictionary();
            var d2 = r2.AsDictionary();

            d2["Y"] = d1["Y"];

            Assert.IsNotNull(r2.Y);
            Assert.AreEqual(r1.Y, r2.Y);

            Assert.AreEqual(1, r1.Y.Id);
            Assert.AreEqual(10.0, r1.Y.X);
            Assert.AreEqual(20.0, r1.Y.Y);

            var dx1 = d1["X"] as IDictionary<string, object>;
            Assert.IsNotNull(dx1);
            Assert.AreEqual(r1.X.Id, dx1["Id"]);
            Assert.AreEqual(r1.X.Time, dx1["Time"]);
            Assert.AreEqual(r1.X.X, dx1["X"]);
            Assert.AreEqual(r1.X.Y, dx1["Y"]);

            var dx2 = d2["X"] as IDictionary<string, object>;
            Assert.IsNotNull(dx2);
            Assert.AreEqual(r2.X.Id, dx2["Id"]);
            Assert.AreEqual(r2.X.Time, dx2["Time"]);
            Assert.AreEqual(r2.X.X, dx2["X"]);
            Assert.AreEqual(r2.X.Y, dx2["Y"]);
        }

        [TestMethod]
        public void TestAccessorForImmutable()
        {
            var p = new ImmutableSample(1, DateTime.Now, 10, 20);
            var d = p.AsDictionary();

            Assert.AreEqual(p.Id, d[nameof(p.Id)]);
            Assert.AreEqual(p.Time, d[nameof(p.Time)]);
            Assert.AreEqual(p.X, d[nameof(p.X)]);
            Assert.AreEqual(p.Y, d[nameof(p.Y)]);
        }

        [TestMethod]
        public void TestAccessorForMutable()
        {
            var p = new MutableSample();
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
        public void TestUtilForImmutable()
        {
            var p = new ImmutableSample(1, DateTime.Now, 10, 20);
            var getter = ReflectionUtil.GetGetter<ImmutableSample>();

            Assert.AreEqual(p.Id, getter(p, nameof(p.Id)));
            Assert.AreEqual(p.Time, getter(p, nameof(p.Time)));
            Assert.AreEqual(p.X, getter(p, nameof(p.X)));
            Assert.AreEqual(p.Y, getter(p, nameof(p.Y)));
        }

        [TestMethod]
        public void TestUtilForMutable()
        {
            var p = new MutableSample();
            var getter = ReflectionUtil.GetGetter<MutableSample>();
            var setter = ReflectionUtil.GetSetter<MutableSample>();

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
