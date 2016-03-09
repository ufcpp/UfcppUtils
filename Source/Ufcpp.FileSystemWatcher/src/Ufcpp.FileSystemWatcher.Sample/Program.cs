using System;
using System.IO;
using System.Linq;

namespace Ufcpp.FileSystemWatcher.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(Path.GetFullPath("sample.txt"));

            using (var loader = new Loader<string[]>("sample.txt", async s =>
            {
                using (var r = new StreamReader(s))
                {
                    var lines = await r.ReadToEndAsync();
                    return lines.Split('\r', '\n')
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .ToArray();
                }
            }))
            {
                loader.Changed += Loader_Changed;

                Console.ReadKey();
            }
        }

        private static void Loader_Changed(string[] lines)
        {
            Console.WriteLine("file changed");

            foreach (var line in lines)
            {
                Console.WriteLine("    " + line);
            }
        }
    }
}
