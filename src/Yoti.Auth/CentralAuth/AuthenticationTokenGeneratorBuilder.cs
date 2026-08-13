using System;
using System.Collections.Generic;
using System.IO;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Constants;

namespace Yoti.Auth.CentralAuth
{
    public class AuthenticationTokenGeneratorBuilder
    {
        private string _sdkId;
        private AsymmetricCipherKeyPair _keyPair;
        private readonly List<string> _scopes = new List<string>();
        private string _authApiUrl = Api.DefaultAuthApiUrl;

        public AuthenticationTokenGeneratorBuilder WithSdkId(string sdkId)
        {
            Validation.NotNullOrEmpty(sdkId, nameof(sdkId));
            _sdkId = sdkId;
            return this;
        }

        public AuthenticationTokenGeneratorBuilder WithKey(StreamReader privateKeyStream)
        {
            if (privateKeyStream == null)
                throw new ArgumentNullException(nameof(privateKeyStream));
            _keyPair = CryptoEngine.LoadRsaKey(privateKeyStream);
            return this;
        }

        public AuthenticationTokenGeneratorBuilder WithKey(AsymmetricCipherKeyPair keyPair)
        {
            _keyPair = keyPair ?? throw new ArgumentNullException(nameof(keyPair));
            return this;
        }

        public AuthenticationTokenGeneratorBuilder WithScopes(IEnumerable<string> scopes)
        {
            if (scopes == null)
                throw new ArgumentNullException(nameof(scopes));
            foreach (var scope in scopes)
            {
                Validation.NotNullOrWhiteSpace(scope, nameof(scopes));
                _scopes.Add(scope);
            }
            return this;
        }

        public AuthenticationTokenGeneratorBuilder WithScope(string scope)
        {
            Validation.NotNullOrWhiteSpace(scope, nameof(scope));
            _scopes.Add(scope);
            return this;
        }

        public AuthenticationTokenGeneratorBuilder WithAuthApiUrl(string authApiUrl)
        {
            Validation.NotNullOrEmpty(authApiUrl, nameof(authApiUrl));
            _authApiUrl = authApiUrl;
            return this;
        }

        public AuthenticationTokenGenerator Build()
        {
            Validation.NotNullOrEmpty(_sdkId, nameof(_sdkId));
            if (_keyPair == null)
                throw new InvalidOperationException("A key pair must be provided via WithKey before calling Build().");

            if (_scopes.Count == 0)
                throw new InvalidOperationException("At least one scope must be added via WithScope or WithScopes before calling Build().");

            return new AuthenticationTokenGenerator(_sdkId, _keyPair, _scopes, _authApiUrl);
        }
    }
}
