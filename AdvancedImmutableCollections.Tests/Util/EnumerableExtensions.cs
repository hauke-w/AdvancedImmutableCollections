using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedImmutableCollections.Tests.Util;
internal static class EnumerableExtensions
{
    /// <summary>
    /// Wraps the enumerable in a new enumerable that enumerates the items of the original enumerable.
    /// This is particularly useful for particularly whitebox testing where a cast to another collection type is undesired.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <returns></returns>
    public static IEnumerable<T> WrapAsEnumerable<T>(this IEnumerable<T> enumerable)
    {
        foreach (var item in enumerable)
        {
            yield return item;
        }
    }
}
