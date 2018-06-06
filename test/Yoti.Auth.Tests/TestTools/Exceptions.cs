using System;
using System.Linq;

namespace Yoti.Auth.Tests.TestTools
{
    internal class Exceptions
    {
        public static bool IsExceptionInAggregateException<ExceptionToCheck>(AggregateException aggregateException) where ExceptionToCheck : Exception
        {
            bool argumentNullExceptionPresent = aggregateException.InnerExceptions
            .Any(x => x.GetType() == typeof(ExceptionToCheck));

            return argumentNullExceptionPresent;
        }
    }
}