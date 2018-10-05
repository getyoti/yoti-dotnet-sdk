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
        private YotiUserProfile _yotiUserProfile;
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

        private const string GivenNamesAttribute = "given_names";
        private const string FamilyNameAttribute = "family_name";
        private const string FullNameAttribute = "full_name";
        private const string GenderAttribute = "gender";
        private const string SelfieAttribute = "selfie";
        private const string PhoneNumberAttribute = "phone_number";
        private const string EmailAddressAttribute = "email_address";
        private const string NationalityAttribute = "nationality";
        private const string AgeOver18Attribute = "age_over:18";
        private const string AgeUnder18Attribute = "age_under:18";
        private const string PostalAddressAttribute = "postal_address";
        private const string StructuredPostalAddressAttribute = "structured_postal_address";
        private const string DateOfBirthAttribute = "date_of_birth";

        private static readonly DateTime DateOfBirthValue = new DateTime(1980, 1, 13);
        private const string DateOfBirthString = "1980-01-13";

        [TestInitialize]
        public void Startup()
        {
            _yotiProfile = new YotiProfile();
            _yotiUserProfile = new YotiUserProfile();
            _activity = new Activity(_yotiProfile, _yotiUserProfile);
            _attributeList = new AttrpubapiV1.AttributeList();
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_Selfie_Jpeg()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = SelfieAttribute,
                ContentType = AttrpubapiV1.ContentType.Jpeg,
                Value = ByteString.CopyFromUtf8("selfieJpegValue")
            };

            AddAttributeToProfile(attribute);

            Assert.IsNotNull(_yotiProfile.Selfie.GetBase64URI());
            Assert.IsNotNull(_yotiProfile.Selfie.GetImage());

            Assert.IsNotNull(_yotiUserProfile.Selfie.Base64URI);
            Assert.IsNotNull(_yotiUserProfile.Selfie.Data);
            Assert.IsNotNull(_yotiUserProfile.Selfie.Type);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_Selfie_Png()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = SelfieAttribute,
                ContentType = AttrpubapiV1.ContentType.Png,
                Value = ByteString.CopyFromUtf8("selfiePngValue")
            };

            AddAttributeToProfile(attribute);

            Assert.IsNotNull(_yotiProfile.Selfie.GetBase64URI());
            Assert.IsNotNull(_yotiProfile.Selfie.GetImage());

            Assert.IsNotNull(_yotiUserProfile.Selfie.Base64URI);
            Assert.IsNotNull(_yotiUserProfile.Selfie.Data);
            Assert.IsNotNull(_yotiUserProfile.Selfie.Type);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_FullName()
        {
            const string value = "fullNameValue";
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = FullNameAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8(value)
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.FullName.GetValue(), value);

            Assert.AreEqual(_yotiUserProfile.FullName, value);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_GivenNames()
        {
            const string value = "GivenNamesValue";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = GivenNamesAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8(value)
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.GivenNames.GetValue(), value);

            Assert.AreEqual(_yotiUserProfile.GivenNames, value);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_FamilyName()
        {
            const string value = "FamilyNameValue";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = FamilyNameAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8(value)
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.FamilyName.GetValue(), value);

            Assert.AreEqual(_yotiUserProfile.FamilyName, value);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_MobileNumber()
        {
            const string value = "0123456789";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = PhoneNumberAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8(value)
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.MobileNumber.GetValue(), value);

            Assert.AreEqual(_yotiUserProfile.MobileNumber, value);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_EmailAddress()
        {
            const string value = "EmailAddressValue";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = EmailAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8(value)
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.EmailAddress.GetValue(), value);

            Assert.AreEqual(_yotiUserProfile.EmailAddress, value);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_DateOfBirth()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = DateOfBirthAttribute,
                ContentType = AttrpubapiV1.ContentType.Date,
                Value = ByteString.CopyFromUtf8(DateOfBirthString)
            };

            AddAttributeToProfile(attribute);

            Assert.IsInstanceOfType(_yotiProfile.DateOfBirth.GetValue(), typeof(DateTime?));

            Assert.AreEqual(_yotiProfile.DateOfBirth.GetValue(), DateOfBirthValue);

            Assert.AreEqual(_yotiUserProfile.DateOfBirth, DateOfBirthValue);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_DateOfBirth_IncorrectFormatNotAdded()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = DateOfBirthAttribute,
                ContentType = AttrpubapiV1.ContentType.Date,
                Value = ByteString.CopyFromUtf8("1980/01/13")
            };

            AddAttributeToProfile(attribute);

            Assert.IsNull(_yotiProfile.DateOfBirth);

            Assert.IsNull(_yotiUserProfile.DateOfBirth);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_AgeVerified_Over_True()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = AgeOver18Attribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8("true")
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.AgeVerified.GetValue(), true);

            Assert.AreEqual(_yotiUserProfile.IsAgeVerified, true);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_AgeVerified_Over_False()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = AgeOver18Attribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8("false")
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.AgeVerified.GetValue(), false);

            Assert.AreEqual(_yotiUserProfile.IsAgeVerified, false);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_AgeVerified_Under_True()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = AgeUnder18Attribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8("true")
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.AgeVerified.GetValue(), true);

            Assert.AreEqual(_yotiUserProfile.IsAgeVerified, true);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_AgeVerified_Under_False()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = AgeUnder18Attribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8("false")
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.AgeVerified.GetValue(), false);

            Assert.AreEqual(_yotiUserProfile.IsAgeVerified, false);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_Address()
        {
            const string value = "AddressValue";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = PostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8(value)
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.Address.GetValue(), value);

            Assert.AreEqual(_yotiUserProfile.Address, value);
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
                Name = StructuredPostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile(attribute);

            Dictionary<string, JToken> structuredPostalAddress = _yotiProfile.StructuredPostalAddress.GetJsonValue();
            AssertDictionaryValue(addressFormat, AddressFormatJson, structuredPostalAddress);
            AssertDictionaryValue(buildingNumber, BuildingNumberJson, structuredPostalAddress);
            AssertDictionaryValue(addressLineOne, AddressLineOneJson, structuredPostalAddress);
            AssertDictionaryValue(townCity, TownCityJson, structuredPostalAddress);
            AssertDictionaryValue(postalCode, PostalCodeJson, structuredPostalAddress);
            AssertDictionaryValue(countryIso, CountryIsoJson, structuredPostalAddress);
            AssertDictionaryValue(country, CountryJson, structuredPostalAddress);
            AssertDictionaryValue(formattedAddress, FormattedAddressJson, structuredPostalAddress);

            Dictionary<string, JToken> legacyStructuredPostalAddress = _yotiUserProfile.StructuredPostalAddress;
            AssertDictionaryValue(addressFormat, AddressFormatJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(buildingNumber, BuildingNumberJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(addressLineOne, AddressLineOneJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(townCity, TownCityJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(postalCode, PostalCodeJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(countryIso, CountryIsoJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(country, CountryJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(formattedAddress, FormattedAddressJson, legacyStructuredPostalAddress);
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
                Name = StructuredPostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile(attribute);

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

            Dictionary<string, JToken> legacyStructuredPostalAddress = _yotiUserProfile.StructuredPostalAddress;
            AssertDictionaryValue(addressFormat, AddressFormatJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(careOf, CareOfJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(building, BuildingJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(street, StreetJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(townCity, TownCityJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(subdistrict, SubdistrictJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(district, DistrictJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(state, StateJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(postalCode, PostalCodeJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(postOffice, PostOfficeJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(countryIso, CountryIsoJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(country, CountryJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(formattedAddress, FormattedAddressJson, legacyStructuredPostalAddress);
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
                Name = StructuredPostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile(attribute);

            Dictionary<string, JToken> structuredPostalAddress = _yotiProfile.StructuredPostalAddress.GetJsonValue();
            AssertDictionaryValue(addressFormat, AddressFormatJson, structuredPostalAddress);
            AssertDictionaryValue(addressLineOne, AddressLineOneJson, structuredPostalAddress);
            AssertDictionaryValue(townCity, TownCityJson, structuredPostalAddress);
            AssertDictionaryValue(state, StateJson, structuredPostalAddress);
            AssertDictionaryValue(postalCode, PostalCodeJson, structuredPostalAddress);
            AssertDictionaryValue(countryIso, CountryIsoJson, structuredPostalAddress);
            AssertDictionaryValue(country, CountryJson, structuredPostalAddress);
            AssertDictionaryValue(formattedAddress, FormattedAddressJson, structuredPostalAddress);

            Dictionary<string, JToken> legacyStructuredPostalAddress = _yotiUserProfile.StructuredPostalAddress;
            AssertDictionaryValue(addressFormat, AddressFormatJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(addressLineOne, AddressLineOneJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(townCity, TownCityJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(state, StateJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(postalCode, PostalCodeJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(countryIso, CountryIsoJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(country, CountryJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(formattedAddress, FormattedAddressJson, legacyStructuredPostalAddress);
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
                Name = StructuredPostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile(attribute);

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

            Dictionary<string, JToken> legacyStructuredPostalAddress = _yotiUserProfile.StructuredPostalAddress;
            AssertDictionaryValue(addressFormat, AddressFormatJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(buildingNumber, BuildingNumberJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(addressLineOne, AddressLineOneJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(townCity, TownCityJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(postalCode, PostalCodeJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(countryIso, CountryIsoJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(country, CountryJson, legacyStructuredPostalAddress);

            JToken legacyNestedJsonObj = legacyStructuredPostalAddress[nestedValueJson];
            JToken legacyToken1 = nestedJsonObj[oneJson];
            JToken legacyToken1_2 = legacyToken1[oneTwoJson];
            JToken legacyToken1_2_5 = legacyToken1_2[oneTwoFiveJson];
            JToken legacyToken1_2_5_1 = legacyToken1_2_5[oneTwoFiveOneJson];

            Assert.AreEqual(oneTwoFiveOneJsonValue, legacyToken1_2_5_1);

            AssertDictionaryValue(nestedValueObject.ToString(), nestedValueJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(formattedAddress, FormattedAddressJson, legacyStructuredPostalAddress);
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
                Name = StructuredPostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiUserProfile.Address, formattedAddress);
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
                Name = StructuredPostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.Json,
                Value = ByteString.CopyFromUtf8(structuredAddressString)
            };

            var addressAttribute = new AttrpubapiV1.Attribute
            {
                Name = PostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8(postalAddress)
            };

            AddAttributeToProfile(structuredAddressAttribute);
            AddAttributeToProfile(addressAttribute);

            Assert.AreNotEqual(_yotiUserProfile.Address, formattedAddress);
            Assert.AreNotEqual(_yotiProfile.Address.GetValue(), formattedAddress);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_Gender()
        {
            const string value = "GenderValue";
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = GenderAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8(value)
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.Gender.GetValue(), value);

            Assert.AreEqual(_yotiUserProfile.Gender, value);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_Nationality()
        {
            const string value = "NationalityValue";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = NationalityAttribute,
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8(value)
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.Nationality.GetValue(), value);

            Assert.AreEqual(_yotiUserProfile.Nationality, value);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_OtherAttributes()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "OtherAttributes",
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8("OtherAttributesValue")
            };

            AddAttributeToProfile(attribute);

            Assert.IsNotNull(_yotiProfile.OtherAttributes);

            Assert.IsNotNull(_yotiUserProfile.OtherAttributes);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_UndefinedAttribute_Isnt_Added()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "UndefinedAttribute",
                ContentType = AttrpubapiV1.ContentType.Undefined,
                Value = ByteString.CopyFromUtf8("UndefinedAttributeValue")
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.OtherAttributes.Count, 0);

            Assert.AreEqual(_yotiUserProfile.OtherAttributes.Count, 0);
        }

        [TestMethod]
        public void Activity_NonMatchingProtoBufName_Date_AddedAsOtherAttribute()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "NonMatchingDateAttribute",
                ContentType = AttrpubapiV1.ContentType.Date,
                Value = ByteString.CopyFromUtf8("NonMatchingDateAttributeValue")
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.OtherAttributes.Count, 1);
            Assert.AreEqual(_yotiUserProfile.OtherAttributes.Count, 1);
        }

        [TestMethod]
        public void Activity_NonMatchingProtoBufName_Jpeg_AddedAsOtherAttribute()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "NonMatchingJpegAttribute",
                ContentType = AttrpubapiV1.ContentType.Jpeg,
                Value = ByteString.CopyFromUtf8("NonMatchingJpegAttributeValue")
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.OtherAttributes.Count, 1);
            Assert.AreEqual(_yotiUserProfile.OtherAttributes.Count, 1);
        }

        [TestMethod]
        public void Activity_NonMatchingProtoBufName_Png_AddedAsOtherAttribute()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "NonMatchingPngAttribute",
                ContentType = AttrpubapiV1.ContentType.Png,
                Value = ByteString.CopyFromUtf8("NonMatchingPngAttributeValue")
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.OtherAttributes.Count, 1);
            Assert.AreEqual(_yotiUserProfile.OtherAttributes.Count, 1);
        }

        [TestMethod]
        public void Activity_GetSources_IncludesDrivingLicense_String()
        {
            AttrpubapiV1.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                GivenNamesAttribute,
                "givenNamesValue",
                AttrpubapiV1.ContentType.String,
                TestAnchors.DrivingLicenseAnchor);

            AddAttributeToProfile(attribute);

            IEnumerable<Anchors.Anchor> sources = _yotiProfile.GivenNames.GetSources();

            Assert.IsTrue(
                sources.Any(
                    s => s.GetValue().Contains(DrivingLicenseSourceType)));
        }

        [TestMethod]
        public void Activity_GetSources_IncludesDrivingLicense_AgeVerified()
        {
            AttrpubapiV1.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                AgeOver18Attribute,
                "true",
                AttrpubapiV1.ContentType.String,
                TestAnchors.DrivingLicenseAnchor);

            AddAttributeToProfile(attribute);

            IEnumerable<Anchors.Anchor> sources = _yotiProfile.AgeVerified.GetSources();
            Assert.IsTrue(
                sources.Any(
                    s => s.GetValue().Contains(DrivingLicenseSourceType)));
        }

        [TestMethod]
        public void Activity_GetSources_IncludesDrivingLicense_StructuredPostalAddress()
        {
            AttrpubapiV1.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                StructuredPostalAddressAttribute,
                "{ \"properties\": { \"name\": { \"type\": \"string\"     } } }",
                AttrpubapiV1.ContentType.Json,
                TestAnchors.DrivingLicenseAnchor);

            AddAttributeToProfile(attribute);

            IEnumerable<Anchors.Anchor> sources = _yotiProfile.StructuredPostalAddress.GetSources();
            Assert.IsTrue(
                sources.Any(
                    s => s.GetValue().Contains(DrivingLicenseSourceType)));
        }

        [TestMethod]
        public void Activity_GetSources_IncludesPassport()
        {
            AttrpubapiV1.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                DateOfBirthAttribute,
                DateOfBirthString,
                AttrpubapiV1.ContentType.Date,
                TestAnchors.PassportAnchor);

            AddAttributeToProfile(attribute);

            IEnumerable<Anchors.Anchor> sources = _yotiProfile.DateOfBirth.GetSources();
            Assert.IsTrue(
                sources.Any(
                    s => s.GetValue().Contains(PassportSourceType)));
        }

        [TestMethod]
        public void Activity_GetVerifiers_IncludesYotiAdmin()
        {
            AttrpubapiV1.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                SelfieAttribute,
                "selfieValue",
                AttrpubapiV1.ContentType.Jpeg,
                TestAnchors.YotiAdminAnchor);

            AddAttributeToProfile(attribute);

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

        private void AddAttributeToProfile(AttrpubapiV1.Attribute attribute)
        {
            _attributeList.Attributes.Add(attribute);

            _activity.AddAttributesToProfile(_attributeList);
        }
    }
}