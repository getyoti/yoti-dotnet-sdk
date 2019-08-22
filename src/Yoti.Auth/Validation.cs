using System;
using System.Reflection;

namespace Yoti.Auth
{
    internal static class Validation
    {
        public static void NotNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void NotNullOrEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException($"'{name}' must not be empty or null");
            }
        }

        public static void NotGreaterThan<T>(T value, T upperLimit, string name) where T : IComparable<T>
        {
            if (value.CompareTo(upperLimit) > 0)
                throw new ArgumentOutOfRangeException($"{name} value {value} is greater than the upper limit of {upperLimit}");
        }

        public static void NotLessThan<T>(T value, T lowerLimit, string name) where T : IComparable<T>
        {
            if (value.CompareTo(lowerLimit) < 0)
                throw new ArgumentOutOfRangeException($"{name} value {value} is less than the lower limit of {lowerLimit}");
        }

        public static void WithinRange<T>(T value, T lowerLimit, T upperLimit, string name) where T : IComparable<T>
        {
            NotLessThan(value, lowerLimit, name);
            NotGreaterThan(value, upperLimit, name);
        }

        public static void IsNotDefault(this object value, string name)
        {
            if (value.IsDefault())
                throw new InvalidOperationException(
                    $"the value of '{name}' must not be equal to the default value for '{value.GetType().ToString()}'");
        }

        public static bool IsDefault(this object value)
        {
            if (value == null)
                return true;

            if (!value.GetType().GetTypeInfo().IsValueType)
                return false;

            object defaultValue = Activator.CreateInstance(value.GetType());
            return value.Equals(defaultValue);
        }
    }
}