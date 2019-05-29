using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Yoti.Auth.Attribute;
using Yoti.Auth.Document;
using Yoti.Auth.Images;
using Yoti.Auth.Profile;
using Yoti.Auth.Tests.TestData;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class ActivityTests
    {
        private YotiProfile _yotiProfile;
        private JToken _dictionaryObject = null;

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

        private const string StringValue = "Value";
        private readonly ByteString _byteValue = ByteString.CopyFromUtf8(StringValue);

        private const string DateOfBirthString = "1980-01-13";
        private static readonly DateTime DateOfBirthValue = new DateTime(1980, 1, 13);

        [TestInitialize]
        public void Startup()
        {
            _yotiProfile = new YotiProfile();
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_Selfie_Jpeg()
        {
            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.SelfieAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.Jpeg,
                Value = _byteValue
            };

            AddAttributeToProfile<Image>(attribute);

            Assert.AreEqual("image/jpeg", _yotiProfile.Selfie.GetValue().GetMIMEType());
            Assert.AreEqual(
                "data:image/jpeg;base64," + Conversion.BytesToBase64(Encoding.UTF8.GetBytes(StringValue)),
                _yotiProfile.Selfie.GetValue().GetBase64URI());
            Assert.IsNotNull(_yotiProfile.Selfie.GetValue());
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_Selfie_Png()
        {
            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.SelfieAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.Png,
                Value = _byteValue
            };

            AddAttributeToProfile<Image>(attribute);

            Assert.AreEqual(
                "data:image/png;base64," + Conversion.BytesToBase64(Encoding.UTF8.GetBytes(StringValue)),
                _yotiProfile.Selfie.GetValue().GetBase64URI());

            Assert.IsNotNull(_yotiProfile.Selfie.GetValue());
            Assert.IsTrue(Encoding.UTF8.GetBytes(StringValue).SequenceEqual(_yotiProfile.Selfie.GetValue().GetContent()));
            Assert.AreEqual("image/png", _yotiProfile.Selfie.GetValue().GetMIMEType());
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_FullName()
        {
            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.FullNameAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.String,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(_yotiProfile.FullName.GetValue(), StringValue);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_GivenNames()
        {
            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.GivenNamesAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.String,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(_yotiProfile.GivenNames.GetValue(), StringValue);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_FamilyName()
        {
            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.FamilyNameAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.String,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(_yotiProfile.FamilyName.GetValue(), StringValue);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_MobileNumber()
        {
            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.PhoneNumberAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.String,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(_yotiProfile.MobileNumber.GetValue(), StringValue);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_EmailAddress()
        {
            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.EmailAddressAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.String,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(_yotiProfile.EmailAddress.GetValue(), StringValue);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_DateOfBirth()
        {
            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.DateOfBirthAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.Date,
                Value = ByteString.CopyFromUtf8(DateOfBirthString)
            };

            AddAttributeToProfile<DateTime>(attribute);

            Assert.IsInstanceOfType(_yotiProfile.DateOfBirth.GetValue(), typeof(DateTime));
            Assert.AreEqual(_yotiProfile.DateOfBirth.GetValue(), DateOfBirthValue);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_Address()
        {
            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.PostalAddressAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.String,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(StringValue, _yotiProfile.Address.GetValue());
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

            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.StructuredPostalAddressAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile<Dictionary<string, JToken>>(attribute);

            Dictionary<string, JToken> structuredPostalAddress = _yotiProfile.StructuredPostalAddress.GetValue();
            AssertDictionaryValue(addressFormat, AddressFormatJson, structuredPostalAddress);
            AssertDictionaryValue(buildingNumber, BuildingNumberJson, structuredPostalAddress);
            AssertDictionaryValue(addressLineOne, AddressLineOneJson, structuredPostalAddress);
            AssertDictionaryValue(townCity, TownCityJson, structuredPostalAddress);
            AssertDictionaryValue(postalCode, PostalCodeJson, structuredPostalAddress);
            AssertDictionaryValue(countryIso, CountryIsoJson, structuredPostalAddress);
            AssertDictionaryValue(country, CountryJson, structuredPostalAddress);
            AssertDictionaryValue(formattedAddress, FormattedAddressJson, structuredPostalAddress);
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

            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.StructuredPostalAddressAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile<Dictionary<string, JToken>>(attribute);

            Dictionary<string, JToken> structuredPostalAddress = _yotiProfile.StructuredPostalAddress.GetValue();
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

            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.StructuredPostalAddressAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile<Dictionary<string, JToken>>(attribute);

            Dictionary<string, JToken> structuredPostalAddress = _yotiProfile.StructuredPostalAddress.GetValue();
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

            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.StructuredPostalAddressAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile<Dictionary<string, JToken>>(attribute);

            Dictionary<string, JToken> structuredPostalAddress = _yotiProfile.StructuredPostalAddress.GetValue();
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

            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.StructuredPostalAddressAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.Json,
                Value = ByteString.CopyFromUtf8(addressString)
            };

            AddAttributeToProfile<Dictionary<string, JToken>>(attribute);

            ProfileParser.SetAddressToBeFormattedAddressIfNull(_yotiProfile);

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

            var structuredAddressAttribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.StructuredPostalAddressAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.Json,
                Value = ByteString.CopyFromUtf8(structuredAddressString)
            };

            var addressAttribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.PostalAddressAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.String,
                Value = ByteString.CopyFromUtf8(postalAddress)
            };

            AddAttributeToProfile<Dictionary<string, JToken>>(structuredAddressAttribute);
            AddAttributeToProfile<string>(addressAttribute);
            ProfileParser.SetAddressToBeFormattedAddressIfNull(_yotiProfile);

            Assert.AreNotEqual(_yotiProfile.Address.GetValue(), formattedAddress);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_Gender()
        {
            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.GenderAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.String,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(_yotiProfile.Gender.GetValue(), StringValue);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_Nationality()
        {
            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.NationalityAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.String,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(_yotiProfile.Nationality.GetValue(), StringValue);
        }

        [TestMethod]
        public void Activity_AddAttributesToProfile_DocumentDetails()
        {
            string issuingCountry = "GBR";
            string documentNumber = "1234abc";
            DateTime expirationDate = new DateTime(2015, 5, 1);
            string expirationDateString = expirationDate.ToString("yyyy-MM-dd");
            string documentType = "DRIVING_LICENCE";
            string issuingAuthority = "DVLA";

            string documentDetailsString = string.Format("{0} {1} {2} {3} {4}",
                    documentType,
                    issuingCountry,
                    documentNumber,
                    expirationDateString,
                    issuingAuthority);

            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.DocumentDetailsAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.String,
                Value = ByteString.CopyFromUtf8(documentDetailsString)
            };

            AddAttributeToProfile<DocumentDetails>(attribute);

            DocumentDetails actualDocumentDetails = _yotiProfile.DocumentDetails.GetValue();
            Assert.AreEqual(issuingCountry, actualDocumentDetails.IssuingCountry);
            Assert.AreEqual(documentNumber, actualDocumentDetails.DocumentNumber);
            Assert.AreEqual(expirationDate, actualDocumentDetails.ExpirationDate);
            Assert.AreEqual(documentType, actualDocumentDetails.DocumentType);
            Assert.AreEqual(issuingAuthority, actualDocumentDetails.IssuingAuthority);
            Assert.AreEqual(documentDetailsString, actualDocumentDetails.ToString());
        }

        [TestMethod]
        public void Activity_GetSources_IncludesDrivingLicense_String()
        {
            ProtoBuf.Attribute.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                Constants.UserProfile.GivenNamesAttribute,
                StringValue,
                ProtoBuf.Attribute.ContentType.String,
                TestAnchors.DrivingLicenseAnchor);

            AddAttributeToProfile<string>(attribute);

            IEnumerable<Anchors.Anchor> sources = _yotiProfile.GivenNames.GetSources();

            Assert.IsTrue(
                sources.Any(
                    s => s.GetValue().Contains(DrivingLicenseSourceType)));
        }

        [TestMethod]
        public void Activity_GetSources_IncludesDrivingLicense()
        {
            ProtoBuf.Attribute.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                Constants.UserProfile.EmailAddressAttribute,
                "true",
                ProtoBuf.Attribute.ContentType.String,
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
            ProtoBuf.Attribute.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                Constants.UserProfile.StructuredPostalAddressAttribute,
                "{ \"properties\": { \"name\": { \"type\": \"string\"} } }",
                ProtoBuf.Attribute.ContentType.Json,
                TestAnchors.DrivingLicenseAnchor);

            AddAttributeToProfile<Dictionary<string, JToken>>(attribute);

            IEnumerable<Anchors.Anchor> sources = _yotiProfile.StructuredPostalAddress.GetSources();
            Assert.IsTrue(
                sources.Any(
                    s => s.GetValue().Contains(DrivingLicenseSourceType)));
        }

        [TestMethod]
        public void Activity_GetSources_IncludesPassport()
        {
            ProtoBuf.Attribute.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                Constants.UserProfile.DateOfBirthAttribute,
                DateOfBirthString,
                ProtoBuf.Attribute.ContentType.Date,
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
            ProtoBuf.Attribute.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                Constants.UserProfile.SelfieAttribute,
                StringValue,
                ProtoBuf.Attribute.ContentType.Jpeg,
                TestAnchors.YotiAdminAnchor);

            AddAttributeToProfile<Image>(attribute);

            IEnumerable<Anchors.Anchor> verifiers = _yotiProfile.Selfie.GetVerifiers();
            Assert.IsTrue(
                verifiers.Any(
                    s => s.GetValue().Contains(YotiAdminVerifierType)));
        }

        [TestMethod]
        public void AddAttributesToProfile_UndefinedContentType_ConvertedToString()
        {
            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.FamilyNameAttribute,
                ContentType = ProtoBuf.Attribute.ContentType.Undefined,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(StringValue, _yotiProfile.FamilyName.GetValue());
        }

        [TestMethod]
        public void AddAttributesToProfile_NewContentType_IsRetrieved()
        {
            string name = "newType";
            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = name,
                ContentType = (ProtoBuf.Attribute.ContentType)99,
                Value = _byteValue
            };

            AddAttributeToProfile<string>(attribute);

            Assert.AreEqual(StringValue, _yotiProfile.GetAttributeByName<string>(name).GetValue().ToString());
        }

        private void AssertDictionaryValue(string expectedValue, string dictionaryKey, Dictionary<string, JToken> dictionary)
        {
            dictionary.TryGetValue(dictionaryKey, out _dictionaryObject);
            Assert.AreEqual(expectedValue, _dictionaryObject.ToString());
        }

        private void AddAttributeToProfile<T>(ProtoBuf.Attribute.Attribute attribute)
        {
            BaseAttribute parsedAttribute = AttributeConverter.ConvertToBaseAttribute(attribute);

            if (parsedAttribute != null)
                _yotiProfile.Add((YotiAttribute<T>)parsedAttribute);
        }
    }
}