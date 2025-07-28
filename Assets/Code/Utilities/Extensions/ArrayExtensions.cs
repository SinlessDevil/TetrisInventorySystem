using System;

namespace Code.Utilities.Extensions
{
    public static class ArrayExtensions
    {
        public static int IndexOf<T>(this T[] array, T value)
        {
            return Array.IndexOf(array, value);
        }
    }
}