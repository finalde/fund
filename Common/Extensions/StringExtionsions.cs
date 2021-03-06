using System;
using System.Linq;

namespace Common.Extensions
{
    public static class StringExtionsions
    {
        public static string FromPascalToSnakeCase(this string source)
        {
            var result = string.Concat(source.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
            return result;
        }
    }
}
