using AttrpubapiV1;
using CompubapiV1;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Yoti.Auth.DataObjects;

namespace Yoti.Auth
{
    public class YotiClient
    {
        private const string _apiUrl = @"https://api.yoti.com/api/v1"; // TODO: Make this configurable

        private readonly string _sdkId = null;
        private readonly AsymmetricCipherKeyPair _keyPair = null;
        private readonly YotiClientEngine _yotiClientEngine = null;
        
        /// <summary>
        /// Create a <see cref="YotiClient"/> 
        /// </summary>
        /// <param name="sdkId">The client SDK ID provided on the Yoti dashboard.</param>
        /// <param name="key">The private key file provided on the Yoti dashboard as a <see cref="StreamReader"/>.</param>
        public YotiClient(string sdkId, StreamReader privateStreamKey)
        {
            if (string.IsNullOrEmpty(sdkId))
            {
                throw new ArgumentNullException("sdkId");
            }

            if (privateStreamKey == null)
            {
                throw new ArgumentNullException("privateStreamKey");
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
            return _yotiClientEngine.GetActivityDetails(encryptedToken, _sdkId, _keyPair);
        }
        

        /// <summary>
        /// Asynchronously request a <see cref="ActivityDetails"/>  using the encrypted token provided by yoti during the login process.
        /// </summary>
        /// <param name="encryptedToken">The encrypted returned by Yoti after successfully authenticating.</param>
        /// <returns>The account details of the logged in user as a <see cref="ActivityDetails"/>. </returns>
        public async Task<ActivityDetails> GetActivityDetailsAsync(string encryptedToken)
        {
            return await _yotiClientEngine.GetActivityDetailsAsync(encryptedToken, _sdkId, _keyPair);
        }
    }
}