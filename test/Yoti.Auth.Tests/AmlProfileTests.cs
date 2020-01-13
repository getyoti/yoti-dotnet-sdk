using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Aml;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class AmlObjectsTests
    {
        [TestMethod]
        public void GettersShouldRetrieveProfile()
        {
            const string givenNames = "Edward Richard George";
            const string familyName = "Heath";
            AmlAddress amlAddress = TestTools.Aml.CreateStandardAmlAddress();
            const string ssn = "AAA-GG-SSSS";

            var amlProfile = new AmlProfile(
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
        public void GettersShouldRetrieveAddress()
        {
            const string country = "UK";
            const string postcode = "AA01 0AA";

            var amlAddress = new AmlAddress(
                country: country,
                postcode: postcode);

            Assert.AreEqual(country, amlAddress.GetCountry());
            Assert.AreEqual(postcode, amlAddress.GetPostcode());
        }
    }
}