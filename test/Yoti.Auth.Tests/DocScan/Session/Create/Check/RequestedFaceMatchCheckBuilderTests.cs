using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Constants;
using Yoti.Auth.DocScan.Session.Create.Check;

namespace Yoti.Auth.Tests.DocScan.Session.Create.Check
{
    [TestClass]
    public class RequestedFaceMatchCheckBuilderTests
    {
        [TestMethod]
        public void ShouldBuildWithManualCheckAlways()
        {
            RequestedFaceMatchCheck check =
              new RequestedFaceMatchCheckBuilder()
              .WithManualCheckAlways()
              .Build();

            Assert.AreEqual(DocScanConstants.IdDocumentFaceMatch, check.Type);
            Assert.AreEqual("ALWAYS", check.Config.ManualCheck);
        }

        [TestMethod]
        public void ShouldBuildWithManualCheckFallback()
        {
            RequestedFaceMatchCheck check =
              new RequestedFaceMatchCheckBuilder()
              .WithManualCheckFallback()
              .Build();

            Assert.AreEqual("FALLBACK", check.Config.ManualCheck);
        }

        [TestMethod]
        public void ShouldBuildWithManualCheckNever()
        {
            RequestedFaceMatchCheck check =
              new RequestedFaceMatchCheckBuilder()
              .WithManualCheckNever()
              .Build();

            Assert.AreEqual("NEVER", check.Config.ManualCheck);
        }

        [TestMethod]
        public void ShouldNotBuildWithOutManualCheckBeingSet()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                new RequestedFaceMatchCheckBuilder().Build();
            });
        }
    }
}