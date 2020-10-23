using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Create.Filter;
using Yoti.Auth.DocScan.Session.Create.Objectives;

namespace Yoti.Auth.Tests.DocScan.Session.Create.Filter
{
    [TestClass]
    public class RequiredSupplementaryDocumentBuilderTests
    {
        [TestMethod]
        public void ShouldBuildWithObjective()
        {
            var proofOfAddress = new ProofOfAddressObjectiveBuilder().Build();

            RequiredSupplementaryDocument result = new RequiredSupplementaryDocumentBuilder()
                .WithObjective(proofOfAddress)
                .Build();

            Assert.AreEqual(proofOfAddress, result.Objective);
            Assert.AreEqual("PROOF_OF_ADDRESS", result.Objective.Type);
            Assert.AreEqual("SUPPLEMENTARY_DOCUMENT", result.Type);
        }

        [TestMethod]
        public void ShouldBuildWithCountries()
        {
            RequiredSupplementaryDocument result = new RequiredSupplementaryDocumentBuilder()
                .WithCountries(new List<string> { "CO1" })
                .Build();

            Assert.AreEqual("CO1", result.CountryCodes.Single());
            Assert.IsNull(result.DocumentTypes);
        }

        [TestMethod]
        public void ShouldBuildWithDocumentTypes()
        {
            RequiredSupplementaryDocument result = new RequiredSupplementaryDocumentBuilder()
                .WithDocumentTypes(new List<string> { "PASSPORT" })
                .Build();

            Assert.AreEqual("PASSPORT", result.DocumentTypes.Single());
            Assert.IsNull(result.CountryCodes);
        }
    }
}