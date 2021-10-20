using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Yoti.Auth.DocScan.Session.Create.Check;
using Yoti.Auth.DocScan.Session.Create.Filter;

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

        [TestMethod]
        public void ShouldBuildWithIssuingAuthoritySubCheckToGiveDefaultObject()
        {
            RequestedDocumentAuthenticityCheck check =
              new RequestedDocumentAuthenticityCheckBuilder()
              .WithIssuingAuthoritySubCheck()
              .Build();

            Assert.IsTrue(check.Config.IssuingAuthoritySubCheck.Requested);
            Assert.IsNull(check.Config.IssuingAuthoritySubCheck.Filter);
        }

        [TestMethod]
        public void ShouldBuildWithIssuingAuthoritySubCheck()
        {
            var filter = new OrthogonalRestrictionsFilterBuilder()
               .WithExcludedCountries(new List<string> { "GBR", "FRA" })
               .WithExcludedDocumentTypes(new List<string> { "PASSPORT", "STATE_ID" })
               .Build();

            RequestedDocumentAuthenticityCheck check =
              new RequestedDocumentAuthenticityCheckBuilder()
              .WithIssuingAuthoritySubCheck(filter)
              .Build();

            Assert.IsTrue(check.Config.IssuingAuthoritySubCheck.Requested);
            Assert.IsNotNull(check.Config.IssuingAuthoritySubCheck.Filter);
            Assert.AreEqual(filter, check.Config.IssuingAuthoritySubCheck.Filter);
        }

        [TestMethod]
        public void ShouldBuildWithIssuingAuthoritySubCheckVariation()
        {
            var filter = new DocumentRestrictionsFilterBuilder()
                      .ForIncludeList()
                      .WithDocumentRestriction(
                          new List<string> { "USA" },
                          new List<string> { "PASSPORT" })
                      .Build();

            RequestedDocumentAuthenticityCheck check =
              new RequestedDocumentAuthenticityCheckBuilder()
              .WithIssuingAuthoritySubCheck(filter)
              .Build();

            Assert.IsTrue(check.Config.IssuingAuthoritySubCheck.Requested);
            Assert.IsNotNull(check.Config.IssuingAuthoritySubCheck.Filter);
            Assert.AreEqual(filter, check.Config.IssuingAuthoritySubCheck.Filter);
        }
    }
}