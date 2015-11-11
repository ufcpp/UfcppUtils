using DynamicUtils;
using System;

namespace Ufcpp.DynamicUtils.TestConsole
{
    public class Program
    {
        public void Main(string[] args)
        {
            var x = new RawData { Name = "aaa", Data = new byte[] { 1, 2, 3, 4, 5, 6 } };
            var d = x.AsDictionary();

            foreach (var item in (byte[])d["Data"])
            {
                Console.WriteLine(item);
            }
        }
    }

    public class Sample
    {
        public string Name { get; set; }
    }

    public class Point : Sample
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class RawData : Sample
    {
        public byte[] Data { get; set; }
    }
}
