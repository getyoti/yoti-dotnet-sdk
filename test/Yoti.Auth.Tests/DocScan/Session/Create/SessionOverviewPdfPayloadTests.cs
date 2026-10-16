using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yoti.Auth.DocScan.Session.Create;

namespace Yoti.Auth.Tests.DocScan.Session.Create
{
    [TestClass]
    public class SessionOverviewPdfPayloadTests
    {
        [TestMethod]
        public void ShouldSerializeAllOptionsWhenProvided()
        {
            var payload = new SessionOverviewPdfPayload(includeImages: true, includeExtractedText: false, includeDeviceTracking: true);

            JObject json = JObject.Parse(JsonConvert.SerializeObject(payload));

            Assert.AreEqual(3, json.Count);
            Assert.AreEqual(true, json["include_images"].Value<bool>());
            Assert.AreEqual(false, json["include_extracted_text"].Value<bool>());
            Assert.AreEqual(true, json["include_device_tracking"].Value<bool>());
        }

        [TestMethod]
        public void ShouldOmitUnsetOptionsFromSerializedBody()
        {
            var payload = new SessionOverviewPdfPayload(includeImages: true);

            JObject json = JObject.Parse(JsonConvert.SerializeObject(payload));

            Assert.AreEqual(1, json.Count);
            Assert.AreEqual(true, json["include_images"].Value<bool>());
            Assert.IsFalse(json.ContainsKey("include_extracted_text"));
            Assert.IsFalse(json.ContainsKey("include_device_tracking"));
        }

        [TestMethod]
        public void ShouldSerializeToEmptyObjectWhenNoOptionsProvided()
        {
            var payload = new SessionOverviewPdfPayload();

            string json = JsonConvert.SerializeObject(payload);

            Assert.AreEqual("{}", json);
        }
    }
}
