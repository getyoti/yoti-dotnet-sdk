using System;
using System.Net.Http;
using Org.BouncyCastle.Crypto;

namespace Yoti.Auth.Sandbox
{
    public class YotiSandboxClientBuilder
    {
        private string _appId;
        private AsymmetricCipherKeyPair _keyPair;
        private Uri _apiUri;

        public YotiSandboxClientBuilder()
        {
        }

        public YotiSandboxClientBuilder WithApiUri(Uri apiUri)
        {
            _apiUri = apiUri;

            return this;
        }

        public YotiSandboxClientBuilder ForApplication(string appId)
        {
            _appId = appId;
            return this;
        }

        public YotiSandboxClientBuilder WithKeyPair(AsymmetricCipherKeyPair keypair)
        {
            _keyPair = keypair;
            return this;
        }

        public YotiSandboxClient Build()
        {
            Validation.NotNull(_appId, nameof(_appId));
            Validation.NotNull(_keyPair, nameof(_keyPair));
            Validation.NotNull(_apiUri, nameof(_apiUri));

            return new YotiSandboxClient(new HttpClient(), _apiUri, _appId, _keyPair);
        }
    }
}