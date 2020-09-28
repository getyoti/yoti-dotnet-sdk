using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Create.Filter;

namespace Yoti.Auth.Tests.DocScan.Session.Create.Filter
{
    [TestClass]
    public class DocumentRestrictionBuilderTests
    {
        [TestMethod]
        public void ShouldBuildWithCountries()
        {
            DocumentRestriction result = new DocumentRestrictionBuilder()
                .WithCountries(new List<string> { "CO1" })
                .Build();

            Assert.AreEqual("CO1", result.CountryCodes.Single());
            Assert.IsNull(result.DocumentTypes);
        }

        [TestMethod]
        public void ShouldBuildWithDocumentTypes()
        {
            DocumentRestriction result = new DocumentRestrictionBuilder()
                .WithDocumentTypes(new List<string> { "PASSPORT" })
                .Build();

            Assert.AreEqual("PASSPORT", result.DocumentTypes.Single());
            Assert.IsNull(result.CountryCodes);
        }
    }
}