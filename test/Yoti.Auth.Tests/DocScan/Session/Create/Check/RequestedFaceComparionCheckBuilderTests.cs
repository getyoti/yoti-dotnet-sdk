using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Create.Check;

namespace Yoti.Auth.Tests.DocScan.Session.Create.Check
{
    [TestClass]
    public class RequestedFaceComparisonCheckBuilderTests
    {
        [TestMethod]
        public void ShouldBuildWithManualCheckNever()
        {
            RequestedFaceComparisonCheck check =
              new RequestedFaceComparisonCheckBuilder()
              .WithManualCheckNever()
              .Build();

            Assert.AreEqual("NEVER", check.Config.ManualCheck);
        }

        [TestMethod]
        public void ShouldNotBuildWithOutManualCheckBeingSet()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                new RequestedFaceComparisonCheckBuilder().Build();
            });
        }
    }
}