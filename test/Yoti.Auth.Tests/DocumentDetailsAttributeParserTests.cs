using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Document;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class DocumentDetailsAttributeParserTests
    {
        [TestMethod]
        public void ShouldThrowExceptionForNullAttribute()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>

               {
                   DocumentDetailsAttributeParser.ParseFrom(null);
               });
        }

        [TestMethod]
        public void ShouldThrowExceptionWhenAttributesAreMissing()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                DocumentDetailsAttributeParser.ParseFrom("PASSPORT GBR");
            });
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("''")]
        public void ShouldThrowExceptionForInvalidValues(string value)
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                DocumentDetailsAttributeParser.ParseFrom(value);
            });
        }

        [TestMethod]
        public void ShouldParseMandatoryAttributes()
        {
            DocumentDetails result = DocumentDetailsAttributeParser.ParseFrom("PASSPORT GBR 1234abc");

            Assert.IsNotNull(result);
            Assert.AreEqual(Constants.DocumentDetails.DocumentTypePassport, result.DocumentType);
            Assert.AreEqual("GBR", result.IssuingCountry);
            Assert.AreEqual("1234abc", result.DocumentNumber);
            Assert.IsNull(result.ExpirationDate);
            Assert.IsNull(result.IssuingAuthority);
        }

        [TestMethod]
        public void ShouldParseOneOptionalAttribute()
        {
            DocumentDetails result = DocumentDetailsAttributeParser.ParseFrom("AADHAAR IND 1234abc 2016-05-01");

            Assert.IsNotNull(result);
            Assert.AreEqual(Constants.DocumentDetails.DocumentTypeAadhaar, result.DocumentType);
            Assert.AreEqual("IND", result.IssuingCountry);
            Assert.AreEqual("1234abc", result.DocumentNumber);
            Assert.AreEqual(new DateTime(2016, 05, 01), result.ExpirationDate);
            Assert.IsNull(result.IssuingAuthority);
        }

        [TestMethod]
        public void ShouldParseTwoOptionalAttributes()
        {
            DocumentDetails result = DocumentDetailsAttributeParser.ParseFrom("DRIVING_LICENCE GBR 1234abc 2016-05-01 DVLA");

            Assert.IsNotNull(result);
            Assert.AreEqual(Constants.DocumentDetails.DocumentTypeDrivingLicense, result.DocumentType);
            Assert.AreEqual("GBR", result.IssuingCountry);
            Assert.AreEqual("1234abc", result.DocumentNumber);
            Assert.AreEqual(new DateTime(2016, 05, 01), result.ExpirationDate);
            Assert.AreEqual("DVLA", result.IssuingAuthority);
        }

        [TestMethod]
        public void ShouldIgnoreAThirdOptionalAttribute()
        {
            DocumentDetails result = DocumentDetailsAttributeParser.ParseFrom("DRIVING_LICENCE GBR 1234abc 2016-05-01 DVLA someThirdAttribute");

            Assert.IsNotNull(result);
            Assert.AreEqual(Constants.DocumentDetails.DocumentTypeDrivingLicense, result.DocumentType);
            Assert.AreEqual("GBR", result.IssuingCountry);
            Assert.AreEqual("1234abc", result.DocumentNumber);
            Assert.AreEqual(new DateTime(2016, 05, 01), result.ExpirationDate);
            Assert.AreEqual("DVLA", result.IssuingAuthority);
        }

        [TestMethod]
        public void ShouldParseWhenOneOptionalAttributeIsMissing()
        {
            DocumentDetails result = DocumentDetailsAttributeParser.ParseFrom("PASS_CARD GBR 1234abc - DVLA");

            Assert.IsNotNull(result);
            Assert.AreEqual(Constants.DocumentDetails.DocumentTypePassCard, result.DocumentType);
            Assert.AreEqual("GBR", result.IssuingCountry);
            Assert.AreEqual("1234abc", result.DocumentNumber);
            Assert.IsNull(result.ExpirationDate);
            Assert.AreEqual("DVLA", result.IssuingAuthority);
        }

        [TestMethod]
        public void ShouldParseRedactedAadhar()
        {
            DocumentDetails result = DocumentDetailsAttributeParser.ParseFrom("NATIONAL_ID IND ********6421 - UIDAI");

            Assert.IsNotNull(result);
            Assert.AreEqual(Constants.DocumentDetails.DocumentTypeNationalId, result.DocumentType);
            Assert.AreEqual("IND", result.IssuingCountry);
            Assert.AreEqual("********6421", result.DocumentNumber);
            Assert.IsNull(result.ExpirationDate);
            Assert.AreEqual("UIDAI", result.IssuingAuthority);
        }

        [DataTestMethod]
        [DataRow("****")]
        [DataRow("~!@#$%^&*()-_=+[]{}|;':,./<>?")]
        [DataRow("\"\"")]
        [DataRow("\\")]
        [DataRow("\"")]
        [DataRow("''")]
        [DataRow("'")]
        public void ShouldParseDocumentDetailsWithSpecialCharacters(string documentNumber)
        {
            DocumentDetails result = DocumentDetailsAttributeParser.ParseFrom($"PASS_CARD GBR {documentNumber} - DVLA");

            Assert.AreEqual(documentNumber, result.DocumentNumber);
        }

        [DataTestMethod]
        [DataRow("PASSPORT  GBR 1234abc")]
        [DataRow("PASSPORT GBR  1234abc")]
        [DataRow("DRIVING_LICENCE GBR 1234abc  2016-05-01 DVLA")]
        [DataRow("DRIVING_LICENCE GBR 1234abc 2016-05-01  DVLA")]
        public void ShouldFailForMoreThanOneConsecutiveSpaces(string stringToParse)
        {
            Assert.ThrowsException<FormatException>(() =>
            {
                DocumentDetails result = DocumentDetailsAttributeParser.ParseFrom(stringToParse);
            });
        }

        [TestMethod]
        public void ShouldThrowExceptionForInvalidDate()
        {
            Assert.ThrowsException<FormatException>(() =>
            {
                DocumentDetailsAttributeParser.ParseFrom("PASSPORT GBR 1234abc" + " X016-05-01");
            });
        }
    }
}