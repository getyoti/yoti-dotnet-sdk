using System;
using System.Linq;

namespace Yoti.Auth.Tests.TestTools
{
    internal static class Exceptions
    {
        public static bool IsExceptionInAggregateException<TExceptionToCheck>(AggregateException aggregateException) where TExceptionToCheck : Exception
        {
            return aggregateException.InnerExceptions
            .Any(x => x.GetType() == typeof(TExceptionToCheck));
        }
    }
}