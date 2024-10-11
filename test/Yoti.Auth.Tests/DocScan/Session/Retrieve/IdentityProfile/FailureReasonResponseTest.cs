using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Retrieve.IdentityProfile.Tests
{
    [TestClass]
    public class FailureReasonResponseTests
    {
        [TestMethod]
        public void Deserialize_ValidJson_CreatesFailureReasonResponse()
        {
            // Arrange
            var json = @"
            {
                ""reason_code"": ""CODE123"",
                ""requirements_not_met_details"": [{
                    ""failure_type"": ""DOCUMENT_EXPIRED"",
                    ""details"": ""The document has expired."",
                    ""audit_id"": ""AUDIT123"",
                    ""document_country_iso_code"": ""USA"",
                    ""document_type"": ""PASSPORT""
                }]
            }";

            var response = JsonConvert.DeserializeObject<FailureReasonResponse>(json);

            Assert.IsNotNull(response);
            Assert.AreEqual("CODE123", response.ReasonCode);
            Assert.IsNotNull(response.RequirementNotMetDetails);
            Assert.AreEqual("DOCUMENT_EXPIRED", response.RequirementNotMetDetails[0].FailureType);
            Assert.AreEqual("The document has expired.", response.RequirementNotMetDetails[0].Details);
            Assert.AreEqual("AUDIT123", response.RequirementNotMetDetails[0].AuditId);
            Assert.AreEqual("USA", response.RequirementNotMetDetails[0].DocumentCountryIsoCode);
            Assert.AreEqual("PASSPORT", response.RequirementNotMetDetails[0].DocumentType);
        }
    }

}
