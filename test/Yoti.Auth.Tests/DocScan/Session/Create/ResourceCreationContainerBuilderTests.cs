using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Create;

namespace Yoti.Auth.Tests.DocScan.Session.Create
{
    [TestClass]
    public class ResourceCreationContainerBuilderTests
    {
        [TestMethod]
        public void ShouldBuildWithApplicantProfile()
        {
            var applicantProfile = new ApplicantProfileBuilder()
                .WithFullName("John Doe")
                .WithDateOfBirth("1988-11-02")
                .Build();

            ResourceCreationContainer container =
                new ResourceCreationContainerBuilder()
                .WithApplicantProfile(applicantProfile)
                .Build();

            Assert.AreEqual(applicantProfile, container.ApplicantProfile);
            Assert.AreEqual("John Doe", container.ApplicantProfile.FullName);
            Assert.AreEqual("1988-11-02", container.ApplicantProfile.DateOfBirth);
        }

        [TestMethod]
        public void ShouldCorrectlySerializeApplicantProfile()
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

            var applicantProfile = new ApplicantProfileBuilder()
                .WithFullName("John Doe")
                .WithDateOfBirth("1988-11-02")
                .WithNamePrefix("Mr")
                .WithStructuredPostalAddress(address)
                .Build();

            ResourceCreationContainer container =
                new ResourceCreationContainerBuilder()
                .WithApplicantProfile(applicantProfile)
                .Build();

            string json = JsonConvert.SerializeObject(container);

            Assert.IsTrue(json.Contains("\"applicant_profile\""));
            Assert.IsTrue(json.Contains("\"full_name\":\"John Doe\""));
            Assert.IsTrue(json.Contains("\"date_of_birth\":\"1988-11-02\""));
            Assert.IsTrue(json.Contains("\"name_prefix\":\"Mr\""));
            Assert.IsTrue(json.Contains("\"structured_postal_address\""));
            Assert.IsTrue(json.Contains("\"building_number\":\"74\""));
            Assert.IsTrue(json.Contains("\"country_iso\":\"GBR\""));
        }

        [TestMethod]
        public void ShouldBuildWithNullApplicantProfile()
        {
            ResourceCreationContainer container =
                new ResourceCreationContainerBuilder()
                .Build();

            Assert.IsNull(container.ApplicantProfile);
        }
    }
}
