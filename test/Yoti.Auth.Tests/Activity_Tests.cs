using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Yoti.Auth.Tests.TestData;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class Activity_Tests
    {
        private YotiProfile _yotiProfile;
        private YotiUserProfile _yotiUserProfile;
        private Activity _activity;
        private JToken _dictionaryObject = null;
        private AttrpubapiV1.AttributeList _attributeList;

        private static readonly string _addressFormatJson = "address_format";
        private static readonly string _buildingNumberJson = "building_number";
        private static readonly string _addressLineOneJson = "address_line1";
        private static readonly string _townCityJson = "town_city";
        private static readonly string _postalCodeJson = "postal_code";
        private static readonly string _countryIsoJson = "country_iso";
        private static readonly string _countryJson = "country";
        private static readonly string _formattedAddressJson = "formatted_address";
        private static readonly string _stateJson = "state";
        private static readonly string _careOfJson = "care_of";
        private static readonly string _buildingJson = "building";
        private static readonly string _streetJson = "street";
        private static readonly string _subdistrictJson = "subdistrict";
        private static readonly string _districtJson = "district";
        private static readonly string _postOfficeJson = "post_office";

        private static readonly string _yotiAdminVerifierType = "YOTI_ADMIN";
        private static readonly string _passportSourceType = "PASSPORT";
        private static readonly string _drivingLicenseSourceType = "DRIVING_LICENCE";

        private static readonly string _givenNamesAttribute = "given_names";
        private static readonly string _familyNameAttribute = "family_name";
        private static readonly string _fullNameAttribute = "full_name";
        private static readonly string _genderAttribute = "gender";
        private static readonly string _selfieAttribute = "selfie";
        private static readonly string _phoneNumberAttribute = "phone_number";
        private static readonly string _emailAddressAttribute = "email_address";
        private static readonly string _nationalityAttribute = "nationality";
        private static readonly string _ageOver18Attribute = "age_over:18";
        private static readonly string _ageUnder18Attribute = "age_under:18";
        private static readonly string _postalAddressAttribute = "postal_address";
        private static readonly string _structuredPostalAddressAttribute = "structured_postal_address";
        private static readonly string _dateOfBirthAttribute = "date_of_birth";

        private static readonly DateTime _DateOfBirthValue = new DateTime(1980, 1, 13);
        private static readonly string _dateOfBirthString = "1980-01-13";

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
                Name = _selfieAttribute,
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
                Name = _selfieAttribute,
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
            string value = "fullNameValue";
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = _fullNameAttribute,
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
            string value = "GivenNamesValue";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = _givenNamesAttribute,
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
            string value = "FamilyNameValue";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = _familyNameAttribute,
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
            string value = "0123456789";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = _phoneNumberAttribute,
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
            string value = "EmailAddressValue";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = _emailAddressAttribute,
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
                Name = _dateOfBirthAttribute,
                ContentType = AttrpubapiV1.ContentType.Date,
                Value = ByteString.CopyFromUtf8(_dateOfBirthString)
            };

            AddAttributeToProfile(attribute);

            Assert.IsInstanceOfType(_yotiProfile.DateOfBirth.GetValue(), typeof(DateTime?));

            Assert.AreEqual(_yotiProfile.DateOfBirth.GetValue(), _DateOfBirthValue);

            Assert.AreEqual(_yotiUserProfile.DateOfBirth, _DateOfBirthValue);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_DateOfBirth_IncorrectFormatNotAdded()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = _dateOfBirthAttribute,
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
                Name = _ageOver18Attribute,
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
                Name = _ageOver18Attribute,
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
                Name = _ageUnder18Attribute,
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
                Name = _ageUnder18Attribute,
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
            string value = "AddressValue";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = _postalAddressAttribute,
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
            string addressFormat = "1";
            string buildingNumber = "15a";
            string addressLineOne = "15a North Street";
            string townCity = "CARSHALTON";
            string postalCode = "SM5 2HW";
            string countryIso = "GBR";
            string country = "UK";
            string formattedAddress = "15a North Street\nCARSHALTON\nSM5 2HW\nUK";

            string addressString =
                "{     \"" + _addressFormatJson + "\": " + addressFormat
                + ",     \"" + _buildingNumberJson + "\": \"" + buildingNumber
                + "\",     \"" + _addressLineOneJson + "\": \"" + addressLineOne
                + "\",     \"" + _townCityJson + "\": \"" + townCity
                + "\",     \"" + _postalCodeJson + "\": \"" + postalCode
                + "\",     \"" + _countryIsoJson + "\": \"" + countryIso
                + "\",     \"" + _countryJson + "\": \"" + country
                + "\",     \"" + _formattedAddressJson + "\": \"" + formattedAddress + "\" }";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = _structuredPostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile(attribute);

            Dictionary<string, JToken> structuredPostalAddress = _yotiProfile.StructuredPostalAddress.GetJsonValue();
            AssertDictionaryValue(addressFormat, _addressFormatJson, structuredPostalAddress);
            AssertDictionaryValue(buildingNumber, _buildingNumberJson, structuredPostalAddress);
            AssertDictionaryValue(addressLineOne, _addressLineOneJson, structuredPostalAddress);
            AssertDictionaryValue(townCity, _townCityJson, structuredPostalAddress);
            AssertDictionaryValue(postalCode, _postalCodeJson, structuredPostalAddress);
            AssertDictionaryValue(countryIso, _countryIsoJson, structuredPostalAddress);
            AssertDictionaryValue(country, _countryJson, structuredPostalAddress);
            AssertDictionaryValue(formattedAddress, _formattedAddressJson, structuredPostalAddress);

            Dictionary<string, JToken> legacyStructuredPostalAddress = _yotiUserProfile.StructuredPostalAddress;
            AssertDictionaryValue(addressFormat, _addressFormatJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(buildingNumber, _buildingNumberJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(addressLineOne, _addressLineOneJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(townCity, _townCityJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(postalCode, _postalCodeJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(countryIso, _countryIsoJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(country, _countryJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(formattedAddress, _formattedAddressJson, legacyStructuredPostalAddress);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_StructuredPostalAddress_India()
        {
            string rajguraNagar = "Rajguru Nagar";

            string addressFormat = "2";
            string careOf = "S/O: Name";
            string building = "House No.1111-A";
            string street = rajguraNagar;
            string townCity = rajguraNagar;
            string subdistrict = "Ludhiana";
            string district = "Ludhiana";
            string state = "Punjab";
            string postalCode = "141012";
            string postOffice = rajguraNagar;
            string countryIso = "IND";
            string country = "India";
            string formattedAddress = "House No.1111-A\nRajgura Nagar\nLudhina\nPunjab\n141012\nIndia";

            string addressString =
                "{     \"" + _addressFormatJson + "\": " + addressFormat
                + ",    \"" + _careOfJson + "\": \"" + careOf
                + "\",     \"" + _buildingJson + "\": \"" + building
                + "\",     \"" + _streetJson + "\": \"" + street
                + "\",     \"" + _townCityJson + "\": \"" + townCity
                + "\",     \"" + _subdistrictJson + "\": \"" + subdistrict
                + "\",     \"" + _districtJson + "\":\"" + district
                + "\",     \"" + _stateJson + "\": \"" + state
                + "\",     \"" + _postalCodeJson + "\": \"" + postalCode
                + "\",     \"" + _postOfficeJson + "\": \"" + postOffice
                + "\",     \"" + _countryIsoJson + "\": \"" + countryIso
                + "\",     \"" + _countryJson + "\": \"" + country
                + "\",     \"" + _formattedAddressJson + "\": \"" + formattedAddress + "\" }";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = _structuredPostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile(attribute);

            Dictionary<string, JToken> structuredPostalAddress = _yotiProfile.StructuredPostalAddress.GetJsonValue();
            AssertDictionaryValue(addressFormat, _addressFormatJson, structuredPostalAddress);
            AssertDictionaryValue(careOf, _careOfJson, structuredPostalAddress);
            AssertDictionaryValue(building, _buildingJson, structuredPostalAddress);
            AssertDictionaryValue(street, _streetJson, structuredPostalAddress);
            AssertDictionaryValue(townCity, _townCityJson, structuredPostalAddress);
            AssertDictionaryValue(subdistrict, _subdistrictJson, structuredPostalAddress);
            AssertDictionaryValue(district, _districtJson, structuredPostalAddress);
            AssertDictionaryValue(state, _stateJson, structuredPostalAddress);
            AssertDictionaryValue(postalCode, _postalCodeJson, structuredPostalAddress);
            AssertDictionaryValue(postOffice, _postOfficeJson, structuredPostalAddress);
            AssertDictionaryValue(countryIso, _countryIsoJson, structuredPostalAddress);
            AssertDictionaryValue(country, _countryJson, structuredPostalAddress);
            AssertDictionaryValue(formattedAddress, _formattedAddressJson, structuredPostalAddress);

            Dictionary<string, JToken> legacyStructuredPostalAddress = _yotiUserProfile.StructuredPostalAddress;
            AssertDictionaryValue(addressFormat, _addressFormatJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(careOf, _careOfJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(building, _buildingJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(street, _streetJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(townCity, _townCityJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(subdistrict, _subdistrictJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(district, _districtJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(state, _stateJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(postalCode, _postalCodeJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(postOffice, _postOfficeJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(countryIso, _countryIsoJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(country, _countryJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(formattedAddress, _formattedAddressJson, legacyStructuredPostalAddress);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_StructuredPostalAddress_USA()
        {
            string addressFormat = "3";
            string addressLineOne = "1512 Ferry Street";
            string townCity = "Anniston";
            string state = "AL";
            string postalCode = "36201";
            string countryIso = "USA";
            string country = "USA";
            string formattedAddress = "1512 Ferry Street\nAnniston AL 36201\nUSA";

            string addressString =
                    "{     \"" + _addressFormatJson + "\": " + addressFormat
                + ",     \"" + _addressLineOneJson + "\": \"" + addressLineOne
                + "\",     \"" + _townCityJson + "\": \"" + townCity
                + "\",     \"" + _stateJson + "\": \"" + state
                + "\",     \"" + _postalCodeJson + "\": \"" + postalCode
                + "\",     \"" + _countryIsoJson + "\": \"" + countryIso
                + "\",     \"" + _countryJson + "\": \"" + country
                + "\",     \"" + _formattedAddressJson + "\": \"" + formattedAddress + "\" }";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = _structuredPostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile(attribute);

            Dictionary<string, JToken> structuredPostalAddress = _yotiProfile.StructuredPostalAddress.GetJsonValue();
            AssertDictionaryValue(addressFormat, _addressFormatJson, structuredPostalAddress);
            AssertDictionaryValue(addressLineOne, _addressLineOneJson, structuredPostalAddress);
            AssertDictionaryValue(townCity, _townCityJson, structuredPostalAddress);
            AssertDictionaryValue(state, _stateJson, structuredPostalAddress);
            AssertDictionaryValue(postalCode, _postalCodeJson, structuredPostalAddress);
            AssertDictionaryValue(countryIso, _countryIsoJson, structuredPostalAddress);
            AssertDictionaryValue(country, _countryJson, structuredPostalAddress);
            AssertDictionaryValue(formattedAddress, _formattedAddressJson, structuredPostalAddress);

            Dictionary<string, JToken> legacyStructuredPostalAddress = _yotiUserProfile.StructuredPostalAddress;
            AssertDictionaryValue(addressFormat, _addressFormatJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(addressLineOne, _addressLineOneJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(townCity, _townCityJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(state, _stateJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(postalCode, _postalCodeJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(countryIso, _countryIsoJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(country, _countryJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(formattedAddress, _formattedAddressJson, legacyStructuredPostalAddress);
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

            string addressFormat = "1";
            string buildingNumber = "15a";
            string addressLineOne = "15a North Street";
            string townCity = "CARSHALTON";
            string postalCode = "SM5 2HW";
            string countryIso = "GBR";
            string country = "UK";
            string formattedAddress = "15a North Street\nCARSHALTON\nSM5 2HW\nUK";

            string nestedValueJson = "nested_value";
            string oneJson = "1";
            string oneTwoJson = "1-2";
            string oneTwoFiveJson = "1-2-5";
            string oneTwoFiveOneJson = "1-2-5-1";
            string oneTwoFiveOneJsonValue = "OneTwoFiveOne";

            string addressString =
                "{     \"" + _addressFormatJson + "\": " + addressFormat
                + ",     \"" + _buildingNumberJson + "\": \"" + buildingNumber
                + "\",     \"" + _addressLineOneJson + "\": \"" + addressLineOne
                + "\",     \"" + _townCityJson + "\": \"" + townCity
                + "\",     \"" + _postalCodeJson + "\": \"" + postalCode
                + "\",     \"" + _countryIsoJson + "\": \"" + countryIso
                + "\",     \"" + _countryJson + "\": \"" + country
                + "\",     \"" + nestedValueJson + "\": " + nestedValueObject.ToString()
                + ",     \"" + _formattedAddressJson + "\": \"" + formattedAddress + "\" }";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = _structuredPostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile(attribute);

            Dictionary<string, JToken> structuredPostalAddress = _yotiProfile.StructuredPostalAddress.GetJsonValue();
            AssertDictionaryValue(addressFormat, _addressFormatJson, structuredPostalAddress);
            AssertDictionaryValue(buildingNumber, _buildingNumberJson, structuredPostalAddress);
            AssertDictionaryValue(addressLineOne, _addressLineOneJson, structuredPostalAddress);
            AssertDictionaryValue(townCity, _townCityJson, structuredPostalAddress);
            AssertDictionaryValue(postalCode, _postalCodeJson, structuredPostalAddress);
            AssertDictionaryValue(countryIso, _countryIsoJson, structuredPostalAddress);
            AssertDictionaryValue(country, _countryJson, structuredPostalAddress);

            JToken nestedJsonObj = structuredPostalAddress[nestedValueJson];
            JToken token1 = nestedJsonObj[oneJson];
            JToken token1_2 = token1[oneTwoJson];
            JToken token1_2_5 = token1_2[oneTwoFiveJson];
            JToken token1_2_5_1 = token1_2_5[oneTwoFiveOneJson];

            Assert.AreEqual(oneTwoFiveOneJsonValue, token1_2_5_1);

            AssertDictionaryValue(nestedValueObject.ToString(), nestedValueJson, structuredPostalAddress);
            AssertDictionaryValue(formattedAddress, _formattedAddressJson, structuredPostalAddress);

            Dictionary<string, JToken> legacyStructuredPostalAddress = _yotiUserProfile.StructuredPostalAddress;
            AssertDictionaryValue(addressFormat, _addressFormatJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(buildingNumber, _buildingNumberJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(addressLineOne, _addressLineOneJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(townCity, _townCityJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(postalCode, _postalCodeJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(countryIso, _countryIsoJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(country, _countryJson, legacyStructuredPostalAddress);

            JToken legacyNestedJsonObj = legacyStructuredPostalAddress[nestedValueJson];
            JToken legacyToken1 = nestedJsonObj[oneJson];
            JToken legacyToken1_2 = legacyToken1[oneTwoJson];
            JToken legacyToken1_2_5 = legacyToken1_2[oneTwoFiveJson];
            JToken legacyToken1_2_5_1 = legacyToken1_2_5[oneTwoFiveOneJson];

            Assert.AreEqual(oneTwoFiveOneJsonValue, legacyToken1_2_5_1);

            AssertDictionaryValue(nestedValueObject.ToString(), nestedValueJson, legacyStructuredPostalAddress);
            AssertDictionaryValue(formattedAddress, _formattedAddressJson, legacyStructuredPostalAddress);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_AddressIsTakenFromFormattedAddressIfNull()
        {
            string addressFormat = "1";
            string buildingNumber = "15a";
            string addressLineOne = "15a North Street";
            string townCity = "CARSHALTON";
            string postalCode = "SM5 2HW";
            string countryIso = "GBR";
            string country = "UK";
            string formattedAddress = "15a North Street\nCARSHALTON\nSM5 2HW\nUK";

            string addressString =
                "{     \"" + _addressFormatJson + "\": " + addressFormat
                + ",     \"" + _buildingNumberJson + "\": \"" + buildingNumber
                + "\",     \"" + _addressLineOneJson + "\": \"" + addressLineOne
                + "\",     \"" + _townCityJson + "\": \"" + townCity
                + "\",     \"" + _postalCodeJson + "\": \"" + postalCode
                + "\",     \"" + _countryIsoJson + "\": \"" + countryIso
                + "\",     \"" + _countryJson + "\": \"" + country
                + "\",     \"" + _formattedAddressJson + "\": \"" + formattedAddress + "\" }";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = _structuredPostalAddressAttribute,
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
            string addressFormat = "1";
            string buildingNumber = "15a";
            string addressLineOne = "15a North Street";
            string townCity = "CARSHALTON";
            string postalCode = "SM5 2HW";
            string countryIso = "GBR";
            string country = "UK";
            string formattedAddress = "15a North Street\nCARSHALTON\nSM5 2HW\nUK";
            string postalAddress = "33a South Street\nCARSHALTON\nSM5 2HW\nUK";

            string structuredAddressString =
                "{     \"" + _addressFormatJson + "\": " + addressFormat
                + ",     \"" + _buildingNumberJson + "\": \"" + buildingNumber
                + "\",     \"" + _addressLineOneJson + "\": \"" + addressLineOne
                + "\",     \"" + _townCityJson + "\": \"" + townCity
                + "\",     \"" + _postalCodeJson + "\": \"" + postalCode
                + "\",     \"" + _countryIsoJson + "\": \"" + countryIso
                + "\",     \"" + _countryJson + "\": \"" + country
                + "\",     \"" + _formattedAddressJson + "\": \"" + formattedAddress + "\" }";

            var structuredAddressAttribute = new AttrpubapiV1.Attribute
            {
                Name = _structuredPostalAddressAttribute,
                ContentType = AttrpubapiV1.ContentType.Json,
                Value = ByteString.CopyFromUtf8(structuredAddressString)
            };

            var addressAttribute = new AttrpubapiV1.Attribute
            {
                Name = _postalAddressAttribute,
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
            string value = "GenderValue";
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = _genderAttribute,
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
            string value = "NationalityValue";

            var attribute = new AttrpubapiV1.Attribute
            {
                Name = _nationalityAttribute,
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
        public void Activity_GetSources_IncludesDrivingLicense_String()
        {
            AttrpubapiV1.Attribute attribute = BuildAnchoredAttribute(
                _givenNamesAttribute,
                "givenNameValue",
                AttrpubapiV1.ContentType.String,
                TestAnchors.DrivingLicenseAnchor);

            AddAttributeToProfile(attribute);

            HashSet<string> sources = _yotiProfile.GivenNames.GetSources();
            Assert.IsTrue(
                sources.Any(
                    s => s.Contains(_drivingLicenseSourceType)));
        }

        [TestMethod]
        public void Activity_GetSources_IncludesDrivingLicense_AgeVerified()
        {
            AttrpubapiV1.Attribute attribute = BuildAnchoredAttribute(
                _ageOver18Attribute,
                "true",
                AttrpubapiV1.ContentType.String,
                TestAnchors.DrivingLicenseAnchor);

            AddAttributeToProfile(attribute);

            HashSet<string> sources = _yotiProfile.AgeVerified.GetSources();
            Assert.IsTrue(
                sources.Any(
                    s => s.Contains(_drivingLicenseSourceType)));
        }

        [TestMethod]
        public void Activity_GetSources_IncludesDrivingLicense_StructuredPostalAddress()
        {
            AttrpubapiV1.Attribute attribute = BuildAnchoredAttribute(
                _structuredPostalAddressAttribute,
                "{ \"properties\": { \"name\": { \"type\": \"string\"     } } }",
                AttrpubapiV1.ContentType.Json,
                TestAnchors.DrivingLicenseAnchor);

            AddAttributeToProfile(attribute);

            HashSet<string> sources = _yotiProfile.StructuredPostalAddress.GetSources();
            Assert.IsTrue(
                sources.Any(
                    s => s.Contains(_drivingLicenseSourceType)));
        }

        [TestMethod]
        public void Activity_GetSources_IncludesPassport()
        {
            AttrpubapiV1.Attribute attribute = BuildAnchoredAttribute(
                _dateOfBirthAttribute,
                _dateOfBirthString,
                AttrpubapiV1.ContentType.Date,
                TestAnchors.PassportAnchor);

            AddAttributeToProfile(attribute);

            HashSet<string> sources = _yotiProfile.DateOfBirth.GetSources();
            Assert.IsTrue(
                sources.Any(
                    s => s.Contains(_passportSourceType)));
        }

        [TestMethod]
        public void Activity_GetVerifiers_IncludesYotiAdmin()
        {
            AttrpubapiV1.Attribute attribute = BuildAnchoredAttribute(
                _selfieAttribute,
                "selfieValue",
                AttrpubapiV1.ContentType.Jpeg,
                TestAnchors.YotiAdminAnchor);

            AddAttributeToProfile(attribute);

            HashSet<string> verifiers = _yotiProfile.Selfie.GetVerifiers();
            Assert.IsTrue(
                verifiers.Any(
                    s => s.Contains(_yotiAdminVerifierType)));
        }

        [TestMethod]
        public void Activity_GetVerifiers_ExcludesDuplicate()
        {
            AttrpubapiV1.Attribute attribute = BuildDoubleAnchoredAttribute(
                _selfieAttribute,
                "selfieValue",
                AttrpubapiV1.ContentType.Jpeg,
                TestAnchors.YotiAdminAnchor);

            AddAttributeToProfile(attribute);

            HashSet<string> verifiers = _yotiProfile.Selfie.GetVerifiers();
            Assert.IsTrue(
                verifiers.Any(
                    s => s.Contains(_yotiAdminVerifierType))
                    && verifiers.Count == 1);
        }

        private static AttrpubapiV1.Attribute BuildAnchoredAttribute(string name, string value, AttrpubapiV1.ContentType contentType, string rawAnchor)
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = name,
                ContentType = contentType,
                Value = ByteString.CopyFromUtf8(value)
            };

            AddAnchorToAttribute(Conversion.Base64ToBytes(rawAnchor), attribute);

            return attribute;
        }

        private static AttrpubapiV1.Attribute BuildDoubleAnchoredAttribute(string name, string value, AttrpubapiV1.ContentType contentType, string rawAnchor)
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = name,
                ContentType = contentType,
                Value = ByteString.CopyFromUtf8(value)
            };

            AddAnchorToAttribute(Conversion.Base64ToBytes(rawAnchor), attribute);
            AddAnchorToAttribute(Conversion.Base64ToBytes(rawAnchor), attribute);

            return attribute;
        }

        private static void AddAnchorToAttribute(byte[] anchorBytes, AttrpubapiV1.Attribute attribute)
        {
            attribute.Anchors.AddRange(
                new RepeatedField<AttrpubapiV1.Anchor>
                {
                    AttrpubapiV1.Anchor.Parser.ParseFrom(anchorBytes)
                });
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