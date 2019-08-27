using Org.BouncyCastle.Crypto;

namespace Yoti.Auth.Sandbox
{
    public class YotiSandboxClientBuilder
    {
        private string _appId;
        private AsymmetricCipherKeyPair _keyPair;

        public YotiSandboxClientBuilder()
        {
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

            return new YotiSandboxClient(_appId, _keyPair);
        }
    }
}