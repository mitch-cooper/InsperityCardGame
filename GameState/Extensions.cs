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

        public static string GetOrdinalSuffix(this int num)
        {
            string number = num.ToString();
            if (number.EndsWith("11")) return number + "th";
            if (number.EndsWith("12")) return number + "th";
            if (number.EndsWith("13")) return number + "th";
            if (number.EndsWith("1")) return number + "st";
            if (number.EndsWith("2")) return number + "nd";
            if (number.EndsWith("3")) return number + "rd";
            return number + "th";
        }

        public static string RemoveValueAndPadRight(this string value, int paddingNumber, char paddingChar)
        {
            return value.PadRight(paddingNumber, paddingChar).Replace(value, "");
        }
        public static string RemoveValueAndPadLeft(this string value, int paddingNumber, char paddingChar)
        {
            return value.PadLeft(paddingNumber, paddingChar).Replace(value, "");
        }
    }
}
