using System;
using System.Linq;

namespace FundImporter.Services
{
    public interface IConvertService
    {
        int ConvertToInt(string value);
        decimal ConvertToDecimal(string value);
        decimal? ConvertToNullableDecimal(string value);
        DateTime ConvertToDate(string value);
    }
    public class ConvertService: IConvertService
    {
        public int ConvertToInt(string value)
        {
            if (value is null) return 0;
            value = value.Trim();
            if (string.IsNullOrEmpty(value)) return 0;
            if (value == "-" || value == "--" || value == "--元" || value == "-- 元" || value == "---") return 0;
            value = value.Replace(",", "");
            if (value.Contains("%"))
            {
                value = value.Replace("%", "").Trim();
                if (string.IsNullOrEmpty(value)) return 0;
                return Convert.ToInt32(value) / 100;
            }
            if (value.Contains("亿元"))
            {
                value = value.Split("亿元").First().Trim();
                if (string.IsNullOrEmpty(value)) return 0;
                return Convert.ToInt32(value) * 100000000;
            }
            if (value.Contains("亿"))
            {
                value = value.Split("亿").First().Trim();
                if (string.IsNullOrEmpty(value)) return 0;
                return Convert.ToInt32(value) * 100000000;
            }
            return Convert.ToInt32(value);
        }
        public decimal ConvertToDecimal(string value)
        {
            if (value is null) return 0;
            value = value.Trim();
            if (string.IsNullOrEmpty(value)) return 0;
            if (value == "-" || value == "--" || value == "--元" || value == "-- 元" || value == "---") return 0;
            value = value.Replace(",", "");
            if (value.Contains("%"))
            {
                value = value.Replace("%", "").Trim();
                if (string.IsNullOrEmpty(value)) return 0;
                return Convert.ToDecimal(value) / 100;
            }
            if (value.Contains("亿元"))
            {
                value = value.Split("亿元").First().Trim();
                if (string.IsNullOrEmpty(value)) return 0;
                return Convert.ToDecimal(value) * 100000000;
            }
            if (value.Contains("亿"))
            {
                value = value.Split("亿").First().Trim();
                if (string.IsNullOrEmpty(value)) return 0;
                return Convert.ToDecimal(value) * 100000000;
            }
            return Convert.ToDecimal(value);
        }
        public decimal? ConvertToNullableDecimal(string value)
        {
            if (value is null) return null;
            value = value.Trim();
            if (string.IsNullOrEmpty(value)) return null;
            if (value == "-" || value == "--" || value == "--元" || value == "-- 元" || value == "---") return null;
            value = value.Replace(",", "");
            if (value.Contains("%"))
            {
                value = value.Replace("%", "").Trim();
                if (string.IsNullOrEmpty(value)) return null;
                return Convert.ToDecimal(value) / 100;
            }
            if (value.Contains("亿元"))
            {
                value = value.Split("亿元").First().Trim();
                if (string.IsNullOrEmpty(value)) return null;
                return Convert.ToDecimal(value) * 100000000;
            }
            if (value.Contains("亿"))
            {
                value = value.Split("亿").First().Trim();
                if (string.IsNullOrEmpty(value)) return null;
                return Convert.ToDecimal(value) * 100000000;
            }
            return Convert.ToDecimal(value);
        }
        public DateTime ConvertToDate(string value)
        {
            if (string.IsNullOrEmpty(value)) return new DateTime();
            value = value.Trim();
            if (value.Length < 8) return new DateTime();
            if(value.Length == 8)
            {
                var year = Convert.ToInt32(value.Substring(0, 4));
                var month = Convert.ToInt32(value.Substring(4, 2));
                var day = Convert.ToInt32(value.Substring(6, 2));
                return new DateTime(year, month, day);
            }
            if(value.Length == 10 && value.Contains("-"))
            {
                var arr = value.Split('-');
                if (arr.Length == 3)
                {
                    var year = Convert.ToInt32(arr[0]);
                    var month = Convert.ToInt32(arr[1]);
                    var day = Convert.ToInt32(arr[2]);
                    return new DateTime(year, month, day);
                }
            }
            return new DateTime();
        }
    }
}
