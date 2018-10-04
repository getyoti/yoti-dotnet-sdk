using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Aml;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class AmlObjectsTests
    {
        [TestMethod]
        public void AmlProfile_Getters()
        {
            string givenNames = "Edward Richard George";
            string familyName = "Heath";
            AmlAddress amlAddress = TestTools.Aml.CreateStandardAmlAddress();
            string ssn = "AAA-GG-SSSS";

            AmlProfile amlProfile = new AmlProfile(
                              givenNames: givenNames,
                              familyName: familyName,
                              amlAddress: amlAddress,
                              ssn: ssn);

            Assert.AreEqual(givenNames, amlProfile.GetGivenNames());
            Assert.AreEqual(familyName, amlProfile.GetFamilyName());
            Assert.AreEqual(amlAddress, amlProfile.GetAmlAddress());
            Assert.AreEqual(ssn, amlProfile.GetSsn());
        }

        [TestMethod]
        public void AmlAddress_Getters()
        {
            string country = "UK";
            string postcode = "AA01 0AA";

            AmlAddress amlAddress = new AmlAddress(
                country: country,
                postcode: postcode);

            Assert.AreEqual(country, amlAddress.GetCountry());
            Assert.AreEqual(postcode, amlAddress.GetPostcode());
        }
    }
}