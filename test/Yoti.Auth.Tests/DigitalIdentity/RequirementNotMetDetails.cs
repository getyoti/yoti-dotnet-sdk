using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Yoti.DigitalIdentity.Tests
{
    [TestClass]
    public class RequirementNotMetDetailsTests
    {
        [TestMethod]
        public void DeserializeValidJsonCreatesRequirementNotMetDetails()
        {
            var json = @"
            {
                ""failure_type"": ""DOCUMENT_EXPIRED"",
                ""details"": ""The document has expired."",
                ""audit_id"": ""AUDIT123"",
                ""document_country_iso_code"": ""USA"",
                ""document_type"": ""PASSPORT""
            }";
            
            var details = JsonConvert.DeserializeObject<RequirementNotMetDetails>(json);

            Assert.IsNotNull(details);
            Assert.AreEqual("DOCUMENT_EXPIRED", details.FailureType);
            Assert.AreEqual("The document has expired.", details.Details);
            Assert.AreEqual("AUDIT123", details.AuditId);
            Assert.AreEqual("USA", details.DocumentCountryIsoCode);
            Assert.AreEqual("PASSPORT", details.DocumentType);
        }
        

        [TestMethod]
        public void PropertyGettersReturnCorrectValues()
        {
            var json = @"
            {
                ""failure_type"": ""DOCUMENT_EXPIRED"",
                ""details"": ""The document has expired."",
                ""audit_id"": ""AUDIT123"",
                ""document_country_iso_code"": ""USA"",
                ""document_type"": ""PASSPORT""
            }";

            var details = JsonConvert.DeserializeObject<RequirementNotMetDetails>(json);
            
            Assert.AreEqual("DOCUMENT_EXPIRED", details.FailureType);
            Assert.AreEqual("The document has expired.", details.Details);
            Assert.AreEqual("AUDIT123", details.AuditId);
            Assert.AreEqual("USA", details.DocumentCountryIsoCode);
            Assert.AreEqual("PASSPORT", details.DocumentType);
        }
    }
}
