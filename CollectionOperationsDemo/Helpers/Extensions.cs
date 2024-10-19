using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectionOperationsDemo.Helpers;

public static class Extensions
{
    public static string GetCountAsString(IEnumerable<object> source)
    {
        return source.Count().ToString();
    }
}
