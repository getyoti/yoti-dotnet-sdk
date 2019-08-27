using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Sandbox.Profile.Request;
using Yoti.Auth.Sandbox.Profile.Response;
using Yoti.Auth.Web;

namespace Yoti.Auth.Sandbox
{
    public class YotiSandboxClient
    {
        private const string _yotiSandboxPathPrefix = "/sandbox/v1";
        private readonly string _defaultSandboxApiUrl = Constants.Web.DefaultYotiHost + _yotiSandboxPathPrefix;

        private readonly string _appId;
        private readonly AsymmetricCipherKeyPair _keyPair;

        public static YotiSandboxClientBuilder Builder()
        {
            return new YotiSandboxClientBuilder();
        }

        public YotiSandboxClient(string appId, AsymmetricCipherKeyPair keyPair)
        {
            _appId = appId;
            _keyPair = keyPair;
        }

        internal string SetupSharingProfile(HttpClient httpClient, IHttpRequester httpRequester, YotiTokenRequest yotiTokenRequest)
        {
            string endpoint = SandboxPathFactory.CreateSandboxPath(_appId);
            HttpMethod httpMethod = HttpMethod.Post;

            try
            {
                string serializedTokenRequest = Newtonsoft.Json.JsonConvert.SerializeObject(yotiTokenRequest);
                byte[] body = Encoding.UTF8.GetBytes(serializedTokenRequest);

                Dictionary<string, string> headers = HeadersFactory.Create(_keyPair, HttpMethod.Post, endpoint, body);

                Response response = httpRequester.DoRequest(
                    httpClient,
                    httpMethod,
                    new Uri(_defaultSandboxApiUrl + endpoint),
                    headers,
                    body).Result;

                if (!response.Success)
                {
                    Response.CreateExceptionFromStatusCode<SandboxException>(response);
                }

                YotiTokenResponse yotiTokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<YotiTokenResponse>(response.Content);

                return yotiTokenResponse.Token;
            }
            catch (Exception ex)
            {
                throw new SandboxException(Properties.Resources.SharingProfileError, ex);
            }
        }
    }
}