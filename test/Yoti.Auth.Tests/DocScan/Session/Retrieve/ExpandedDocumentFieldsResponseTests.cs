using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve;

namespace Yoti.Auth.Tests.DocScan.Session.Retrieve
{
    [TestClass]
    public class ExpandedDocumentFieldsResponseTests
    {
        [TestMethod]
        public void ShouldDeserializeMedia()
        {
            const string json = @"{
                ""media"": {
                    ""id"": ""media-id-123"",
                    ""type"": ""IMAGE""
                }
            }";

            var result = JsonConvert.DeserializeObject<ExpandedDocumentFieldsResponse>(json);

            Assert.IsNotNull(result.Media);
            Assert.AreEqual("media-id-123", result.Media.Id);
            Assert.AreEqual("IMAGE", result.Media.Type);
        }
    }
}
