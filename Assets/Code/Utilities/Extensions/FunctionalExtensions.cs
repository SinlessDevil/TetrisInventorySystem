using System;

namespace Code.Utilities.Extensions
{
    public static class FunctionalExtensions
    {
        public static T With<T>(this T self, Action<T> set)
        {
            set.Invoke(self);
            return self;
        }
    }
}