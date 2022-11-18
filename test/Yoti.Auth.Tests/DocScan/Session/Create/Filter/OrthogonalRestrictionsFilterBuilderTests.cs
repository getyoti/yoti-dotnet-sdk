using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Create.Filter;

namespace Yoti.Auth.Tests.DocScan.Session.Create.Filter
{
    [TestClass]
    public class OrthogonalRestrictionsFilterBuilderTests
    {
        [TestMethod]
        public void WithIncludedCountries()
        {
            OrthogonalRestrictionsFilter result = new OrthogonalRestrictionsFilterBuilder()
                .WithIncludedCountries(new List<string> { "CO1" })
                .Build();

            Assert.AreEqual("ORTHOGONAL_RESTRICTIONS", result.Type);
            Assert.AreEqual("CO1", result.CountryRestriction.CountryCodes.Single());
            Assert.AreEqual("WHITELIST", result.CountryRestriction.Inclusion);
            Assert.IsNull(result.TypeRestriction);
        }

        [TestMethod]
        public void WithExcludedCountries()
        {
            OrthogonalRestrictionsFilter result = new OrthogonalRestrictionsFilterBuilder()
                .WithExcludedCountries(new List<string> { "CO2" })
                .Build();

            Assert.AreEqual("CO2", result.CountryRestriction.CountryCodes.Single());
            Assert.AreEqual("BLACKLIST", result.CountryRestriction.Inclusion);
            Assert.IsNull(result.TypeRestriction);
        }

        [TestMethod]
        public void WithIncludedDocumentTypes()
        {
            OrthogonalRestrictionsFilter result = new OrthogonalRestrictionsFilterBuilder()
                .WithIncludedDocumentTypes(new List<string> { "CO3" })
                .Build();

            Assert.AreEqual("ORTHOGONAL_RESTRICTIONS", result.Type);
            Assert.AreEqual("CO3", result.TypeRestriction.DocumentTypes.Single());
            Assert.AreEqual("WHITELIST", result.TypeRestriction.Inclusion);
            Assert.IsNull(result.CountryRestriction);
        }

        [TestMethod]
        public void WithExcludedDocumentTypes()
        {
            OrthogonalRestrictionsFilter result = new OrthogonalRestrictionsFilterBuilder()
                .WithExcludedDocumentTypes(new List<string> { "CO4" })
                .Build();

            Assert.AreEqual("ORTHOGONAL_RESTRICTIONS", result.Type);
            Assert.AreEqual("CO4", result.TypeRestriction.DocumentTypes.Single());
            Assert.AreEqual("BLACKLIST", result.TypeRestriction.Inclusion);
            Assert.IsNull(result.CountryRestriction);
        }

        [TestMethod]
        public void WithAllowExpiredDocuments()
        {
            OrthogonalRestrictionsFilter result = new OrthogonalRestrictionsFilterBuilder()
                .WithExcludedDocumentTypes(new List<string> { "CO4" })
                .withAllowExpiredDocuments()
                .Build();

            Assert.AreEqual("ORTHOGONAL_RESTRICTIONS", result.Type);
            Assert.AreEqual("CO4", result.TypeRestriction.DocumentTypes.Single());
            Assert.AreEqual("BLACKLIST", result.TypeRestriction.Inclusion);
            Assert.IsTrue(result.AllowExpiredDocuments);
            Assert.IsNull(result.CountryRestriction);
        }

        [TestMethod]
        public void WithDenyExpiredDocuments()
        {
            OrthogonalRestrictionsFilter result = new OrthogonalRestrictionsFilterBuilder()
                .WithExcludedDocumentTypes(new List<string> { "CO4" })
                .withDenyExpiredDocuments()
                .Build();

            Assert.AreEqual("ORTHOGONAL_RESTRICTIONS", result.Type);
            Assert.AreEqual("CO4", result.TypeRestriction.DocumentTypes.Single());
            Assert.AreEqual("BLACKLIST", result.TypeRestriction.Inclusion);
            Assert.IsFalse(result.AllowExpiredDocuments);
            Assert.IsNull(result.CountryRestriction);
        }

        [TestMethod]
        public void WithAllowedNonLatinDocuments()
        {
            OrthogonalRestrictionsFilter result = new OrthogonalRestrictionsFilterBuilder()
                .WithExcludedDocumentTypes(new List<string> { "CO4" })
                .isAllowNonLatinDocuments(true)
                .Build();

            Assert.AreEqual("ORTHOGONAL_RESTRICTIONS", result.Type);
            Assert.AreEqual("CO4", result.TypeRestriction.DocumentTypes.Single());
            Assert.AreEqual("BLACKLIST", result.TypeRestriction.Inclusion);
            Assert.IsTrue(result.AllowNonLatinDocuments);
            Assert.IsNull(result.CountryRestriction);
        }


    }
}