using System;
using System.Reflection;

namespace Yoti.Auth.Tests.TestTools
{
    internal static class DefaultValueComparer
    {
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