using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algorithmix.Common.Extensions
{
    public static class IEnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> func)
        {
            foreach (var item in collection)
                func(item);
        }

        public static async Task ForEachAsync<T>(this IEnumerable<T> collection, Func<T, Task> func)
        {
            foreach (var item in collection)
                await func(item);
        }
    }
}
