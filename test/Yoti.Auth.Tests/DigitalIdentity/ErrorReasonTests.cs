using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DigitalIdentity;

namespace Yoti.DigitalIdentity.Tests
{
    [TestClass]
    public class ErrorReasonTests
    {
        [TestMethod]
        public void Deserialize_ValidJson_CreatesErrorReason()
        {
            var json = @"
            {
                ""requirements_not_met_details"": [
                    {
                        ""failure_type"": ""ID_DOCUMENT_COUNTRY"",
                        ""details"": ""NOT_APPLICABLE_FOR_SCHEME"",
                        ""audit_id"": ""97001564-a18a-4afd-bf19-3ffacc88abbb"",
                        ""document_country_iso_code"": ""IRL"",
                        ""document_type"": ""PASSPORT""
                    }
                ]
            }";

            var errorReason = JsonConvert.DeserializeObject<ErrorReason>(json);

            Assert.IsNotNull(errorReason);
            Assert.IsNotNull(errorReason.RequirementsNotMetDetails);
            Assert.AreEqual(1, errorReason.RequirementsNotMetDetails.Count);
            Assert.AreEqual("ID_DOCUMENT_COUNTRY", errorReason.RequirementsNotMetDetails[0].FailureType);
            Assert.AreEqual("NOT_APPLICABLE_FOR_SCHEME", errorReason.RequirementsNotMetDetails[0].Details);
            Assert.AreEqual("97001564-a18a-4afd-bf19-3ffacc88abbb", errorReason.RequirementsNotMetDetails[0].AuditId);
            Assert.AreEqual("IRL", errorReason.RequirementsNotMetDetails[0].DocumentCountryIsoCode);
            Assert.AreEqual("PASSPORT", errorReason.RequirementsNotMetDetails[0].DocumentType);
        }

        [TestMethod]
        public void Deserialize_EmptyArray_CreatesEmptyList()
        {
            var json = @"
            {
                ""requirements_not_met_details"": []
            }";

            var errorReason = JsonConvert.DeserializeObject<ErrorReason>(json);

            Assert.IsNotNull(errorReason);
            Assert.IsNotNull(errorReason.RequirementsNotMetDetails);
            Assert.AreEqual(0, errorReason.RequirementsNotMetDetails.Count);
        }
    }
}
