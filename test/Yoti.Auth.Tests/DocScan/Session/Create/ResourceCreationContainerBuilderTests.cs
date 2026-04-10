using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            var jObject = JObject.Parse(json);

            Assert.IsNotNull(jObject["applicant_profile"]);
            Assert.AreEqual("John Doe", jObject["applicant_profile"]["full_name"].ToString());
            Assert.AreEqual("1988-11-02", jObject["applicant_profile"]["date_of_birth"].ToString());
            Assert.AreEqual("Mr", jObject["applicant_profile"]["name_prefix"].ToString());
            Assert.IsNotNull(jObject["applicant_profile"]["structured_postal_address"]);
            Assert.AreEqual("74", jObject["applicant_profile"]["structured_postal_address"]["building_number"].ToString());
            Assert.AreEqual("GBR", jObject["applicant_profile"]["structured_postal_address"]["country_iso"].ToString());
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
