using System;

namespace DynamicUtils.Test
{
    class Class1
    {
        public int Id { get; }
        public DateTime Time { get; }
        public double X { get; }
        public double Y { get; }

        public Class1(int id, DateTime time, double x, double y)
        {
            Id = id;
            Time = time;
            X = x;
            Y = y;
        }
    }
}
