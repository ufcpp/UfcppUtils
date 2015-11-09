using System;
using System.Collections.Generic;
using Xunit;

namespace DynamicUtils.Test
{
    public class UnitTest1
    {
        [Fact]
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

            Assert.NotNull(r2.Y);
            Assert.Equal(r1.Y, r2.Y);

            Assert.Equal(1, r1.Y.Id);
            Assert.Equal(10.0, r1.Y.X);
            Assert.Equal(20.0, r1.Y.Y);

            var dx1 = d1["X"] as IDictionary<string, object>;
            Assert.NotNull(dx1);
            Assert.Equal(r1.X.Id, dx1["Id"]);
            Assert.Equal(r1.X.Time, dx1["Time"]);
            Assert.Equal(r1.X.X, dx1["X"]);
            Assert.Equal(r1.X.Y, dx1["Y"]);

            var dx2 = d2["X"] as IDictionary<string, object>;
            Assert.NotNull(dx2);
            Assert.Equal(r2.X.Id, dx2["Id"]);
            Assert.Equal(r2.X.Time, dx2["Time"]);
            Assert.Equal(r2.X.X, dx2["X"]);
            Assert.Equal(r2.X.Y, dx2["Y"]);
        }

        [Fact]
        public void TestAccessorForImmutable()
        {
            var p = new ImmutableSample(1, DateTime.Now, 10, 20);
            var d = p.AsDictionary();

            Assert.Equal(p.Id, d[nameof(p.Id)]);
            Assert.Equal(p.Time, d[nameof(p.Time)]);
            Assert.Equal(p.X, d[nameof(p.X)]);
            Assert.Equal(p.Y, d[nameof(p.Y)]);
        }

        [Fact]
        public void TestAccessorForMutable()
        {
            var p = new MutableSample();
            var d = p.AsDictionary();

            d[nameof(p.Id)] = 1;
            d[nameof(p.Time)] = DateTime.Now;
            d[nameof(p.X)] = 10.0;
            d[nameof(p.Y)] = 20.0;

            Assert.Equal(p.Id, d[nameof(p.Id)]);
            Assert.Equal(p.Time, d[nameof(p.Time)]);
            Assert.Equal(p.X, d[nameof(p.X)]);
            Assert.Equal(p.Y, d[nameof(p.Y)]);
        }

        [Fact]
        public void TestUtilForImmutable()
        {
            var p = new ImmutableSample(1, DateTime.Now, 10, 20);
            var getter = ReflectionUtil.GetGetter<ImmutableSample>();

            Assert.Equal(p.Id, getter(p, nameof(p.Id)));
            Assert.Equal(p.Time, getter(p, nameof(p.Time)));
            Assert.Equal(p.X, getter(p, nameof(p.X)));
            Assert.Equal(p.Y, getter(p, nameof(p.Y)));
        }

        [Fact]
        public void TestUtilForMutable()
        {
            var p = new MutableSample();
            var getter = ReflectionUtil.GetGetter<MutableSample>();
            var setter = ReflectionUtil.GetSetter<MutableSample>();

            setter(p, nameof(p.Id), 1);
            setter(p, nameof(p.Time), DateTime.Now);
            setter(p, nameof(p.X), 10.0);
            setter(p, nameof(p.Y), 20.0);

            Assert.Equal(p.Id, getter(p, nameof(p.Id)));
            Assert.Equal(p.Time, getter(p, nameof(p.Time)));
            Assert.Equal(p.X, getter(p, nameof(p.X)));
            Assert.Equal(p.Y, getter(p, nameof(p.Y)));
        }
    }
}
