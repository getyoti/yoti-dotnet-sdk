using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DigitalIdentity;
using Yoti.Auth.DigitalIdentity.Extensions;
using Yoti.Auth.DigitalIdentity.Policy;
using Yoti.Auth.Tests.TestData;

namespace Yoti.Auth.Tests.DigitalIdentity
{
    [TestClass]
    public class ShareSessionRequestBuilderTests
    {
        private const string _someEndpoint = "someEndpoint";

        private readonly BaseExtension extension1 = new ExtensionBuilder<string>()
            .WithContent("content")
            .WithType("string type")
            .Build();

        private readonly BaseExtension extension2 = new LocationConstraintExtensionBuilder()
            .WithLatitude(51.5044772)
            .WithLongitude(-0.082161)
            .WithMaxUncertainty(300)
            .WithRadius(1500)
            .Build();

        [TestMethod]
        public void ShouldBuildADynamicScenario()
        {
            DynamicPolicy somePolicy = TestTools.ShareSession.CreateStandardPolicy();
            object someSubject = IdentityProfiles.CreateStandardSubject();

            ShareSessionRequest result = new ShareSessionRequestBuilder()
                .WithCallbackEndpoint(_someEndpoint)
                .WithPolicy(somePolicy)
                .WithExtension(extension1)
                .WithExtension(extension2)
                .WithSubject(someSubject)
                .Build();

            var expectedExtensions = new List<BaseExtension> { extension1, extension2 };

            Assert.AreEqual(_someEndpoint, result.CallbackEndpoint);
            Assert.AreEqual(somePolicy, result.DynamicPolicy);
            CollectionAssert.AreEqual(expectedExtensions, result.Extensions);
            Assert.AreEqual(someSubject, result.Subject);

            string serializedScenario = JsonConvert.SerializeObject(
                result,

                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            object deserializedObject;
            using (StreamReader r = File.OpenText("TestData/DynamicPolicy.json"))
            {
                string json = r.ReadToEnd();
                deserializedObject = JsonConvert.DeserializeObject(json);
            }

            string expectedJson = JsonConvert.SerializeObject(deserializedObject);

            Assert.AreEqual(expectedJson, serializedScenario);
        }
    }
}