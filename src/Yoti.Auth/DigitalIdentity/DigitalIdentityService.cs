using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Attribute;
using Yoti.Auth.Exceptions;
using Yoti.Auth.Profile;
using Yoti.Auth.Web;
using Yoti.Auth.ProtoBuf.Attribute;
using Yoti.Auth.Share;
using ApplicationProfile = Yoti.Auth.Profile.ApplicationProfile;

namespace Yoti.Auth.DigitalIdentity
{
    public static class DigitalIdentityService
    {
        private const string receiptRetrieval = "/v2/receipts/{0}";
        private const string receiptKeyRetrieval = "/v2/wrapped-item-keys/{0}";
        private const string sessionCreation = "/v2/sessions";
        private const string yotiAuthId = "X-Yoti-Auth-Id";

        internal static async Task<ShareSessionResult> CreateShareSession(HttpClient httpClient, Uri apiUrl, IAuthStrategy authStrategy, ShareSessionRequest shareSessionRequestPayload)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(authStrategy, nameof(authStrategy));
            Validation.NotNull(shareSessionRequestPayload, nameof(shareSessionRequestPayload));

            string serializedScenario = JsonConvert.SerializeObject(
                shareSessionRequestPayload,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            byte[] body = Encoding.UTF8.GetBytes(serializedScenario);

            var builder = new RequestBuilder()
                .WithAuthStrategy(authStrategy)
                .WithBaseUri(apiUrl)
                .WithEndpoint(sessionCreation)
                .WithHttpMethod(HttpMethod.Post)
                .WithContent(body);

            if (authStrategy.SdkId != null)
            {
                builder = builder
                    .WithHeader(yotiAuthId, authStrategy.SdkId)
                    .WithQueryParam("sdkID", authStrategy.SdkId);
            }

            Request shareSessionRequest = builder.Build();

            using (HttpResponseMessage response = await shareSessionRequest.Execute(httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    Response.CreateYotiExceptionFromStatusCode<DigitalIdentityException>(response);

                var responseObject = await response.Content.ReadAsStringAsync();
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ShareSessionResult>(responseObject));
            }
        }

        internal static async Task<GetSessionResult> GetSession(HttpClient httpClient, Uri apiUrl, IAuthStrategy authStrategy, string sessionId)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(authStrategy, nameof(authStrategy));
            Validation.NotNull(sessionId, nameof(sessionId));

            var builder = new RequestBuilder()
                .WithAuthStrategy(authStrategy)
                .WithBaseUri(apiUrl)
                .WithEndpoint(string.Format("{0}/{1}", sessionCreation, sessionId))
                .WithHttpMethod(HttpMethod.Get);

            if (authStrategy.SdkId != null)
            {
                builder = builder
                    .WithHeader(yotiAuthId, authStrategy.SdkId)
                    .WithQueryParam("appId", authStrategy.SdkId);
            }

            Request getSessionRequest = builder.Build();

            using (HttpResponseMessage response = await getSessionRequest.Execute(httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    Response.CreateYotiExceptionFromStatusCode<DigitalIdentityException>(response);

                var responseObject = await response.Content.ReadAsStringAsync();
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<GetSessionResult>(responseObject));
            }
        }

        internal static async Task<CreateQrResult> CreateQrCode(HttpClient httpClient, Uri apiUrl, IAuthStrategy authStrategy, string sessionId, QrRequest qrRequestPayload)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(authStrategy, nameof(authStrategy));
            Validation.NotNullOrEmpty(sessionId, nameof(sessionId));
            Validation.NotNull(qrRequestPayload, nameof(qrRequestPayload));

            string serializedQrCode = JsonConvert.SerializeObject(
                qrRequestPayload,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            byte[] body = Encoding.UTF8.GetBytes(serializedQrCode);

            var builder = new RequestBuilder()
                .WithAuthStrategy(authStrategy)
                .WithBaseUri(apiUrl)
                .WithEndpoint(string.Format("/v2/sessions/{0}/qr-codes", sessionId))
                .WithHttpMethod(HttpMethod.Post)
                .WithContent(body);

            if (authStrategy.SdkId != null)
            {
                builder = builder
                    .WithHeader(yotiAuthId, authStrategy.SdkId)
                    .WithQueryParam("appId", authStrategy.SdkId);
            }

            Request createQrRequest = builder.Build();

            using (HttpResponseMessage response = await createQrRequest.Execute(httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    Response.CreateYotiExceptionFromStatusCode<DigitalIdentityException>(response);

                var responseObject = await response.Content.ReadAsStringAsync();
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<CreateQrResult>(responseObject));
            }
        }

