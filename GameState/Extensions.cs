using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameState
{
    internal static class Extensions
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }
    }
}
