using System;
using System.IO;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Aml;

namespace Yoti.Auth
{
    public class YotiClient
    {
        private readonly string _sdkId;
        private readonly AsymmetricCipherKeyPair _keyPair;
        private readonly YotiClientEngine _yotiClientEngine;
        private readonly string _defaultApiUrl = YotiConstants.DefaultYotiApiUrl;

        /// <summary>
        /// Create a <see cref="YotiClient"/>
        /// </summary>
        /// <param name="sdkId">The client SDK ID provided on the Yoti dashboard.</param>
        /// <param name="privateStreamKey">The private key file provided on the Yoti dashboard as a <see cref="StreamReader"/>.</param>
        public YotiClient(string sdkId, StreamReader privateStreamKey)
        {
            if (string.IsNullOrEmpty(sdkId))
            {
                throw new ArgumentNullException(nameof(sdkId));
            }

            if (privateStreamKey == null)
            {
                throw new ArgumentNullException(nameof(privateStreamKey));
            }

            _sdkId = sdkId;

            try
            {
                _keyPair = CryptoEngine.LoadRsaKey(privateStreamKey);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Could not read private key file: Are you sure it is valid", e);
            }

            _yotiClientEngine = new YotiClientEngine(new HttpRequester());
        }

        /// <summary>
        /// Request an <see cref="ActivityDetails"/>  using the encrypted token provided by yoti during the login process.
        /// </summary>
        /// <param name="encryptedToken">The encrypted returned by Yoti after successfully authenticating.</param>
        /// <returns>The account details of the logged in user as a <see cref="ActivityDetails"/>. </returns>
        public ActivityDetails GetActivityDetails(string encryptedToken)
        {
            return _yotiClientEngine.GetActivityDetails(encryptedToken, _sdkId, _keyPair, _defaultApiUrl);
        }

        /// <summary>
        /// Asynchronously request a <see cref="ActivityDetails"/>  using the encrypted token provided by yoti during the login process.
        /// </summary>
        /// <param name="encryptedToken">The encrypted returned by Yoti after successfully authenticating.</param>
        /// <returns>The account details of the logged in user as a <see cref="ActivityDetails"/>. </returns>
        public async Task<ActivityDetails> GetActivityDetailsAsync(string encryptedToken)
        {
            return await _yotiClientEngine.GetActivityDetailsAsync(encryptedToken, _sdkId, _keyPair, _defaultApiUrl);
        }

        /// <summary>
        /// Request an <see cref="AmlResult"/>  using an individual's name and address.
        /// </summary>
        /// <param name="amlProfile">An individual's name and address.</param>
        /// <returns>The result of the AML check in the form of a <see cref="AmlResult"/>. </returns>
        public AmlResult PerformAmlCheck(IAmlProfile amlProfile)
        {
            return _yotiClientEngine.PerformAmlCheck(_sdkId, _keyPair, _defaultApiUrl, amlProfile);
        }

        /// <summary>
        /// Asynchronously request a <see cref="AmlResult"/>  using an individual's name and address.
        /// </summary>
        /// <param name="amlProfile">An individual's name and address.</param>
        /// <returns>The result of the AML check in the form of a <see cref="AmlResult"/>. </returns>
        public async Task<AmlResult> PerformAmlCheckAsync(IAmlProfile amlProfile)
        {
            return await _yotiClientEngine.PerformAmlCheckAsync(_sdkId, _keyPair, _defaultApiUrl, amlProfile);
        }
    }
}