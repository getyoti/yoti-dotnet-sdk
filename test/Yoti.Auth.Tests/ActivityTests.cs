using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Yoti.Auth.Tests.TestData;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class ActivityTests
    {
        private YotiProfile _yotiProfile;
        private Activity _activity;
        private JToken _dictionaryObject = null;
        private AttrpubapiV1.AttributeList _attributeList;

        private const string AddressFormatJson = "address_format";
        private const string BuildingNumberJson = "building_number";
        private const string AddressLineOneJson = "address_line1";
        private const string TownCityJson = "town_city";
        private const string PostalCodeJson = "postal_code";
        private const string CountryIsoJson = "country_iso";
        private const string CountryJson = "country";
        private const string FormattedAddressJson = "formatted_address";
        private const string StateJson = "state";
        private const string CareOfJson = "care_of";
        private const string BuildingJson = "building";
        private const string StreetJson = "street";
        private const string SubdistrictJson = "subdistrict";
        private const string DistrictJson = "district";
        private const string PostOfficeJson = "post_office";

        private const string YotiAdminVerifierType = "YOTI_ADMIN";
        private const string PassportSourceType = "PASSPORT";
        private const string DrivingLicenseSourceType = "DRIVING_LICENCE";

        private const string Value = "Value";

        private static readonly DateTime DateOfBirthValue = new DateTime(1980, 1, 13);
        private const string DateOfBirthString = "1980-01-13";

        private readonly ByteString _byteValue = ByteString.CopyFromUtf8(Value);

        [TestInitialize]
        public void Startup()
        {
            _yotiProfile = new YotiProfile();
            _activity = new Activity(_yotiProfile);
            _attributeList = new AttrpubapiV1.AttributeList();
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_Selfie_Jpeg()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.SelfieAttribute,
                ContentType = AttrpubapiV1.ContentType.Jpeg,
                Value = _byteValue
            };

            AddAttributeToProfile<Image>(attribute);

            Assert.AreEqual(AttrpubapiV1.ContentType.Jpeg, _yotiProfile.Selfie.Type());
            Assert.IsNotNull(_yotiProfile.Selfie.GetValue().Base64URI);
            Assert.IsNotNull(_yotiProfile.Selfie.GetValue());
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_Selfie_Png()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.SelfieAttribute,
                ContentType = AttrpubapiV1.ContentType.Png,
                Value = _byteValue
            };

            AddAttributeToProfile<Image>(attribute);

            Assert.AreEqual(AttrpubapiV1.ContentType.Png, _yotiProfile.Selfie.Type());
            Assert.IsNotNull(_yotiProfile.Selfie.GetValue().Base64URI);
            Assert.IsNotNull(_yotiProfile.Selfie.GetValue());
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_FullName()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.FullNameAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(_yotiProfile.FullName.GetValue(), Value);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_GivenNames()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.GivenNamesAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(_yotiProfile.GivenNames.GetValue(), Value);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_FamilyName()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.FamilyNameAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(_yotiProfile.FamilyName.GetValue(), Value);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_MobileNumber()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.PhoneNumberAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(_yotiProfile.MobileNumber.GetValue(), Value);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_EmailAddress()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.EmailAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(_yotiProfile.EmailAddress.GetValue(), Value);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_DateOfBirth()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.DateOfBirthAttribute,
                ContentType = AttrpubapiV1.ContentType.Date,
                Value = ByteString.CopyFromUtf8(DateOfBirthString)
            };

            AddAttributeToProfile<DateTime>(attribute);

            Assert.IsInstanceOfType(_yotiProfile.DateOfBirth.GetValue(), typeof(DateTime));
            Assert.AreEqual(AttrpubapiV1.ContentType.Date, _yotiProfile.DateOfBirth.Type());

            Assert.AreEqual(_yotiProfile.DateOfBirth.GetValue(), DateOfBirthValue);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_Address()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.PostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(AttrpubapiV1.ContentType.String, _yotiProfile.Address.Type());
            Assert.AreEqual(_yotiProfile.Address.GetValue(), Value);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_StructuredPostalAddress_UK()
        {
            const string addressFormat = "1";
            const string buildingNumber = "15a";
            const string addressLineOne = "15a North Street";
            const string townCity = "CARSHALTON";
            const string postalCode = "SM5 2HW";
            const string countryIso = "GBR";
            const string country = "UK";
            const string formattedAddress = "15a North Street\nCARSHALTON\nSM5 2HW\nUK";

            const string addressString =
                "{     \"" + AddressFormatJson + "\": " + addressFormat
                + ",     \"" + BuildingNumberJson + "\": \"" + buildingNumber
                + "\",     \"" + AddressLineOneJson + "\": \"" + addressLineOne
                + "\",     \"" + TownCityJson + "\": \"" + townCity
                + "\",     \"" + PostalCodeJson + "\": \"" + postalCode
                + "\",     \"" + CountryIsoJson + "\": \"" + countryIso
                + "\",     \"" + CountryJson + "\": \"" + country
                + "\",     \"" + FormattedAddressJson + "\": \"" + formattedAddress + "\" }";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.StructuredPostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile<IEnumerable<Dictionary<string, JToken>>>(attribute);

            Dictionary<string, JToken> structuredPostalAddress = _yotiProfile.StructuredPostalAddress.GetJsonValue();
            AssertDictionaryValue(addressFormat, AddressFormatJson, structuredPostalAddress);
            AssertDictionaryValue(buildingNumber, BuildingNumberJson, structuredPostalAddress);
            AssertDictionaryValue(addressLineOne, AddressLineOneJson, structuredPostalAddress);
            AssertDictionaryValue(townCity, TownCityJson, structuredPostalAddress);
            AssertDictionaryValue(postalCode, PostalCodeJson, structuredPostalAddress);
            AssertDictionaryValue(countryIso, CountryIsoJson, structuredPostalAddress);
            AssertDictionaryValue(country, CountryJson, structuredPostalAddress);
            AssertDictionaryValue(formattedAddress, FormattedAddressJson, structuredPostalAddress);

            Assert.AreEqual(AttrpubapiV1.ContentType.Json, _yotiProfile.StructuredPostalAddress.Type());
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_StructuredPostalAddress_India()
        {
            const string rajguraNagar = "Rajguru Nagar";

            const string addressFormat = "2";
            const string careOf = "S/O: Name";
            const string building = "House No.1111-A";
            const string street = rajguraNagar;
            const string townCity = rajguraNagar;
            const string subdistrict = "Ludhiana";
            const string district = "Ludhiana";
            const string state = "Punjab";
            const string postalCode = "141012";
            const string postOffice = rajguraNagar;
            const string countryIso = "IND";
            const string country = "India";
            const string formattedAddress = "House No.1111-A\nRajgura Nagar\nLudhina\nPunjab\n141012\nIndia";

            const string addressString =
                "{     \"" + AddressFormatJson + "\": " + addressFormat
                + ",    \"" + CareOfJson + "\": \"" + careOf
                + "\",     \"" + BuildingJson + "\": \"" + building
                + "\",     \"" + StreetJson + "\": \"" + street
                + "\",     \"" + TownCityJson + "\": \"" + townCity
                + "\",     \"" + SubdistrictJson + "\": \"" + subdistrict
                + "\",     \"" + DistrictJson + "\":\"" + district
                + "\",     \"" + StateJson + "\": \"" + state
                + "\",     \"" + PostalCodeJson + "\": \"" + postalCode
                + "\",     \"" + PostOfficeJson + "\": \"" + postOffice
                + "\",     \"" + CountryIsoJson + "\": \"" + countryIso
                + "\",     \"" + CountryJson + "\": \"" + country
                + "\",     \"" + FormattedAddressJson + "\": \"" + formattedAddress + "\" }";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.StructuredPostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile<IEnumerable<Dictionary<string, JToken>>>(attribute);

            Dictionary<string, JToken> structuredPostalAddress = _yotiProfile.StructuredPostalAddress.GetJsonValue();
            AssertDictionaryValue(addressFormat, AddressFormatJson, structuredPostalAddress);
            AssertDictionaryValue(careOf, CareOfJson, structuredPostalAddress);
            AssertDictionaryValue(building, BuildingJson, structuredPostalAddress);
            AssertDictionaryValue(street, StreetJson, structuredPostalAddress);
            AssertDictionaryValue(townCity, TownCityJson, structuredPostalAddress);
            AssertDictionaryValue(subdistrict, SubdistrictJson, structuredPostalAddress);
            AssertDictionaryValue(district, DistrictJson, structuredPostalAddress);
            AssertDictionaryValue(state, StateJson, structuredPostalAddress);
            AssertDictionaryValue(postalCode, PostalCodeJson, structuredPostalAddress);
            AssertDictionaryValue(postOffice, PostOfficeJson, structuredPostalAddress);
            AssertDictionaryValue(countryIso, CountryIsoJson, structuredPostalAddress);
            AssertDictionaryValue(country, CountryJson, structuredPostalAddress);
            AssertDictionaryValue(formattedAddress, FormattedAddressJson, structuredPostalAddress);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_StructuredPostalAddress_USA()
        {
            const string addressFormat = "3";
            const string addressLineOne = "1512 Ferry Street";
            const string townCity = "Anniston";
            const string state = "AL";
            const string postalCode = "36201";
            const string countryIso = "USA";
            const string country = "USA";
            const string formattedAddress = "1512 Ferry Street\nAnniston AL 36201\nUSA";

            const string addressString =
                    "{     \"" + AddressFormatJson + "\": " + addressFormat
                + ",     \"" + AddressLineOneJson + "\": \"" + addressLineOne
                + "\",     \"" + TownCityJson + "\": \"" + townCity
                + "\",     \"" + StateJson + "\": \"" + state
                + "\",     \"" + PostalCodeJson + "\": \"" + postalCode
                + "\",     \"" + CountryIsoJson + "\": \"" + countryIso
                + "\",     \"" + CountryJson + "\": \"" + country
                + "\",     \"" + FormattedAddressJson + "\": \"" + formattedAddress + "\" }";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.StructuredPostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile<IEnumerable<Dictionary<string, JToken>>>(attribute);

            Dictionary<string, JToken> structuredPostalAddress = _yotiProfile.StructuredPostalAddress.GetJsonValue();
            AssertDictionaryValue(addressFormat, AddressFormatJson, structuredPostalAddress);
            AssertDictionaryValue(addressLineOne, AddressLineOneJson, structuredPostalAddress);
            AssertDictionaryValue(townCity, TownCityJson, structuredPostalAddress);
            AssertDictionaryValue(state, StateJson, structuredPostalAddress);
            AssertDictionaryValue(postalCode, PostalCodeJson, structuredPostalAddress);
            AssertDictionaryValue(countryIso, CountryIsoJson, structuredPostalAddress);
            AssertDictionaryValue(country, CountryJson, structuredPostalAddress);
            AssertDictionaryValue(formattedAddress, FormattedAddressJson, structuredPostalAddress);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_StructuredPostalAddress_NestedJSON()
        {
            object nestedValueObject;
            using (StreamReader r = File.OpenText("TestData/NestedJSON.json"))
            {
                string json = r.ReadToEnd();
                nestedValueObject = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            }

            const string addressFormat = "1";
            const string buildingNumber = "15a";
            const string addressLineOne = "15a North Street";
            const string townCity = "CARSHALTON";
            const string postalCode = "SM5 2HW";
            const string countryIso = "GBR";
            const string country = "UK";
            const string formattedAddress = "15a North Street\nCARSHALTON\nSM5 2HW\nUK";

            const string nestedValueJson = "nested_value";
            const string oneJson = "1";
            const string oneTwoJson = "1-2";
            const string oneTwoFiveJson = "1-2-5";
            const string oneTwoFiveOneJson = "1-2-5-1";
            const string oneTwoFiveOneJsonValue = "OneTwoFiveOne";

            string addressString =
                "{     \"" + AddressFormatJson + "\": " + addressFormat
                + ",     \"" + BuildingNumberJson + "\": \"" + buildingNumber
                + "\",     \"" + AddressLineOneJson + "\": \"" + addressLineOne
                + "\",     \"" + TownCityJson + "\": \"" + townCity
                + "\",     \"" + PostalCodeJson + "\": \"" + postalCode
                + "\",     \"" + CountryIsoJson + "\": \"" + countryIso
                + "\",     \"" + CountryJson + "\": \"" + country
                + "\",     \"" + nestedValueJson + "\": " + nestedValueObject
                + ",     \"" + FormattedAddressJson + "\": \"" + formattedAddress + "\" }";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.StructuredPostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile<IEnumerable<Dictionary<string, JToken>>>(attribute);

            Dictionary<string, JToken> structuredPostalAddress = _yotiProfile.StructuredPostalAddress.GetJsonValue();
            AssertDictionaryValue(addressFormat, AddressFormatJson, structuredPostalAddress);
            AssertDictionaryValue(buildingNumber, BuildingNumberJson, structuredPostalAddress);
            AssertDictionaryValue(addressLineOne, AddressLineOneJson, structuredPostalAddress);
            AssertDictionaryValue(townCity, TownCityJson, structuredPostalAddress);
            AssertDictionaryValue(postalCode, PostalCodeJson, structuredPostalAddress);
            AssertDictionaryValue(countryIso, CountryIsoJson, structuredPostalAddress);
            AssertDictionaryValue(country, CountryJson, structuredPostalAddress);

            JToken nestedJsonObj = structuredPostalAddress[nestedValueJson];
            JToken token1 = nestedJsonObj[oneJson];
            JToken token1_2 = token1[oneTwoJson];
            JToken token1_2_5 = token1_2[oneTwoFiveJson];
            JToken token1_2_5_1 = token1_2_5[oneTwoFiveOneJson];

            Assert.AreEqual(oneTwoFiveOneJsonValue, token1_2_5_1);

            AssertDictionaryValue(nestedValueObject.ToString(), nestedValueJson, structuredPostalAddress);
            AssertDictionaryValue(formattedAddress, FormattedAddressJson, structuredPostalAddress);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_AddressIsTakenFromFormattedAddressIfNull()
        {
            const string addressFormat = "1";
            const string buildingNumber = "15a";
            const string addressLineOne = "15a North Street";
            const string townCity = "CARSHALTON";
            const string postalCode = "SM5 2HW";
            const string countryIso = "GBR";
            const string country = "UK";
            const string formattedAddress = "15a North Street\nCARSHALTON\nSM5 2HW\nUK";

            const string addressString =
                "{     \"" + AddressFormatJson + "\": " + addressFormat
                + ",     \"" + BuildingNumberJson + "\": \"" + buildingNumber
                + "\",     \"" + AddressLineOneJson + "\": \"" + addressLineOne
                + "\",     \"" + TownCityJson + "\": \"" + townCity
                + "\",     \"" + PostalCodeJson + "\": \"" + postalCode
                + "\",     \"" + CountryIsoJson + "\": \"" + countryIso
                + "\",     \"" + CountryJson + "\": \"" + country
                + "\",     \"" + FormattedAddressJson + "\": \"" + formattedAddress + "\" }";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.StructuredPostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile<IEnumerable<Dictionary<string, JToken>>>(attribute);

            _activity.SetAddressToBeFormattedAddressIfNull();

            Assert.AreEqual(_yotiProfile.Address.GetValue(), formattedAddress);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_AddressIsNotTakenFromFormattedAddressIfAddressIsPresent()
        {
            const string addressFormat = "1";
            const string buildingNumber = "15a";
            const string addressLineOne = "15a North Street";
            const string townCity = "CARSHALTON";
            const string postalCode = "SM5 2HW";
            const string countryIso = "GBR";
            const string country = "UK";
            const string formattedAddress = "15a North Street\nCARSHALTON\nSM5 2HW\nUK";
            const string postalAddress = "33a South Street\nCARSHALTON\nSM5 2HW\nUK";

            const string structuredAddressString =
                "{     \"" + AddressFormatJson + "\": " + addressFormat
                + ",     \"" + BuildingNumberJson + "\": \"" + buildingNumber
                + "\",     \"" + AddressLineOneJson + "\": \"" + addressLineOne
                + "\",     \"" + TownCityJson + "\": \"" + townCity
                + "\",     \"" + PostalCodeJson + "\": \"" + postalCode
                + "\",     \"" + CountryIsoJson + "\": \"" + countryIso
                + "\",     \"" + CountryJson + "\": \"" + country
                + "\",     \"" + FormattedAddressJson + "\": \"" + formattedAddress + "\" }";

            var structuredAddressAttribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.StructuredPostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.Json,
                Value = ByteString.CopyFromUtf8(structuredAddressString)
            };

            var addressAttribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.PostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8(postalAddress)
            };

            AddAttributeToProfile<IEnumerable<Dictionary<string, JToken>>>(structuredAddressAttribute);
            AddAttributeToProfile<string>(addressAttribute);
            _activity.SetAddressToBeFormattedAddressIfNull();

            Assert.AreNotEqual(_yotiProfile.Address.GetValue(), formattedAddress);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_Gender()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.GenderAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(_yotiProfile.Gender.GetValue(), Value);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_Nationality()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = Constants.UserProfile.NationalityAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(_yotiProfile.Nationality.GetValue(), Value);
        }

        [TestMethod]
        public void Activity_GetSources_IncludesDrivingLicense_String()
        {
            AttrpubapiV1.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                Constants.UserProfile.GivenNamesAttribute,
                Value,
                AttrpubapiV1.ContentType.String,
                TestAnchors.DrivingLicenseAnchor);

            AddAttributeToProfile<string>(attribute);

            IEnumerable<Anchors.Anchor> sources = _yotiProfile.GivenNames.GetSources();

            Assert.IsTrue(
                sources.Any(
                    s => s.GetValue().Contains(DrivingLicenseSourceType)));
        }

        [TestMethod]
        public void Activity_GetSources_IncludesDrivingLicense_AgeVerified()
        {
            AttrpubapiV1.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                Constants.UserProfile.EmailAddressAttribute,
                "true",
                AttrpubapiV1.ContentType.String,
                TestAnchors.DrivingLicenseAnchor);

            AddAttributeToProfile<string>(attribute);

            IEnumerable<Anchors.Anchor> sources = _yotiProfile.EmailAddress.GetSources();
            Assert.IsTrue(
                sources.Any(
                    s => s.GetValue().Contains(DrivingLicenseSourceType)));
        }

        [TestMethod]
        public void Activity_GetSources_IncludesDrivingLicense_StructuredPostalAddress()
        {
            AttrpubapiV1.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                Constants.UserProfile.StructuredPostalAddressAttribute,
                "{ \"properties\": { \"name\": { \"type\": \"string\"     } } }",
                AttrpubapiV1.ContentType.Json,
                TestAnchors.DrivingLicenseAnchor);

            AddAttributeToProfile<IEnumerable<Dictionary<string, JToken>>>(attribute);

            IEnumerable<Anchors.Anchor> sources = _yotiProfile.StructuredPostalAddress.GetSources();
            Assert.IsTrue(
                sources.Any(
                    s => s.GetValue().Contains(DrivingLicenseSourceType)));
        }

        [TestMethod]
        public void Activity_GetSources_IncludesPassport()
        {
            AttrpubapiV1.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                Constants.UserProfile.DateOfBirthAttribute,
                DateOfBirthString,
                AttrpubapiV1.ContentType.Date,
                TestAnchors.PassportAnchor);

            AddAttributeToProfile<DateTime>(attribute);

            IEnumerable<Anchors.Anchor> sources = _yotiProfile.DateOfBirth.GetSources();
            Assert.IsTrue(
                sources.Any(
                    s => s.GetValue().Contains(PassportSourceType)));
        }

        [TestMethod]
        public void Activity_GetVerifiers_IncludesYotiAdmin()
        {
            AttrpubapiV1.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                Constants.UserProfile.SelfieAttribute,
                Value,
                AttrpubapiV1.ContentType.Jpeg,
                TestAnchors.YotiAdminAnchor);

            AddAttributeToProfile<Image>(attribute);

            IEnumerable<Anchors.Anchor> verifiers = _yotiProfile.Selfie.GetVerifiers();
            Assert.IsTrue(
                verifiers.Any(
                    s => s.GetValue().Contains(YotiAdminVerifierType)));
        }

        private void AssertDictionaryValue(string expectedValue, string dictionaryKey, Dictionary<string, JToken> dictionary)
        {
            dictionary.TryGetValue(dictionaryKey, out _dictionaryObject);
            Assert.AreEqual(expectedValue, _dictionaryObject.ToString());
        }

        private void AddAttributeToProfile<T>(AttrpubapiV1.Attribute attribute)
        {
            BaseAttribute parsedAttribute = AttributeConverter.ConvertToBaseAttribute(attribute);
            _yotiProfile.Add((YotiAttribute<T>)parsedAttribute);
        }
    }
}