using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.IO;
using Yoti.Auth.Aml;
using Yoti.Auth.Tests.Common;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class YotiClientTests
    {
        [TestMethod]
        public void NullSdkIdShouldThrowException()
        {
            StreamReader keystream = KeyPair.GetValidKeyStream();
            string sdkId = null;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new YotiClient(sdkId, keystream);
            });
        }

        [TestMethod]
        public void EmptySdkIdShouldThrowException()
        {
            StreamReader keystream = KeyPair.GetValidKeyStream();
            string sdkId = string.Empty;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new YotiClient(sdkId, keystream);
            });
        }

        [TestMethod]
        public void NoKeyStreamShouldThrowException()
        {
            StreamReader keystream = null;
            string sdkId = "fake-sdk-id";
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new YotiClient(sdkId, keystream);
            });
        }

        [TestMethod]
        public void InvalidKeyStreamShouldThrowException()
        {
            StreamReader keystream = KeyPair.GetInvalidFormatKeyStream();
            const string sdkId = "fake-sdk-id";
            Assert.ThrowsException<FormatException>(() =>
            {
                new YotiClient(sdkId, keystream);
            });
        }

        [TestMethod]
        public void NullAmlProfileShouldThrowException()
        {
            YotiClient client = CreateYotiClient();

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                client.PerformAmlCheck(amlProfile: null);
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
        }

        [TestMethod]
        public void NullAmlAddressShouldThrowException()
        {
            YotiClient client = CreateYotiClient();

            var amlProfile = new AmlProfile(
                           givenNames: "Edward Richard George",
                           familyName: "Heath",
                           amlAddress: null);

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                client.PerformAmlCheck(amlProfile: amlProfile);
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<JsonSerializationException>(aggregateException));
        }

        [TestMethod]
        public void NullAmlGivenNameShouldThrowException()
        {
            YotiClient client = CreateYotiClient();

            var amlProfile = new AmlProfile(
                givenNames: null,
                familyName: "Heath",
                amlAddress: TestTools.Aml.CreateStandardAmlAddress());

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                client.PerformAmlCheck(amlProfile: amlProfile);
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<JsonSerializationException>(aggregateException));
        }

        [TestMethod]
        public void NullAmlFamilyNameShouldThrowException()
        {
            YotiClient client = CreateYotiClient();

            var amlProfile = new AmlProfile(
                givenNames: "Edward Richard George",
                familyName: null,
                amlAddress: TestTools.Aml.CreateStandardAmlAddress());

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                client.PerformAmlCheck(amlProfile: amlProfile);
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<JsonSerializationException>(aggregateException));
        }

        [TestMethod]
        public void NullAmlCountryShouldThrowException()
        {
            YotiClient client = CreateYotiClient();

            var amlAddress = new AmlAddress(
               country: null);

            var amlProfile = new AmlProfile(
                givenNames: "Edward Richard George",
                familyName: "Heath",
                amlAddress: amlAddress);

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                client.PerformAmlCheck(amlProfile: amlProfile);
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<JsonSerializationException>(aggregateException));
        }

        [TestMethod]
        public void NullDynamicScenarioShouldThrowException()
        {
            YotiClient client = CreateYotiClient();

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                client.CreateShareUrl(null);
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(null)]
        public void ApiUriDefaultIsUsedForNullOrEmpty(string envVar)
        {
            Environment.SetEnvironmentVariable("YOTI_API_URL", envVar);
            YotiClient client = CreateYotiClient();
            Uri expectedDefaultUri = new Uri(Constants.Api.DefaultYotiApiUrl);

            Assert.AreEqual(expectedDefaultUri, client.ApiUri);
        }

        [TestMethod]
        public void ApiUriOverriddenOverEnvVariable()
        {
            Uri overriddenApiUri = new Uri("https://overridden.com");
            Environment.SetEnvironmentVariable("YOTI_API_URL", "https://envapiuri.com");
            YotiClient client = CreateYotiClient();
            client.OverrideApiUri(overriddenApiUri);

            Assert.AreEqual(overriddenApiUri, client.ApiUri);
        }

        [TestMethod]
        public void ApiUriEnvVariableIsUsed()
        {
            Environment.SetEnvironmentVariable("YOTI_API_URL", "https://envapiuri.com");
            YotiClient client = CreateYotiClient();

            Uri expectedApiUri = new Uri("https://envapiuri.com");
            Assert.AreEqual(expectedApiUri, client.ApiUri);
        }

        private static YotiClient CreateYotiClient()
        {
            const string sdkId = "fake-sdk-id";
            StreamReader privateStreamKey = KeyPair.GetValidKeyStream();

            return new YotiClient(sdkId, privateStreamKey);
        }
    }
}