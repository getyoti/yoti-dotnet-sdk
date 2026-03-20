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
            var json = @"
            {
                ""reason_code"": ""CODE123"",
                ""requirements_not_met_details"": [
                    {
                        ""failure_type"": ""DOCUMENT_EXPIRED"",
                        ""details"": ""The document has expired."",
                        ""audit_id"": ""AUDIT123"",
                        ""document_country_iso_code"": ""USA"",
                        ""document_type"": ""PASSPORT""
                    }
                ]
            }";

            var response = JsonConvert.DeserializeObject<FailureReasonResponse>(json);

            Assert.IsNotNull(response);
            Assert.AreEqual("CODE123", response.ReasonCode);
            Assert.IsNotNull(response.RequirementsNotMetDetails);
            Assert.AreEqual(1, response.RequirementsNotMetDetails.Count);
            Assert.AreEqual("DOCUMENT_EXPIRED", response.RequirementsNotMetDetails[0].FailureType);
            Assert.AreEqual("The document has expired.", response.RequirementsNotMetDetails[0].Details);
            Assert.AreEqual("AUDIT123", response.RequirementsNotMetDetails[0].AuditId);
            Assert.AreEqual("USA", response.RequirementsNotMetDetails[0].DocumentCountryIsoCode);
            Assert.AreEqual("PASSPORT", response.RequirementsNotMetDetails[0].DocumentType);
        }

        [TestMethod]
        public void Deserialize_EmptyArray_CreatesEmptyList()
        {
            var json = @"
            {
                ""reason_code"": ""CODE456"",
                ""requirements_not_met_details"": []
            }";

            var response = JsonConvert.DeserializeObject<FailureReasonResponse>(json);

            Assert.IsNotNull(response);
            Assert.AreEqual("CODE456", response.ReasonCode);
            Assert.IsNotNull(response.RequirementsNotMetDetails);
            Assert.AreEqual(0, response.RequirementsNotMetDetails.Count);
        }
    }
}
