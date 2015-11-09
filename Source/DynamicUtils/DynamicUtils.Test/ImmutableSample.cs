using System;

namespace DynamicUtils.Test
{
    class ImmutableSample
    {
        public int Id { get; }
        public DateTime Time { get; }
        public double X { get; }
        public double Y { get; }

        public ImmutableSample(int id, DateTime time, double x, double y)
        {
            Id = id;
            Time = time;
            X = x;
            Y = y;
        }
    }
}
