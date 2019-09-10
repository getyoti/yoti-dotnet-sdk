﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Aml;
using Yoti.Auth.ShareUrl;
using Yoti.Auth.Web;

namespace Yoti.Auth
{
    public class YotiClient
    {
        private readonly string _sdkId;
        private readonly AsymmetricCipherKeyPair _keyPair;
        private readonly YotiClientEngine _yotiClientEngine;
        private readonly Uri _defaultApiUrl = new Uri(Constants.Web.DefaultYotiApiUrl);

        /// <summary>
        /// Create a <see cref="YotiClient"/>
        /// </summary>
        /// <param name="sdkId">The client SDK ID provided on the Yoti dashboard.</param>
        /// <param name="privateStreamKey">
        /// The private key file provided on the Yoti dashboard as a <see cref="StreamReader"/>.
        /// </param>
        public YotiClient(string sdkId, StreamReader privateStreamKey) : this(new HttpClient(), sdkId, privateStreamKey)
        {
        }

        /// <summary>
        /// Create a <see cref="YotiClient"/> with a specified <see cref="HttpClient"/>
        /// </summary>
        /// <param name="httpClient">Allows the specification of a HttpClient</param>
        /// <param name="sdkId">The client SDK ID provided on the Yoti dashboard.</param>
        /// <param name="privateStreamKey">
        /// The private key file provided on the Yoti dashboard as a <see cref="StreamReader"/>.
        /// </param>
        public YotiClient(HttpClient httpClient, string sdkId, StreamReader privateStreamKey)
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
            _keyPair = CryptoEngine.LoadRsaKey(privateStreamKey);

            _yotiClientEngine = new YotiClientEngine(httpClient);
        }

        /// <summary>
        /// Request an <see cref="ActivityDetails"/> using the encrypted token provided by yoti
        /// during the login process.
        /// </summary>
        /// <param name="encryptedToken">The encrypted returned by Yoti after successfully authenticating.</param>
        /// <returns>The account details of the logged in user as a <see cref="ActivityDetails"/>.</returns>
        public ActivityDetails GetActivityDetails(string encryptedToken, Uri apiUrl = null)
        {
            Task<ActivityDetails> task = Task.Run(async () => await GetActivityDetailsAsync(encryptedToken, apiUrl).ConfigureAwait(false));

            return task.Result;
        }

        /// <summary>
        /// Asynchronously request a <see cref="ActivityDetails"/> using the encrypted token provided
        /// by yoti during the login process.
        /// </summary>
        /// <param name="encryptedToken">The encrypted returned by Yoti after successfully authenticating.</param>
        /// <returns>The account details of the logged in user as a <see cref="ActivityDetails"/>.</returns>
        public async Task<ActivityDetails> GetActivityDetailsAsync(string encryptedToken, Uri apiUrl = null)
        {
            if (apiUrl == null)
                apiUrl = _defaultApiUrl;

            return await _yotiClientEngine.GetActivityDetailsAsync(encryptedToken, _sdkId, _keyPair, apiUrl).ConfigureAwait(false);
        }

        /// <summary>
        /// Request an <see cref="AmlResult"/> using an individual's name and address.
        /// </summary>
        /// <param name="amlProfile">An individual's name and address.</param>
        /// <returns>The result of the AML check in the form of a <see cref="AmlResult"/>.</returns>
        public AmlResult PerformAmlCheck(IAmlProfile amlProfile)
        {
            Task<AmlResult> task = Task.Run(async () => await PerformAmlCheckAsync(amlProfile).ConfigureAwait(true));

            return task.Result;
        }

        /// <summary>
        /// Asynchronously request a <see cref="AmlResult"/> using an individual's name and address.
        /// </summary>
        /// <param name="amlProfile">An individual's name and address.</param>
        /// <returns>The result of the AML check in the form of a <see cref="AmlResult"/>.</returns>
        public async Task<AmlResult> PerformAmlCheckAsync(IAmlProfile amlProfile)
        {
            return await _yotiClientEngine.PerformAmlCheckAsync(_sdkId, _keyPair, _defaultApiUrl, amlProfile).ConfigureAwait(false);
        }

        /// <summary>
        /// Initiate a sharing process based on a <see cref="DynamicScenario"/>.
        /// </summary>
        /// <param name="dynamicScenario">
        /// Details of the device's callback endpoint, <see
        /// cref="Yoti.Auth.ShareUrl.Policy.DynamicPolicy"/> and extensions for the application
        /// </param>
        /// <returns><see cref="ShareUrlResult"/> containing a Sharing URL and Reference ID</returns>
        public ShareUrlResult CreateShareUrl(DynamicScenario dynamicScenario)
        {
            Task<ShareUrlResult> task = Task.Run(async () => await CreateShareUrlAsync(dynamicScenario).ConfigureAwait(true));

            return task.Result;
        }

        /// <summary>
        /// Asynchronously initiate a sharing process based on a <see cref="DynamicScenario"/>.
        /// </summary>
        /// <param name="dynamicScenario">
        /// Details of the device's callback endpoint, <see
        /// cref="Yoti.Auth.ShareUrl.Policy.DynamicPolicy"/> and extensions for the application
        /// </param>
        /// <returns><see cref="ShareUrlResult"/> containing a Sharing URL and Reference ID</returns>
        public async Task<ShareUrlResult> CreateShareUrlAsync(DynamicScenario dynamicScenario)
        {
            return await _yotiClientEngine.CreateShareURLAsync(_sdkId, _keyPair, _defaultApiUrl, dynamicScenario).ConfigureAwait(false);
        }
    }
}