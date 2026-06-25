using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.CentralAuth;
using Yoti.Auth.Tests.Common;

namespace Yoti.Auth.Tests.CentralAuth
{
    [TestClass]
    public class AuthenticationTokenGeneratorBuilderTests
    {
        private readonly AsymmetricCipherKeyPair _keyPair = KeyPair.Get();
        private const string _sdkId = "test-sdk-id";
        private const string _scope = "identity:create";

        [TestMethod]
        public void WithSdkId_NullShouldThrow()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
                new AuthenticationTokenGeneratorBuilder().WithSdkId(null));
        }

        [TestMethod]
        public void WithSdkId_EmptyShouldThrow()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
                new AuthenticationTokenGeneratorBuilder().WithSdkId(string.Empty));
        }

        [TestMethod]
        public void WithKey_NullKeyPairShouldThrow()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new AuthenticationTokenGeneratorBuilder().WithKey((AsymmetricCipherKeyPair)null));
        }

        [TestMethod]
        public void WithKey_NullStreamReaderShouldThrow()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new AuthenticationTokenGeneratorBuilder().WithKey((StreamReader)null));
        }

        [TestMethod]
        public void WithScopes_NullShouldThrow()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new AuthenticationTokenGeneratorBuilder().WithScopes(null));
        }

        [TestMethod]
        public void WithAuthApiUrl_NullShouldThrow()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
                new AuthenticationTokenGeneratorBuilder().WithAuthApiUrl(null));
        }

        [TestMethod]
        public void WithAuthApiUrl_EmptyShouldThrow()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
                new AuthenticationTokenGeneratorBuilder().WithAuthApiUrl(string.Empty));
        }

        [TestMethod]
        public void Build_MissingSdkIdShouldThrow()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
                new AuthenticationTokenGeneratorBuilder()
                    .WithKey(_keyPair)
                    .WithScope(_scope)
                    .Build());
        }

        [TestMethod]
        public void Build_MissingKeyPairShouldThrow()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
                new AuthenticationTokenGeneratorBuilder()
                    .WithSdkId(_sdkId)
                    .WithScope(_scope)
                    .Build());
        }

        [TestMethod]
        public void Build_EmptyScopesShouldThrow()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
                new AuthenticationTokenGeneratorBuilder()
                    .WithSdkId(_sdkId)
                    .WithKey(_keyPair)
                    .Build());
        }

        [TestMethod]
        public void Build_ValidConfigurationShouldSucceed()
        {
            var generator = new AuthenticationTokenGeneratorBuilder()
                .WithSdkId(_sdkId)
                .WithKey(_keyPair)
                .WithScope(_scope)
                .Build();

            Assert.IsNotNull(generator);
        }

        [TestMethod]
        public void Build_WithMultipleScopesViaWithScopes()
        {
            var generator = new AuthenticationTokenGeneratorBuilder()
                .WithSdkId(_sdkId)
                .WithKey(_keyPair)
                .WithScopes(new List<string> { _scope, "identity:read" })
                .Build();

            Assert.IsNotNull(generator);
        }

        [TestMethod]
        public void Build_WithKeyStreamShouldSucceed()
        {
            using (StreamReader stream = KeyPair.GetValidKeyStream())
            {
                var generator = new AuthenticationTokenGeneratorBuilder()
                    .WithSdkId(_sdkId)
                    .WithKey(stream)
                    .WithScope(_scope)
                    .Build();

                Assert.IsNotNull(generator);
            }
        }

        [TestMethod]
        public void Build_WithCustomAuthApiUrlShouldSucceed()
        {
            var generator = new AuthenticationTokenGeneratorBuilder()
                .WithSdkId(_sdkId)
                .WithKey(_keyPair)
                .WithScope(_scope)
                .WithAuthApiUrl("https://custom.auth.example.com/token")
                .Build();

            Assert.IsNotNull(generator);
        }
    }
}
