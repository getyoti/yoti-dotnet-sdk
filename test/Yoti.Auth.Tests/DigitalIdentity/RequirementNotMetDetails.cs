using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using Yoti.DigitalIdentity;

namespace Yoti.DigitalIdentity.Tests
{
    [TestClass]
    public class RequirementNotMetDetailsTests
    {
        [TestMethod]
        public void DeserializeValidJsonCreatesRequirementNotMetDetails()
        {
            // Arrange
            var json = @"
            {
                ""failure_type"": ""DOCUMENT_EXPIRED"",
                ""details"": ""The document has expired."",
                ""audit_id"": ""AUDIT123"",
                ""document_country_iso_code"": ""USA"",
                ""document_type"": ""PASSPORT""
            }";

            // Act
            var details = JsonConvert.DeserializeObject<RequirementNotMetDetails>(json);

            // Assert
            Assert.IsNotNull(details);
            Assert.AreEqual("DOCUMENT_EXPIRED", details.GetFailureType());
            Assert.AreEqual("The document has expired.", details.GetDetails());
            Assert.AreEqual("AUDIT123", details.GetAuditId());
            Assert.AreEqual("USA", details.GetDocumentCountryIsoCode());
            Assert.AreEqual("PASSPORT", details.GetDocumentType());
        }
        

        [TestMethod]
        public void PropertyGettersReturnCorrectValues()
        {
            // Arrange
            var json = @"
            {
                ""failure_type"": ""DOCUMENT_EXPIRED"",
                ""details"": ""The document has expired."",
                ""audit_id"": ""AUDIT123"",
                ""document_country_iso_code"": ""USA"",
                ""document_type"": ""PASSPORT""
            }";

            var details = JsonConvert.DeserializeObject<RequirementNotMetDetails>(json);

            // Act & Assert
            Assert.AreEqual("DOCUMENT_EXPIRED", details.GetFailureType());
            Assert.AreEqual("The document has expired.", details.GetDetails());
            Assert.AreEqual("AUDIT123", details.GetAuditId());
            Assert.AreEqual("USA", details.GetDocumentCountryIsoCode());
            Assert.AreEqual("PASSPORT", details.GetDocumentType());
        }
    }
}
