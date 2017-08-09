using System.Collections.Generic;
using Xunit;

namespace TestBindableWrapper
{
    public class Test
    {
        [Fact]
        public void Properties()
        {
            var p = new Point1 { X = 1, Y = 2 };
            var w = new BindableHelper.BindableWrapper<Point1>(p);

            var list = new List<(string name, object value)>();

            w.PropertyChanged += (_, arg) =>
            {
                list.Add((arg.PropertyName, w.GetPropertyValue(arg.PropertyName)));
            };

            Assert.Equal(p.X, w.GetPropertyValue("X"));
            Assert.Equal(p.Y, w.GetPropertyValue("Y"));
            Assert.Equal(0, list.Count);

            w.SetPropertyValue("X", 10);

            Assert.Equal(10, w.Value.X);
            Assert.Equal(2, w.Value.Y);
            Assert.Equal(1, list.Count);
            Assert.Equal("X", list[0].name);
            Assert.Equal(10, list[0].value);

            w.SetPropertyValue("Y", 20);

            Assert.Equal(10, w.Value.X);
            Assert.Equal(20, w.Value.Y);
            Assert.Equal(2, list.Count);
            Assert.Equal("Y", list[1].name);
            Assert.Equal(20, list[1].value);
        }

        [Fact]
        public void Fields()
        {
            var p = new Point2 { X = 1, Y = 2 };
            var w = new BindableHelper.BindableWrapper<Point2>(p);

            var list = new List<(string name, object value)>();

            w.PropertyChanged += (_, arg) =>
            {
                list.Add((arg.PropertyName, w.GetPropertyValue(arg.PropertyName)));
            };

            Assert.Equal(p.X, w.GetPropertyValue("X"));
            Assert.Equal(p.Y, w.GetPropertyValue("Y"));
            Assert.Equal(0, list.Count);

            w.SetPropertyValue("X", 10);

            Assert.Equal(10, w.Value.X);
            Assert.Equal(2, w.Value.Y);
            Assert.Equal(1, list.Count);
            Assert.Equal("X", list[0].name);
            Assert.Equal(10, list[0].value);

            w.SetPropertyValue("Y", 20);

            Assert.Equal(10, w.Value.X);
            Assert.Equal(20, w.Value.Y);
            Assert.Equal(2, list.Count);
            Assert.Equal("Y", list[1].name);
            Assert.Equal(20, list[1].value);
        }

        [Fact]
        public void Mixed()
        {
            var p = new Point3 { X = 1, Y = 2 };
            var w = new BindableHelper.BindableWrapper<Point3>(p);

            var list = new List<(string name, object value)>();

            w.PropertyChanged += (_, arg) =>
            {
                list.Add((arg.PropertyName, w.GetPropertyValue(arg.PropertyName)));
            };

            Assert.Equal(p.X, w.GetPropertyValue("X"));
            Assert.Equal(p.Y, w.GetPropertyValue("Y"));
            Assert.Equal(0, list.Count);

            w.SetPropertyValue("X", 10);

            Assert.Equal(10, w.Value.X);
            Assert.Equal(2, w.Value.Y);
            Assert.Equal(1, list.Count);
            Assert.Equal("X", list[0].name);
            Assert.Equal(10, list[0].value);

            w.SetPropertyValue("Y", 20);

            Assert.Equal(10, w.Value.X);
            Assert.Equal(20, w.Value.Y);
            Assert.Equal(2, list.Count);
            Assert.Equal("Y", list[1].name);
            Assert.Equal(20, list[1].value);
        }

        [Fact]
        public void CProperties()
        {
            var p = new CPoint1 { X = 1, Y = 2 };
            var w = new BindableHelper.BindableWrapper<CPoint1>(p);

            var list = new List<(string name, object value)>();

            w.PropertyChanged += (_, arg) =>
            {
                list.Add((arg.PropertyName, w.GetPropertyValue(arg.PropertyName)));
            };

            Assert.Equal(p.X, w.GetPropertyValue("X"));
            Assert.Equal(p.Y, w.GetPropertyValue("Y"));
            Assert.Equal(0, list.Count);

            w.SetPropertyValue("X", 10);

            Assert.Equal(10, p.X);
            Assert.Equal(2, p.Y);
            Assert.Equal(1, list.Count);
            Assert.Equal("X", list[0].name);
            Assert.Equal(10, list[0].value);

            w.SetPropertyValue("Y", 20);

            Assert.Equal(10, p.X);
            Assert.Equal(20, p.Y);
            Assert.Equal(2, list.Count);
            Assert.Equal("Y", list[1].name);
            Assert.Equal(20, list[1].value);
        }

        [Fact]
        public void CFields()
        {
            var p = new CPoint2 { X = 1, Y = 2 };
            var w = new BindableHelper.BindableWrapper<CPoint2>(p);

            var list = new List<(string name, object value)>();

            w.PropertyChanged += (_, arg) =>
            {
                list.Add((arg.PropertyName, w.GetPropertyValue(arg.PropertyName)));
            };

            Assert.Equal(p.X, w.GetPropertyValue("X"));
            Assert.Equal(p.Y, w.GetPropertyValue("Y"));
            Assert.Equal(0, list.Count);

            w.SetPropertyValue("X", 10);

            Assert.Equal(10, p.X);
            Assert.Equal(2, p.Y);
            Assert.Equal(1, list.Count);
            Assert.Equal("X", list[0].name);
            Assert.Equal(10, list[0].value);

            w.SetPropertyValue("Y", 20);

            Assert.Equal(10, p.X);
            Assert.Equal(20, p.Y);
            Assert.Equal(2, list.Count);
            Assert.Equal("Y", list[1].name);
            Assert.Equal(20, list[1].value);
        }

        [Fact]
        public void CMixed()
        {
            var p = new CPoint3 { X = 1, Y = 2 };
            var w = new BindableHelper.BindableWrapper<CPoint3>(p);

            var list = new List<(string name, object value)>();

            w.PropertyChanged += (_, arg) =>
            {
                list.Add((arg.PropertyName, w.GetPropertyValue(arg.PropertyName)));
            };

            Assert.Equal(p.X, w.GetPropertyValue("X"));
            Assert.Equal(p.Y, w.GetPropertyValue("Y"));
            Assert.Equal(0, list.Count);

            w.SetPropertyValue("X", 10);

            Assert.Equal(10, p.X);
            Assert.Equal(2, p.Y);
            Assert.Equal(1, list.Count);
            Assert.Equal("X", list[0].name);
            Assert.Equal(10, list[0].value);

            w.SetPropertyValue("Y", 20);

            Assert.Equal(10, p.X);
            Assert.Equal(20, p.Y);
            Assert.Equal(2, list.Count);
            Assert.Equal("Y", list[1].name);
            Assert.Equal(20, list[1].value);
        }
    }
}
