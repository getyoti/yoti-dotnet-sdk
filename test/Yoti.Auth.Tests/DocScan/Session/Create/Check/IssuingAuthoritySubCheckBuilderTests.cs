using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Yoti.Auth.DocScan.Session.Create.Check;
using Yoti.Auth.DocScan.Session.Create.Filter;

namespace Yoti.Auth.Tests.DocScan.Session.Create.Check
{
    [TestClass]
    public class IssuingAuthoritySubCheckBuilderTests
    {
        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void ShouldBuildWithCorrectRequestedValue(bool requested)
        {
            IssuingAuthoritySubCheck check =
              new IssuingAuthoritySubCheckBuilder()
              .WithRequested(requested)
              .Build();

            Assert.AreEqual(requested, check.Requested);
        }

        [TestMethod]
        public void ShouldBuildWithCorrectFilterValue()
        {
            var filter = new OrthogonalRestrictionsFilterBuilder()
                .WithIncludedCountries(new List<string> { "GBR", "FRA" })
                .WithIncludedDocumentTypes(new List<string> { "PASSPORT", "STATE_ID" })
                .Build();

            IssuingAuthoritySubCheck check =
              new IssuingAuthoritySubCheckBuilder()
              .WithFilter(filter)
              .Build();

            Assert.AreEqual(filter, check.Filter);
        }
    }
}