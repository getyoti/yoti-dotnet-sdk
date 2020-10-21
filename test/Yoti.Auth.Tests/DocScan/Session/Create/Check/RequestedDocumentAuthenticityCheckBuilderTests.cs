using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Create.Check;

namespace Yoti.Auth.Tests.DocScan.Session.Create.Check
{
    [TestClass]
    public class RequestedDocumentAuthenticityCheckBuilderTests
    {
        [TestMethod]
        public void ShouldBuildWithManualCheckAlways()
        {
            RequestedDocumentAuthenticityCheck check =
              new RequestedDocumentAuthenticityCheckBuilder()
              .WithManualCheckAlways()
              .Build();

            Assert.AreEqual("ALWAYS", check.Config.ManualCheck);
        }

        [TestMethod]
        public void ShouldBuildWithManualCheckFallback()
        {
            RequestedDocumentAuthenticityCheck check =
              new RequestedDocumentAuthenticityCheckBuilder()
              .WithManualCheckFallback()
              .Build();

            Assert.AreEqual("FALLBACK", check.Config.ManualCheck);
        }

        [TestMethod]
        public void ShouldBuildWithManualCheckNever()
        {
            RequestedDocumentAuthenticityCheck check =
              new RequestedDocumentAuthenticityCheckBuilder()
              .WithManualCheckNever()
              .Build();

            Assert.AreEqual("NEVER", check.Config.ManualCheck);
        }
    }
}