using System;
using System.Linq;

namespace Yoti.Auth.Tests.TestTools
{
    internal class Exceptions
    {
        public static bool IsExceptionInAggregateException<TExceptionToCheck>(AggregateException aggregateException) where TExceptionToCheck : Exception
        {
            bool argumentNullExceptionPresent = aggregateException.InnerExceptions
            .Any(x => x.GetType() == typeof(TExceptionToCheck));

            return argumentNullExceptionPresent;
        }
    }
}