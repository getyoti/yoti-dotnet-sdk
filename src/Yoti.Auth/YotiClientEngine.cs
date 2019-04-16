﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Aml;

namespace Yoti.Auth
{
    internal class YotiClientEngine
    {
        private readonly IHttpRequester _httpRequester;
        private readonly HttpClient _httpClient;

        public YotiClientEngine(IHttpRequester httpRequester, HttpClient httpClient)
        {
            _httpRequester = httpRequester;
            _httpClient = httpClient;

#if !NETSTANDARD1_6
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
#endif
        }

        public ActivityDetails GetActivityDetails(string encryptedToken, string sdkId, AsymmetricCipherKeyPair keyPair, string apiUrl)
        {
            Task<ActivityDetails> task = Task.Run<ActivityDetails>(async () => await GetActivityDetailsAsync(encryptedToken, sdkId, keyPair, apiUrl).ConfigureAwait(false));

            return task.Result;
        }

        public async Task<ActivityDetails> GetActivityDetailsAsync(string encryptedConnectToken, string sdkId, AsymmetricCipherKeyPair keyPair, string apiUrl)
        {
            string token = CryptoEngine.DecryptToken(encryptedConnectToken, keyPair);
            const string path = "profile";
            byte[] httpContent = null;
            HttpMethod httpMethod = HttpMethod.Get;

            string endpoint = EndpointFactory.CreateProfileEndpoint(path, token, sdkId);

            Dictionary<string, string> headers = CreateHeaders(keyPair, httpMethod, endpoint, httpContent: null);

            Response response = await _httpRequester.DoRequest(
                _httpClient,
                HttpMethod.Get,
                new Uri(
                    apiUrl + endpoint),
                headers,
                httpContent).ConfigureAwait(false);

            return ProfileParser.HandleResponse(keyPair, response);
        }

        public AmlResult PerformAmlCheck(string appId, AsymmetricCipherKeyPair keyPair, string apiUrl, IAmlProfile amlProfile)
        {
            Task<AmlResult> task = Task.Run(async () => await PerformAmlCheckAsync(appId, keyPair, apiUrl, amlProfile).ConfigureAwait(true));

            return task.Result;
        }

        public Task<AmlResult> PerformAmlCheckAsync(string appId, AsymmetricCipherKeyPair keyPair, string apiUrl, IAmlProfile amlProfile)
        {
            if (apiUrl == null)
            {
                throw new ArgumentNullException(nameof(apiUrl));
            }

            if (amlProfile == null)
            {
                throw new ArgumentNullException(nameof(amlProfile));
            }

            return PerformAmlCheckInternalAsync(appId, keyPair, apiUrl, amlProfile);
        }

        private async Task<AmlResult> PerformAmlCheckInternalAsync(string appId, AsymmetricCipherKeyPair keyPair, string apiUrl, IAmlProfile amlProfile)
        {
            string serializedProfile = Newtonsoft.Json.JsonConvert.SerializeObject(amlProfile);
            byte[] httpContent = System.Text.Encoding.UTF8.GetBytes(serializedProfile);

            HttpMethod httpMethod = HttpMethod.Post;

            string endpoint = EndpointFactory.CreateAmlEndpoint(appId);

            Dictionary<string, string> headers = CreateHeaders(keyPair, httpMethod, endpoint, httpContent);

            AmlResult result = await Task.Run(async () => await new RemoteAmlService()
                .PerformCheck(
                _httpClient, _httpRequester, headers, apiUrl, endpoint, httpContent).ConfigureAwait(false))
                .ConfigureAwait(false);

            return result;
        }

        private static Dictionary<string, string> CreateHeaders(AsymmetricCipherKeyPair keyPair, HttpMethod httpMethod, string endpoint, byte[] httpContent)
        {
            string authKey = CryptoEngine.GetAuthKey(keyPair);
            string authDigest = SignedMessageFactory.SignMessage(httpMethod, endpoint, keyPair, httpContent);

            if (string.IsNullOrEmpty(authDigest))
                throw new InvalidOperationException("Could not sign request");

            return HeadersFactory.Create(authDigest, authKey);
        }
    }
}