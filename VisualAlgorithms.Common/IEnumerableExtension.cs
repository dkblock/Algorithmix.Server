using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VisualAlgorithms.Common
{
    public static class IEnumerableExtension
    {
        public static async Task ForEachAsync<T>(this IEnumerable<T> collection, Func<T, Task> func)
        {
            foreach (var item in collection)
                await func(item);
        }
    }
}
