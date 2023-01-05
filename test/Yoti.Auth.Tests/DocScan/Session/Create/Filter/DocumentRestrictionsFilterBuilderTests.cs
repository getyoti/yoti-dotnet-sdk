using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Create.Filter;

namespace Yoti.Auth.Tests.DocScan.Session.Create.Filter
{
    [TestClass]
    public class DocumentRestrictionsFilterBuilderTests
    {
        [TestMethod]
        public void LastInclusionShouldOverrideFirstInclusion()
        {
            DocumentRestrictionsFilter result = new DocumentRestrictionsFilterBuilder()
                .ForIncludeList()
                .ForExcludeList()
                .Build();

            Assert.AreEqual("BLACKLIST", result.Inclusion);
        }

        [TestMethod]
        public void ShouldBuildWithDocumentRestrictionArguments()
        {
            DocumentRestrictionsFilter result = new DocumentRestrictionsFilterBuilder()
              .WithDocumentRestriction(new List<string> { "FRA" }, new List<string> { "PASSPORT" })
              .ForExcludeList()
              .Build();

            Assert.AreEqual("FRA", result.Documents[0].CountryCodes.Single());
            Assert.AreEqual("PASSPORT", result.Documents[0].DocumentTypes.Single());
        }

        [TestMethod]
        public void ShouldBuildWithDocumentRestrictionObject()
        {
            DocumentRestrictionsFilter result = new DocumentRestrictionsFilterBuilder()
              .WithDocumentRestriction(new DocumentRestriction(new List<string> { "USA" }, new List<string> { "NATIONAL_ID" }))
              .ForIncludeList()
              .Build();

            Assert.AreEqual("USA", result.Documents[0].CountryCodes.Single());
            Assert.AreEqual("NATIONAL_ID", result.Documents[0].DocumentTypes.Single());
        }

        [TestMethod]
        public void ShouldBuildWithAllowExpiredDocuments()
        {
            DocumentRestrictionsFilter result = new DocumentRestrictionsFilterBuilder()
               .WithDocumentRestriction(new DocumentRestriction(new List<string> { "USA" }, new List<string> { "NATIONAL_ID" }))
                 .ForIncludeList()
                 .withAllowExpiredDocuments()
                .Build();

            Assert.AreEqual("USA", result.Documents[0].CountryCodes.Single());
            Assert.AreEqual("NATIONAL_ID", result.Documents[0].DocumentTypes.Single());
            Assert.IsTrue(result.AllowExpiredDocuments);
        }


        [TestMethod]
        public void ShouldBuildWithDenyExpiredDocuments()
        {
            DocumentRestrictionsFilter result = new DocumentRestrictionsFilterBuilder()
               .WithDocumentRestriction(new DocumentRestriction(new List<string> { "USA" }, new List<string> { "NATIONAL_ID" }))
                 .ForIncludeList()
                 .withDenyExpiredDocuments()
                .Build();
            Assert.AreEqual("USA", result.Documents[0].CountryCodes.Single());
            Assert.AreEqual("NATIONAL_ID", result.Documents[0].DocumentTypes.Single());
            Assert.IsFalse(result.AllowExpiredDocuments);
        }
    }
}