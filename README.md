# DynamicUtils

Utilities for reflection, expression trees, dynamic features.

NuGet package: https://www.nuget.org/packages/Ufcpp.DynamicUtils/

## DictionaryAccessor

### Summary

get/set properties through `IDictionary<string, object>` with property names as a key by using Expression Trees.

### Sample

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




