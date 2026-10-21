using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yoti.Auth.DocScan.Session.Create;

namespace Yoti.Auth.Tests.DocScan.Session.Create
{
    [TestClass]
    public class SessionOverviewPdfRequestTests
    {
        [TestMethod]
        public void ShouldWrapPayloadInOptionsEnvelope()
        {
            var payload = new SessionOverviewPdfPayload(includeImages: false, includeExtractedText: false, includeDeviceTracking: false);
            var request = new SessionOverviewPdfRequest(payload);

            JObject json = JObject.Parse(JsonConvert.SerializeObject(request));

            Assert.AreEqual(1, json.Count);
            Assert.IsTrue(json.ContainsKey("options"));

            JObject options = (JObject)json["options"];
            Assert.AreEqual(3, options.Count);
            Assert.AreEqual(false, options["include_images"].Value<bool>());
            Assert.AreEqual(false, options["include_extracted_text"].Value<bool>());
            Assert.AreEqual(false, options["include_device_tracking"].Value<bool>());
        }

        [TestMethod]
        public void ShouldSerializeToEmptyOptionsObjectWhenNoOptionsProvided()
        {
            var request = new SessionOverviewPdfRequest(new SessionOverviewPdfPayload());

            string json = JsonConvert.SerializeObject(request);

            Assert.AreEqual("{\"options\":{}}", json);
        }
    }
}
