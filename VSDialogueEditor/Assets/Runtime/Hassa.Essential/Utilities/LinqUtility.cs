using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Hassa.Essentials
{

    public static class LinqUtility
    {
        public static IEnumerable<T> Concat<T>(params IEnumerable[] enumerables)
        {
            foreach (var enumerable in enumerables.NotNull())
            {
                foreach (var item in enumerable.OfType<T>())
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            return items.GroupBy(property).Select(x => x.First());
        }

        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Where(i => i != null);
        }

        public static IEnumerable<T> Yield<T>(this T t)
        {
            yield return t;
        }
    }

}