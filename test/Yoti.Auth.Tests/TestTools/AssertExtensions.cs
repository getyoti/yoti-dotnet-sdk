using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Yoti.Auth.Tests.TestTools
{
    internal static class AssertExtensions
    {
        public static void DoesNotThrowException(this Assert source, Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            try
            {
                action();
            }
            catch (Exception ex)
            {
                throw new Exception($"Assert.{nameof(DoesNotThrowException)}", ex);
            }
        }
    }
}