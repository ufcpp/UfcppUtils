# UfcppUtils

A collection of NuGet packages

## Ufcpp.DynamicUtils

Utilities for reflection, expression trees, dynamic features.

NuGet package: https://www.nuget.org/packages/Ufcpp.DynamicUtils/

### DictionaryAccessor

get/set properties through `IDictionary<string, object>` with property names as a key by using Expression Trees.

#### Sample

```cs
var p = new MutableSample();
var d = p.AsDictionary();

d["Id"] = 1;
d["Time"] = DateTime.Now;
d["X"] = 10.0;
d["Y"] = 20.0;

Assert.Equal(p.Id, d["Id"]);
Assert.Equal(p.Time, d["Time"]);
Assert.Equal(p.X, d["X");
Assert.Equal(p.Y, d["Y"]);
```

## Ufcpp.FileSystemWatcher

dynamically loads an object from a serialized file with `System.IO.FileSystemWatcher`.

#### Sample

```cs
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
```