        internal static async Task<GetQrCodeResult> GetQrCode(HttpClient httpClient, Uri apiUrl, IAuthStrategy authStrategy, string qrCodeId)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(authStrategy, nameof(authStrategy));
            Validation.NotNull(qrCodeId, nameof(qrCodeId));

            var builder = new RequestBuilder()
                .WithAuthStrategy(authStrategy)
                .WithBaseUri(apiUrl)
                .WithEndpoint(string.Format("/v2/qr-codes/{0}", qrCodeId))
                .WithHttpMethod(HttpMethod.Get);

            if (authStrategy.SdkId != null)
            {
                builder = builder
                    .WithHeader(yotiAuthId, authStrategy.SdkId)
                    .WithQueryParam("appId", authStrategy.SdkId);
            }

            Request qrCodeRequest = builder.Build();

            using (HttpResponseMessage response = await qrCodeRequest.Execute(httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    Response.CreateYotiExceptionFromStatusCode<DigitalIdentityException>(response);

                var responseObject = await response.Content.ReadAsStringAsync();
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<GetQrCodeResult>(responseObject));
            }
        }

        public static async Task<SharedReceiptResponse> GetShareReceipt(HttpClient httpClient, Uri apiUrl, IAuthStrategy authStrategy, string receiptId)
        {
            Validation.NotNullOrEmpty(receiptId, nameof(receiptId));

            var keyPair = (authStrategy as SignedRequestAuthStrategy)?.KeyPair;
            if (keyPair == null)
                throw new InvalidOperationException("GetShareReceipt requires a signed-request strategy with a private key for receipt decryption.");

            try
            {
                var receiptResponse = await GetReceipt(httpClient, receiptId, apiUrl, authStrategy);
                var itemKeyId = receiptResponse.WrappedItemKeyId;

                var encryptedItemKeyResponse = await GetReceiptItemKey(httpClient, itemKeyId, apiUrl, authStrategy);

                var receiptContentKey = CryptoEngine.UnwrapReceiptKey(receiptResponse.WrappedKey, encryptedItemKeyResponse.Value, encryptedItemKeyResponse.Iv, keyPair);

                var (attrData, aextra, decryptAttrDataError) = DecryptReceiptContent(receiptResponse.Content, receiptContentKey);
                if (decryptAttrDataError != null)
                    throw new Exception($"An unexpected error occurred: {decryptAttrDataError.Message}");

                var parsedAttributesApp = AttributeConverter.ConvertToBaseAttributes(attrData);
                var appProfile = new ApplicationProfile(parsedAttributesApp);

                var (attrOtherData, aOtherExtra, decryptOtherAttrDataError) = DecryptReceiptContent(receiptResponse.OtherPartyContent, receiptContentKey);
                if (decryptOtherAttrDataError != null)
                    throw new Exception($"An unexpected error occurred: {decryptOtherAttrDataError.Message}");

                var userProfile = new YotiProfile();
                if (attrOtherData != null)
                {
                    var parsedAttributesUser = AttributeConverter.ConvertToBaseAttributes(attrOtherData);
                    userProfile = new YotiProfile(parsedAttributesUser);
                }

                ExtraData userExtraData = new ExtraData();
                if (aOtherExtra != null)
                    userExtraData = ExtraDataConverter.ParseExtraDataProto(aOtherExtra);

                ExtraData appExtraData = new ExtraData();
                if (aextra != null)
                    appExtraData = ExtraDataConverter.ParseExtraDataProto(aextra);

                return new SharedReceiptResponse
                {
                    ID = receiptResponse.ID,
                    SessionID = receiptResponse.SessionID,
                    RememberMeID = receiptResponse.RememberMeID,
                    ParentRememberMeID = receiptResponse.ParentRememberMeID,
                    Timestamp = receiptResponse.Timestamp,
                    UserContent = new UserContent
                    {
                        UserProfile = userProfile,
                        ExtraData = userExtraData
                    },
                    ApplicationContent = new ApplicationContent
                    {
                        ApplicationProfile = appProfile,
                        ExtraData = appExtraData
                    },
                    Error = receiptResponse.Error,
                    ErrorDetails = receiptResponse.ErrorDetails
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }

        private static async Task<ReceiptResponse> GetReceipt(HttpClient httpClient, string receiptId, Uri apiUrl, IAuthStrategy authStrategy)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(authStrategy, nameof(authStrategy));

            string receiptUrl = Base64ToBase64URL(receiptId);
            string endpoint = string.Format(receiptRetrieval, receiptUrl);

            var builder = new RequestBuilder()
                .WithAuthStrategy(authStrategy)
                .WithBaseUri(apiUrl)
                .WithEndpoint(endpoint)
                .WithHttpMethod(HttpMethod.Get);

            if (authStrategy.SdkId != null)
            {
                builder = builder
                    .WithHeader(yotiAuthId, authStrategy.SdkId)
                    .WithQueryParam("sdkID", authStrategy.SdkId);
            }

            Request receiptRequest = builder.Build();

            using (HttpResponseMessage response = await receiptRequest.Execute(httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    Response.CreateYotiExceptionFromStatusCode<DigitalIdentityException>(response);

                var responseObject = await response.Content.ReadAsStringAsync();
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ReceiptResponse>(responseObject));
            }
        }

        private static async Task<ReceiptItemKeyResponse> GetReceiptItemKey(HttpClient httpClient, string receiptItemKeyId, Uri apiUrl, IAuthStrategy authStrategy)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(authStrategy, nameof(authStrategy));

            string endpoint = string.Format(receiptKeyRetrieval, receiptItemKeyId);

            var builder = new RequestBuilder()
                .WithAuthStrategy(authStrategy)
                .WithBaseUri(apiUrl)
                .WithEndpoint(endpoint)
                .WithHttpMethod(HttpMethod.Get);

            if (authStrategy.SdkId != null)
            {
                builder = builder
                    .WithHeader(yotiAuthId, authStrategy.SdkId)
                    .WithQueryParam("appId", authStrategy.SdkId);
            }

            Request receiptItemKeyRequest = builder.Build();

            using (HttpResponseMessage response = await receiptItemKeyRequest.Execute(httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    Response.CreateYotiExceptionFromStatusCode<DigitalIdentityException>(response);

                var responseObject = await response.Content.ReadAsStringAsync();
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ReceiptItemKeyResponse>(responseObject));
            }
        }

        public static string Base64ToBase64URL(string base64Str)
        {
            try
            {
                byte[] decodedBytes = Convert.FromBase64String(base64Str);
                return Convert.ToBase64String(decodedBytes)
                    .Replace('+', '-')
                    .Replace('/', '_')
                    .TrimEnd('=');
            }
            catch (FormatException)
            {
                return "";
            }
        }

        public static (AttributeList attrData, byte[] aextra, Exception error) DecryptReceiptContent(Content content, byte[] key)
        {
            AttributeList attrData = null;
            byte[] aextra = null;

            if (content != null)
            {
                if (content.Profile != null && content.Profile.Length > 0)
                {
                    try
                    {
                        byte[] aattr = CryptoEngine.DecryptReceiptContent(content.Profile, key);
                        attrData = new AttributeList();
                        attrData.MergeFrom(aattr);
                    }
                    catch (Exception ex)
                    {
                        return (null, null, new Exception($"failed to decrypt content profile: {ex.Message}", ex));
                    }
                }

                if (content.ExtraData != null && content.ExtraData.Length > 0)
                {
                    try
                    {
                        aextra = CryptoEngine.DecryptReceiptContent(content.ExtraData, key);
                    }
                    catch (Exception ex)
                    {
                        return (null, null, new Exception($"failed to decrypt receipt content extra data: {ex.Message}", ex));
                    }
                }
            }

            return (attrData, aextra, null);
        }
    }
}
