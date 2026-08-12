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

        [TestMethod]
        public void ShouldBuildWithHandledCheckLimit()
        {
            RequestedFaceComparisonCheck check =
              new RequestedFaceComparisonCheckBuilder()
              .WithManualCheckNever()
              .WithHandledCheckLimit(5)
              .Build();

            Assert.AreEqual(5, check.Config.HandledCheckLimit);
        }

        [TestMethod]
        public void ShouldBuildWithoutHandledCheckLimit()
        {
            RequestedFaceComparisonCheck check =
              new RequestedFaceComparisonCheckBuilder()
              .WithManualCheckNever()
              .Build();

            Assert.IsNull(check.Config.HandledCheckLimit);
        }
    }
}