using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.Aml;
using Yoti.Auth.Tests.Common;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class YotiClientTests
    {
        [TestMethod]
        public void YotiClient_NullSdkId_ThrowsException()
        {
            StreamReader keystream = KeyPair.GetValidKeyStream();
            string sdkId = null;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new YotiClient(sdkId, keystream);
            });
        }

        [TestMethod]
        public void YotiClient_EmptySdkId_ThrowsException()
        {
            StreamReader keystream = KeyPair.GetValidKeyStream();
            string sdkId = string.Empty;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new YotiClient(sdkId, keystream);
            });
        }

        [TestMethod]
        public void YotiClient_NoKeyStream_ThrowsException()
        {
            StreamReader keystream = null;
            string sdkId = "fake-sdk-id";
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new YotiClient(sdkId, keystream);
            });
        }

        [TestMethod]
        public void YotiClient_InvalidKeyStream_ThrowsException()
        {
            StreamReader keystream = KeyPair.GetInvalidFormatKeyStream();
            const string sdkId = "fake-sdk-id";
            Assert.ThrowsException<FormatException>(() =>
            {
                new YotiClient(sdkId, keystream);
            });
        }

        [TestMethod]
        public void YotiClient_PerformAmlCheck_NullAmlProfile_ThrowsException()
        {
            YotiClient client = CreateYotiClient();

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                client.PerformAmlCheck(amlProfile: null);
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
        }

        [TestMethod]
        public void YotiClient_PerformAmlCheck_NullAmlAddress_ThrowsException()
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
        public void YotiClient_PerformAmlCheck_NullGivenName_ThrowsException()
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
        public void YotiClient_PerformAmlCheck_NullFamilyName_ThrowsException()
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
        public void YotiClient_PerformAmlCheck_NullCountry_ThrowsException()
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
        public void CreateShareUrl_NullDynamicScenario_ThrowsException()
        {
            YotiClient client = CreateYotiClient();

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                client.CreateShareUrl(null);
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
        }

        private static YotiClient CreateYotiClient()
        {
            const string sdkId = "fake-sdk-id";
            StreamReader privateStreamKey = KeyPair.GetValidKeyStream();

            return new YotiClient(sdkId, privateStreamKey);
        }
    }
}