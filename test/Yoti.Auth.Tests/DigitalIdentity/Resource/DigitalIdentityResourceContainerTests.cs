using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DigitalIdentity.Resource;

namespace Yoti.Auth.Tests.DigitalIdentity.Resource
{
    [TestClass]
    public class DigitalIdentityResourceContainerTests
    {
        [TestMethod]
        public void ShouldDeserializeResourceContainer()
        {
            const string json = @"{
                ""share_codes"": [
                    {
                        ""id"": ""resource-1"",
                        ""source"": ""USER_PROVIDED"",
                        ""created_at"": ""2023-01-01T12:00:00Z"",
                        ""last_updated"": ""2023-01-01T13:00:00Z"",
                        ""tasks"": []
                    },
                    {
                        ""id"": ""resource-2"",
                        ""source"": ""SYSTEM_GENERATED"",
                        ""created_at"": ""2023-01-02T12:00:00Z"",
                        ""last_updated"": ""2023-01-02T13:00:00Z"",
                        ""tasks"": []
                    }
                ]
            }";

            DigitalIdentityResourceContainer result = JsonConvert.DeserializeObject<DigitalIdentityResourceContainer>(json);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ShareCodes);
            Assert.AreEqual(2, result.ShareCodes.Count);
            Assert.AreEqual("resource-1", result.ShareCodes[0].Id);
            Assert.AreEqual("resource-2", result.ShareCodes[1].Id);
        }

        [TestMethod]
        public void ShouldHandleEmptyShareCodes()
        {
            const string json = @"{
                ""share_codes"": []
            }";

            DigitalIdentityResourceContainer result = JsonConvert.DeserializeObject<DigitalIdentityResourceContainer>(json);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ShareCodes);
            Assert.AreEqual(0, result.ShareCodes.Count);
        }

        [TestMethod]
        public void ShouldHandleMissingShareCodes()
        {
            const string json = @"{}";

            DigitalIdentityResourceContainer result = JsonConvert.DeserializeObject<DigitalIdentityResourceContainer>(json);

            Assert.IsNotNull(result);
            Assert.IsNull(result.ShareCodes);
        }
    }
}
