using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DigitalIdentity;

namespace Yoti.Auth.Tests.DigitalIdentity
{
    [TestClass]
    public class ErrorReasonTests
    {
        [TestMethod]
        public void Deserialize_ValidJson_CreatesErrorReason()
        {
            var json = @"
            {
                ""requirements_not_met_details"": {
                    ""failure_type"": ""ID_DOCUMENT_COUNTRY"",
                    ""details"": ""NOT_APPLICABLE_FOR_SCHEME"",
                    ""audit_id"": ""97001564-a18a-4afd-bf19-3ffacc88abbb"",
                    ""document_country_iso_code"": ""IRL"",
                    ""document_type"": ""PASSPORT""
                }
            }";

            var errorReason = JsonConvert.DeserializeObject<ErrorReason>(json);

            Assert.IsNotNull(errorReason);
            Assert.IsNotNull(errorReason.RequirementNotMetDetails);
            Assert.AreEqual("ID_DOCUMENT_COUNTRY", errorReason.RequirementNotMetDetails.FailureType);
            Assert.AreEqual("NOT_APPLICABLE_FOR_SCHEME", errorReason.RequirementNotMetDetails.Details);
            Assert.AreEqual("97001564-a18a-4afd-bf19-3ffacc88abbb", errorReason.RequirementNotMetDetails.AuditId);
            Assert.AreEqual("IRL", errorReason.RequirementNotMetDetails.DocumentCountryIsoCode);
            Assert.AreEqual("PASSPORT", errorReason.RequirementNotMetDetails.DocumentType);
        }

        [TestMethod]
        public void Deserialize_NullDetails_LeavesDetailsNull()
        {
            var json = @"{ ""requirements_not_met_details"": null }";

            var errorReason = JsonConvert.DeserializeObject<ErrorReason>(json);

            Assert.IsNotNull(errorReason);
            Assert.IsNull(errorReason.RequirementNotMetDetails);
        }
    }
}
