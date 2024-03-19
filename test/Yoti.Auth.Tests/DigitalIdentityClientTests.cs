using System;
using System.IO;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Aml;
using Yoti.Auth.Tests.Common;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class DigitalIdentityClientTests
    {
        private const string _someSdkId = "some-sdk-id";
        private readonly Uri _expectedDefaultUri = new Uri(Constants.Api.DefaultYotiShareApiUrl);

        [TestInitialize]
        public void BeforeTests()
        {
            Environment.SetEnvironmentVariable("YOTI_API_URL", null);
        }

        [TestMethod]
        public void NullSdkIdShouldThrowException()
        {
            StreamReader keystream = KeyPair.GetValidKeyStream();
            string sdkId = null;
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                new DigitalIdentityClient(sdkId, keystream);
            });
        }

        [TestMethod]
        public void EmptySdkIdShouldThrowException()
        {
            StreamReader keystream = KeyPair.GetValidKeyStream();
            string sdkId = string.Empty;
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                new DigitalIdentityClient(sdkId, keystream);
            });
        }

        [TestMethod]
        public void NoKeyStreamShouldThrowException()
        {
            StreamReader keystream = null;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new DigitalIdentityClient(_someSdkId, keystream);
            });
        }

        [TestMethod]
        public void InvalidKeyStreamShouldThrowException()
        {
            StreamReader keystream = KeyPair.GetInvalidFormatKeyStream();
            Assert.ThrowsException<FormatException>(() =>
            {
                new DigitalIdentityClient(_someSdkId, keystream);
            });
        }

        [TestMethod]
        public void NullDynamicScenarioShouldThrowException()
        {
            DigitalIdentityClient client = CreateDigitalIdentityClient();

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                client.CreateShareSession(null);
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
        }

        [TestMethod]
        public void EmptyReceiptShouldThrowException()
        {
            DigitalIdentityClient client = CreateDigitalIdentityClient();
            var result =  client.GetShareReceipt("SOME_RECEIPT_ID");
            Assert.IsNotNull(result);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(null)]
        public void ApiUriDefaultIsUsedForNullOrEmpty(string envVar)
        {
            Environment.SetEnvironmentVariable("YOTI_API_URL", envVar);
            DigitalIdentityClient client = CreateDigitalIdentityClient();

            Assert.AreEqual(_expectedDefaultUri, client.ApiUri);
        }

        [TestMethod]
        public void ApiUriOverriddenOverEnvVariable()
        {
            Uri overriddenApiUri = new Uri("https://overridden.com");
            Environment.SetEnvironmentVariable("YOTI_API_URL", "https://envapiuri.com");
            DigitalIdentityClient client = CreateDigitalIdentityClient();
            client.OverrideApiUri(overriddenApiUri);

            Assert.AreEqual(overriddenApiUri, client.ApiUri);
        }

        [TestMethod]
        public void ApiUriEnvVariableIsUsed()
        {
            Environment.SetEnvironmentVariable("YOTI_API_URL", "https://envapiuri.com");
            DigitalIdentityClient client = CreateDigitalIdentityClient();

            Uri expectedApiUri = new Uri("https://envapiuri.com");
            Assert.AreEqual(expectedApiUri, client.ApiUri);
        }
        private static DigitalIdentityClient CreateDigitalIdentityClient()
        {
            StreamReader privateStreamKey = KeyPair.GetValidKeyStream();

            return new DigitalIdentityClient(_someSdkId, privateStreamKey);
        }

        [TestMethod]
        public void ApiUriSetForPrivateKeyInitialisationHttpClient()
        {
            AsymmetricCipherKeyPair keyPair = KeyPair.Get();

            DigitalIdentityClient yotiClient = new DigitalIdentityClient(new HttpClient(), _someSdkId, keyPair);

            Assert.AreEqual(_expectedDefaultUri, yotiClient.ApiUri);
        }

        [TestMethod]
        public void ApiUriSetForStreamInitialisation()
        {
            StreamReader privateStreamKey = KeyPair.GetValidKeyStream();

            DigitalIdentityClient yotiClient = new DigitalIdentityClient(_someSdkId, privateStreamKey);

            Assert.AreEqual(_expectedDefaultUri, yotiClient.ApiUri);
        }  

        [TestMethod]
        public void ApiUriSetForStreamInitialisationHttpClient()
        {
            StreamReader privateStreamKey = KeyPair.GetValidKeyStream();

            DigitalIdentityClient yotiClient = new DigitalIdentityClient(new HttpClient(), _someSdkId, privateStreamKey);

            Assert.AreEqual(_expectedDefaultUri, yotiClient.ApiUri);
        }
    }
}