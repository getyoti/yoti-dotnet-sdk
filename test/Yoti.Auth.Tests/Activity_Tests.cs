﻿using System;
using System.Collections.Generic;
using System.IO;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

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
        private string _addressFormatJson = "address_format";
        private string _buildingNumberJson = "building_number";
        private string _addressLineOneJson = "address_line1";
        private string _townCityJson = "town_city";
        private string _postalCodeJson = "postal_code";
        private string _countryIsoJson = "country_iso";
        private string _countryJson = "country";
        private string _formattedAddressJson = "formatted_address";
        private string _stateJson = "state";
        private string _careOfJson = "care_of";
        private string _buildingJson = "building";
        private string _streetJson = "street";
        private string _subdistrictJson = "subdistrict";
        private string _districtJson = "district";
        private string _postOfficeJson = "post_office";

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
                Name = "selfie",
                ContentType = AttrpubapiV1.ContentType.Jpeg,
                Value = ByteString.CopyFromUtf8("selfieJpegValue")
            };

            AddAttributeToProfile(attribute);

            Assert.IsNotNull(_yotiProfile.Selfie.Base64URI);
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
                Name = "selfie",
                ContentType = AttrpubapiV1.ContentType.Png,
                Value = ByteString.CopyFromUtf8("selfiePngValue")
            };

            AddAttributeToProfile(attribute);

            Assert.IsNotNull(_yotiProfile.Selfie.Base64URI);
            Assert.IsNotNull(_yotiProfile.Selfie.GetImage());

            Assert.IsNotNull(_yotiUserProfile.Selfie.Base64URI);
            Assert.IsNotNull(_yotiUserProfile.Selfie.Data);
            Assert.IsNotNull(_yotiUserProfile.Selfie.Type);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_FullName()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "full_name",
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8("fullNameValue")
            };

            AddAttributeToProfile(attribute);

            Assert.IsNotNull(_yotiProfile.FullName.GetValue());

            Assert.IsNotNull(_yotiUserProfile.FullName);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_GivenNames()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "given_names",
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8("GivenNamesValue")
            };

            AddAttributeToProfile(attribute);

            Assert.IsNotNull(_yotiProfile.GivenNames.GetValue());

            Assert.IsNotNull(_yotiUserProfile.GivenNames);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_FamilyName()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "family_name",
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8("FamilyNameValue")
            };

            AddAttributeToProfile(attribute);

            Assert.IsNotNull(_yotiProfile.FamilyName.GetValue());

            Assert.IsNotNull(_yotiUserProfile.FamilyName);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_MobileNumber()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "phone_number",
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8("0123456789")
            };

            AddAttributeToProfile(attribute);

            Assert.IsNotNull(_yotiProfile.MobileNumber.GetValue());

            Assert.IsNotNull(_yotiUserProfile.MobileNumber);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_EmailAddress()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "email_address",
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8("EmailAddressValue")
            };

            AddAttributeToProfile(attribute);

            Assert.IsNotNull(_yotiProfile.EmailAddress.GetValue());

            Assert.IsNotNull(_yotiUserProfile.EmailAddress);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_DateOfBirth()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "date_of_birth",
                ContentType = AttrpubapiV1.ContentType.Date,
                Value = ByteString.CopyFromUtf8("1980-01-13")
            };

            AddAttributeToProfile(attribute);

            Assert.IsInstanceOfType(_yotiProfile.DateOfBirth.GetValue(), typeof(DateTime?));
            Assert.IsNotNull(_yotiProfile.DateOfBirth.GetValue());

            Assert.IsNotNull(_yotiUserProfile.DateOfBirth);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_DateOfBirth_IncorrectFormatNotAdded()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "date_of_birth",
                ContentType = AttrpubapiV1.ContentType.Date,
                Value = ByteString.CopyFromUtf8("1980/01/13")
            };

            AddAttributeToProfile(attribute);

            Assert.IsNull(_yotiProfile.DateOfBirth);

            Assert.IsNull(_yotiUserProfile.DateOfBirth);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_IsAgeVerified_Over_True()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "age_over:18",
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8("true")
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.IsAgeVerified.GetValue(), true);

            Assert.AreEqual(_yotiUserProfile.IsAgeVerified, true);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_IsAgeVerified_Over_False()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "age_over:18",
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8("false")
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.IsAgeVerified.GetValue(), false);

            Assert.AreEqual(_yotiUserProfile.IsAgeVerified, false);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_IsAgeVerified_Under_True()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "age_under:18",
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8("true")
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.IsAgeVerified.GetValue(), true);

            Assert.AreEqual(_yotiUserProfile.IsAgeVerified, true);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_IsAgeVerified_Under_False()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "age_under:18",
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8("false")
            };

            AddAttributeToProfile(attribute);

            Assert.AreEqual(_yotiProfile.IsAgeVerified.GetValue(), false);

            Assert.AreEqual(_yotiUserProfile.IsAgeVerified, false);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_Address()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "postal_address",
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8("AddressValue")
            };

            AddAttributeToProfile(attribute);

            Assert.IsNotNull(_yotiProfile.Address.GetValue());

            Assert.IsNotNull(_yotiUserProfile.Address);
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
                Name = "structured_postal_address",
                ContentType = AttrpubapiV1.ContentType.String,
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
                Name = "structured_postal_address",
                ContentType = AttrpubapiV1.ContentType.String,
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
                Name = "structured_postal_address",
                ContentType = AttrpubapiV1.ContentType.String,
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
                Name = "structured_postal_address",
                ContentType = AttrpubapiV1.ContentType.String,
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
        public void Activity_AddAttributesToProfile_Gender()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "gender",
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8("GenderValue")
            };

            AddAttributeToProfile(attribute);

            Assert.IsNotNull(_yotiProfile.Gender.GetValue());

            Assert.IsNotNull(_yotiUserProfile.Gender);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_Nationality()
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = "nationality",
                ContentType = AttrpubapiV1.ContentType.String,
                Value = ByteString.CopyFromUtf8("NationalityValue")
            };

            AddAttributeToProfile(attribute);

            Assert.IsNotNull(_yotiProfile.Nationality.GetValue());

            Assert.IsNotNull(_yotiUserProfile.Nationality);
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