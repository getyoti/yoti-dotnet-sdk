using System;

namespace Yoti.Auth
{
    internal static class Validation
    {
        public static void NotNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException($"{name} must not be null.");
            }
        }

        public static void NotGreaterThan<T>(T value, T upperLimit, string name) where T : IComparable<T>
        {
            if (value.CompareTo(upperLimit) > 0)
                throw new ArgumentException($"{name} value {value} is greater than the upper limit of {upperLimit}");
        }

        public static void NotLessThan<T>(T value, T lowerLimit, string name) where T : IComparable<T>
        {
            if (value.CompareTo(lowerLimit) < 0)
                throw new ArgumentException($"{name} value {value} is less than the lower limit of {lowerLimit}");
        }

        public static void WithinRange<T>(T value, T lowerLimit, T upperLimit, string name) where T : IComparable<T>
        {
            NotLessThan(value, lowerLimit, name);
            NotGreaterThan(value, upperLimit, name);
        }
    }
}