using DynamicUtils;
using System;
using System.Collections.Generic;

namespace Ufcpp.DynamicUtils.TestConsole
{
    public class Program
    {
        public void Main(string[] args)
        {
            var x = new RawData { Name = "aaa", Data = new byte[] { 1, 2, 3, 4, 5, 6 } };
            var d = x.AsDictionary();

            var keys = d.Keys;
            Console.WriteLine(string.Join(", ", keys));

            Console.WriteLine(d["Name"]);

            foreach (var item in (byte[])d["Data"])
            {
                Console.WriteLine(item);
            }

            foreach (var key in d.Keys)
            {
                Console.WriteLine(key);
            }
        }
    }

    public class Sample
    {
        public static string Version => "1.1";

        public string Name { get; set; }

        public int? OptionalId { get; set; }
    }

    public class Point : Sample
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class RawData : Sample
    {
        public byte[] Data { get; set; }

        public RawData() { }
        public RawData(int id) { OptionalId = id; }
    }
}
