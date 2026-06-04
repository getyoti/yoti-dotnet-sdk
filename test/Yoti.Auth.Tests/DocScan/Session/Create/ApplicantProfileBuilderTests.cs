using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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

            Assert.IsTrue(json.Contains("\"full_name\":\"John Doe\""));
            Assert.IsTrue(json.Contains("\"date_of_birth\":\"1988-11-02\""));
            Assert.IsTrue(json.Contains("\"name_prefix\":\"Mr\""));
            Assert.IsTrue(json.Contains("\"structured_postal_address\""));
            Assert.IsTrue(json.Contains("\"building_number\":\"74\""));
            Assert.IsTrue(json.Contains("\"country_iso\":\"GBR\""));
        }
    }
}
