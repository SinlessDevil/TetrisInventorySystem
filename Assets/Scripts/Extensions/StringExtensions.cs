using System;
using System.Linq;

namespace Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static string ToTitleCase(this string str)
        {
            if (str.IsNullOrEmpty()) return str;
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        public static string RemoveWhitespace(this string str)
        {
            return new string(str.ToCharArray()
              .Where(c => !Char.IsWhiteSpace(c))
              .ToArray());
        }

        public static bool ContainsIgnoreCase(this string str, string value)
        {
            if (str.IsNullOrEmpty() || value.IsNullOrEmpty()) return false;
            return str.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool EqualsIgnoreCase(this string str, string value)
        {
            if (str.IsNullOrEmpty() && value.IsNullOrEmpty()) return true;
            if (str.IsNullOrEmpty() || value.IsNullOrEmpty()) return false;
            return str.Equals(value, StringComparison.OrdinalIgnoreCase);
        }
    }
}