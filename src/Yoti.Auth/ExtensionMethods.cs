using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Yoti.Auth
{
    public static class ExtensionMethods
    {
        public static void SetPropertyValue<T, TValue>(this T target, Expression<Func<T, TValue>> memberLamda, TValue value)
        {
            if (memberLamda.Body is MemberExpression memberSelectorExpression)
            {
                if (memberSelectorExpression.Member is PropertyInfo property)
                {
                    property.SetValue(target, value, null);
                }
            }
        }
    }
}