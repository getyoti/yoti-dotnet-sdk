using System;
using System.Collections.Generic;
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

namespace Yoti.Auth.DigitalIdentity
{
    public static class DigitalIdentityService
    {
        private const string identitySessionReceiptRetrieval = "/path/to/identity/session/receipt/retrieval/{0}";
        private const string identitySessionReceiptKeyRetrieval = "/path/to/identity/session/receipt/key/retrieval/{0}";

        internal static async Task<ShareSessionResult> CreateShareSession(HttpClient httpClient, Uri apiUrl, string sdkId, AsymmetricCipherKeyPair keyPair, ShareSessionRequest shareSessionRequestPayload)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));
            Validation.NotNull(shareSessionRequestPayload, nameof(shareSessionRequestPayload));

            string serializedScenario = JsonConvert.SerializeObject(
                shareSessionRequestPayload,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            byte[] body = Encoding.UTF8.GetBytes(serializedScenario);

            Request shareSessionRequest = new RequestBuilder()
                .WithKeyPair(keyPair)
                .WithBaseUri(apiUrl)
                .WithHeader("X-Yoti-Auth-Id", sdkId)
                .WithEndpoint($"/v2/sessions")
                .WithQueryParam("appId", sdkId)
                .WithHttpMethod(HttpMethod.Post)
                .WithContent(body)
                .Build();

            using (HttpResponseMessage response = await shareSessionRequest.Execute(httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DigitalIdentityException>(response);
                }

                var responseObject = await response.Content.ReadAsStringAsync();
                var deserialized = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ShareSessionResult>(responseObject));

                return deserialized;

            }
        }

        internal static async Task<GetSessionResult> GetSession(HttpClient httpClient, Uri apiUrl, string sdkId, AsymmetricCipherKeyPair keyPair, string sessionId)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));
            Validation.NotNull(sessionId, nameof(sessionId));           


            Request getSessionRequest = new RequestBuilder()
                .WithKeyPair(keyPair)
                .WithBaseUri(apiUrl)
                .WithHeader("X-Yoti-Auth-Id", sdkId)
                .WithEndpoint(string.Format($"/v2/sessions/{0}", sessionId))
                .WithQueryParam("appId", sdkId)
                .WithHttpMethod(HttpMethod.Get)
                .Build();

            using (HttpResponseMessage response = await getSessionRequest.Execute(httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DigitalIdentityException>(response);
                }

                var responseObject = await response.Content.ReadAsStringAsync();
                var deserialized = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<GetSessionResult>(responseObject));

                return deserialized;

            }
        }

        internal static async Task<CreateQrResult> CreateQrCode(HttpClient httpClient, Uri apiUrl, string sdkId, AsymmetricCipherKeyPair keyPair, string sessionId,QrRequest qrRequestPayload)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));

            string serializedQrCode = JsonConvert.SerializeObject(
                qrRequestPayload,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            byte[] body = Encoding.UTF8.GetBytes(serializedQrCode);


            Request createQrRequest = new RequestBuilder()
                .WithKeyPair(keyPair)
                .WithBaseUri(apiUrl)
                .WithHeader("X-Yoti-Auth-Id", sdkId)
                .WithEndpoint(string.Format($"/v2/sessions/{0}/qr-codes", sessionId))
                .WithQueryParam("appId", sdkId)
                .WithHttpMethod(HttpMethod.Post)
                .WithContent(body)
                .Build();


            using (HttpResponseMessage response = await createQrRequest.Execute(httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DigitalIdentityException>(response);
                }

                var responseObject = await response.Content.ReadAsStringAsync();
                var deserialized = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<CreateQrResult>(responseObject));

                return deserialized;

            }
        }


        internal static async Task<GetQrCodeResult> GetQrCode(HttpClient httpClient, Uri apiUrl, string sdkId, AsymmetricCipherKeyPair keyPair, string qrCodeId)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));
            Validation.NotNull(qrCodeId, nameof(qrCodeId));

            Request QrCodeRequest = new RequestBuilder()
                .WithKeyPair(keyPair)
                .WithBaseUri(apiUrl)
                .WithHeader("X-Yoti-Auth-Id", sdkId)
                .WithEndpoint(string.Format($"/v2/qr-codes/{0}", qrCodeId))
                .WithQueryParam("appId", sdkId)
                .WithHttpMethod(HttpMethod.Get)
                .Build();

            using (HttpResponseMessage response = await QrCodeRequest.Execute(httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DigitalIdentityException>(response);
                }

                var responseObject = await response.Content.ReadAsStringAsync();
                var deserialized = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<GetQrCodeResult>(responseObject));

                return deserialized;

            }
        }
        
        private static async Task<ReceiptResponse> GetReceipt(HttpClient httpClient, string receiptId,  string sdkId,Uri apiUrl, AsymmetricCipherKeyPair keyPair)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));

            string receiptUrl = Convert.ToBase64String(Encoding.UTF8.GetBytes(receiptId));
            string endpoint = string.Format(identitySessionReceiptRetrieval, receiptUrl);
            
            Request ReceiptRequest = new RequestBuilder()
                .WithKeyPair(keyPair)
                .WithBaseUri(apiUrl)
                .WithHeader("X-Yoti-Auth-Id", sdkId)
                .WithEndpoint(endpoint)
                .WithQueryParam("appId", sdkId)
                .WithHttpMethod(HttpMethod.Get)
                .Build();

            using (HttpResponseMessage response = await ReceiptRequest.Execute(httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DigitalIdentityException>(response);
                }

                var responseObject = await response.Content.ReadAsStringAsync();
                var deserialized = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ReceiptResponse>(responseObject));

                return deserialized;

            }
        }
       
        private static Dictionary<string, List<BaseAttribute>> ParseProfileContent(AsymmetricCipherKeyPair keyPair, string wrappedReceiptKey, string profileContent)
        {
            var parsedAttributes = new Dictionary<string, List<BaseAttribute>>();

            if (!string.IsNullOrEmpty(profileContent))
            {
                ProtoBuf.Attribute.AttributeList profileAttributeList = CryptoEngine.DecryptAttributeList(
                    wrappedReceiptKey,
                    profileContent,
                    keyPair);

                parsedAttributes = AttributeConverter.ConvertToBaseAttributes(profileAttributeList);
            }

            return parsedAttributes;
        }

        public static async Task<(SharedReceiptResponse, Exception)> GetShareReceipt(HttpClient httpClient, string receiptId, string clientSdkId, Uri apiUrl, AsymmetricCipherKeyPair key)
        {
            try
            {
                var receiptResponse = await GetReceipt(httpClient, receiptId, clientSdkId, apiUrl, key);
                var itemKeyId = receiptResponse.WrappedItemKeyId;

                var encryptedItemKeyResponse = await GetReceiptItemKey(httpClient, itemKeyId, clientSdkId, apiUrl, key);
                
                var receiptContentKey = CryptoEngine.UnwrapReceiptKey(receiptResponse.WrappedKey, encryptedItemKeyResponse.Value, encryptedItemKeyResponse.Iv, key);
 
                var (attrData, aextra, decryptAttrDataError) = DecryptReceiptContent(receiptResponse.Content, receiptContentKey);
                if (decryptAttrDataError != null)
                {
                    return (null, new Exception($"failed to decrypt receipt content: {decryptAttrDataError.Message}"));
                }


                var userProfile = new YotiProfile(
                ParseProfileContent(key, Encoding.UTF8.GetString(receiptResponse.WrappedKey), Encoding.UTF8.GetString(receiptResponse.OtherPartyContent.Profile)));
       
                var applicationProfile = new ApplicationProfile(
                    ParseProfileContent(key, Encoding.UTF8.GetString(receiptResponse.WrappedKey), Encoding.UTF8.GetString(receiptResponse.Content.Profile)));

                ExtraData userExtraData = new ExtraData();
                userExtraData = CryptoEngine.DecryptExtraData(
                        Encoding.UTF8.GetString(receiptResponse.WrappedKey),
                        Encoding.UTF8.GetString(receiptResponse.OtherPartyContent.ExtraData),
                        key);

                ExtraData appExtraData = new ExtraData();
                appExtraData = CryptoEngine.DecryptExtraData(
                        Encoding.UTF8.GetString(receiptResponse.WrappedKey),
                        Encoding.UTF8.GetString(receiptResponse.Content.ExtraData),
                        key);


                var sharedReceiptResponse = new SharedReceiptResponse
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
                        ApplicationProfile = applicationProfile,
                        ExtraData = appExtraData
                    },
                    Error = receiptResponse.Error
                };

                return (sharedReceiptResponse, null);
            }
            catch (Exception ex)
            {
                return (null, new Exception($"An unexpected error occurred: {ex.Message}"));
            }
        }

        private static async Task<ReceiptItemKeyResponse> GetReceiptItemKey(HttpClient httpClient, string receiptItemKeyId, string sdkId, Uri apiUrl, AsymmetricCipherKeyPair keyPair)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));
            string endpoint = string.Format(identitySessionReceiptKeyRetrieval, receiptItemKeyId);


            Request ReceiptItemKeyRequest = new RequestBuilder()
                .WithKeyPair(keyPair)
                .WithBaseUri(apiUrl)
                .WithHeader("X-Yoti-Auth-Id", sdkId)
                .WithEndpoint(endpoint)
                .WithQueryParam("appId", sdkId)
                .WithHttpMethod(HttpMethod.Get)
                .Build();

            using (HttpResponseMessage response = await ReceiptItemKeyRequest.Execute(httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DigitalIdentityException>(response);
                }

                var responseObject = await response.Content.ReadAsStringAsync();
                var deserialized = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ReceiptItemKeyResponse>(responseObject));

                return deserialized;

            }
        }

        public static (AttributeList attrData, byte[] aextra, Exception error) DecryptReceiptContent(Content content, byte[] key)
        {
            AttributeList attrData = null;
            byte[] aextra = null;
            Exception error = null;

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
                        error = new Exception($"failed to decrypt content profile: {ex.Message}", ex);
                        return (null, null, error);
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
                        error = new Exception($"failed to decrypt receipt content extra data: {ex.Message}", ex);
                        return (null, null, error);
                    }
                }
            }

            return (attrData, aextra, null);
        }

       
    }
}