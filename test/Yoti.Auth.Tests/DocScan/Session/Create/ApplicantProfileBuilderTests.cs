using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yoti.Auth.DocScan.Session.Create;

namespace Yoti.Auth.Tests.DocScan.Session.Create
{
    [TestClass]
    public class ApplicantProfileBuilderTests
    {
        [TestMethod]
        public void ShouldBuildWithFullName()
        {
            var profile = new ApplicantProfileBuilder()
                .WithFullName("John Doe")
                .Build();

            Assert.AreEqual("John Doe", profile.FullName);
        }

        [TestMethod]
        public void ShouldBuildWithDateOfBirth()
        {
            var profile = new ApplicantProfileBuilder()
                .WithDateOfBirth("1988-11-02")
                .Build();

            Assert.AreEqual("1988-11-02", profile.DateOfBirth);
        }

        [TestMethod]
        public void ShouldBuildWithNamePrefix()
        {
            var profile = new ApplicantProfileBuilder()
                .WithNamePrefix("Mr")
                .Build();

            Assert.AreEqual("Mr", profile.NamePrefix);
        }

        [TestMethod]
        public void ShouldBuildWithStructuredPostalAddress()
        {
            var address = new StructuredPostalAddressBuilder()
                .WithBuildingNumber("74")
                .WithPostalCode("E143RN")
                .Build();

            var profile = new ApplicantProfileBuilder()
                .WithStructuredPostalAddress(address)
                .Build();

            Assert.AreEqual("74", profile.StructuredPostalAddress.BuildingNumber);
            Assert.AreEqual("E143RN", profile.StructuredPostalAddress.PostalCode);
        }

        [TestMethod]
        public void ShouldCorrectlySerializeWithAllProperties()
        {
            var address = new StructuredPostalAddressBuilder()
                .WithAddressFormat(1)
                .WithBuildingNumber("74")
                .WithAddressLine1("AddressLine1")
                .WithTownCity("CityName")
                .WithPostalCode("E143RN")
                .WithCountryIso("GBR")
                .WithCountry("United Kingdom")
                .Build();

            var profile = new ApplicantProfileBuilder()
                .WithFullName("John Doe")
                .WithDateOfBirth("1988-11-02")
                .WithNamePrefix("Mr")
                .WithStructuredPostalAddress(address)
                .Build();

            string json = JsonConvert.SerializeObject(profile);
            var jObject = JObject.Parse(json);

            Assert.AreEqual("John Doe", jObject["full_name"].ToString());
            Assert.AreEqual("1988-11-02", jObject["date_of_birth"].ToString());
            Assert.AreEqual("Mr", jObject["name_prefix"].ToString());
            Assert.IsNotNull(jObject["structured_postal_address"]);
            Assert.AreEqual("74", jObject["structured_postal_address"]["building_number"].ToString());
            Assert.AreEqual("GBR", jObject["structured_postal_address"]["country_iso"].ToString());
        }
    }
}
