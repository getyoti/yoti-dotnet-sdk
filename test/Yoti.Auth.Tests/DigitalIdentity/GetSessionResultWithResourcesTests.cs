using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DigitalIdentity;

namespace Yoti.Auth.Tests.DigitalIdentity
{
    [TestClass]
    public class GetSessionResultWithResourcesTests
    {
        [TestMethod]
        public void ShouldDeserializeSessionWithResources()
        {
            const string json = @"{
                ""id"": ""session-123"",
                ""status"": ""COMPLETED"",
                ""expiry"": ""2023-12-31T23:59:59Z"",
                ""created"": ""2023-01-01T00:00:00Z"",
                ""updated"": ""2023-01-01T12:00:00Z"",
                ""qrCode"": {
                    ""id"": ""qr-123""
                },
                ""receipt"": {
                    ""id"": ""receipt-123""
                },
                ""resources"": {
                    ""share_codes"": [
                        {
                            ""id"": ""share-code-1"",
                            ""source"": ""USER_PROVIDED"",
                            ""created_at"": ""2023-01-01T12:00:00Z"",
                            ""last_updated"": ""2023-01-01T13:00:00Z"",
                            ""tasks"": []
                        }
                    ]
                }
            }";

            GetSessionResult result = JsonConvert.DeserializeObject<GetSessionResult>(json);

            Assert.IsNotNull(result);
            Assert.AreEqual("session-123", result.Id);
            Assert.AreEqual("COMPLETED", result.Status);
            
            Assert.IsNotNull(result.Resources);
            Assert.IsNotNull(result.Resources.ShareCodes);
            Assert.AreEqual(1, result.Resources.ShareCodes.Count);
            Assert.AreEqual("share-code-1", result.Resources.ShareCodes[0].Id);
            Assert.AreEqual("USER_PROVIDED", result.Resources.ShareCodes[0].Source);
        }

        [TestMethod]
        public void ShouldHandleSessionWithoutResources()
        {
            const string json = @"{
                ""id"": ""session-123"",
                ""status"": ""PENDING"",
                ""expiry"": ""2023-12-31T23:59:59Z"",
                ""created"": ""2023-01-01T00:00:00Z"",
                ""updated"": ""2023-01-01T12:00:00Z"",
                ""qrCode"": {
                    ""id"": ""qr-123""
                },
                ""receipt"": {
                    ""id"": ""receipt-123""
                }
            }";

            GetSessionResult result = JsonConvert.DeserializeObject<GetSessionResult>(json);

            Assert.IsNotNull(result);
            Assert.AreEqual("session-123", result.Id);
            Assert.IsNull(result.Resources);
        }

        [TestMethod]
        public void ShouldHandleResourcesWithEmptyShareCodes()
        {
            const string json = @"{
                ""id"": ""session-123"",
                ""status"": ""COMPLETED"",
                ""expiry"": ""2023-12-31T23:59:59Z"",
                ""created"": ""2023-01-01T00:00:00Z"",
                ""updated"": ""2023-01-01T12:00:00Z"",
                ""qrCode"": {
                    ""id"": ""qr-123""
                },
                ""receipt"": {
                    ""id"": ""receipt-123""
                },
                ""resources"": {
                    ""share_codes"": []
                }
            }";

            GetSessionResult result = JsonConvert.DeserializeObject<GetSessionResult>(json);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Resources);
            Assert.IsNotNull(result.Resources.ShareCodes);
            Assert.AreEqual(0, result.Resources.ShareCodes.Count);
        }
    }
}
