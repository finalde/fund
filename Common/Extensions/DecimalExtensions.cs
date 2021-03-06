using System;
namespace Common.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal SafeDivideBy(this decimal source, decimal denominator)
        {
            if (denominator == 0) return 0;
            return source / denominator;
        }
    }
}
