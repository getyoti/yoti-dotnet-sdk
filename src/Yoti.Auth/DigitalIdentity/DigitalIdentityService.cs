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
                .WithHeader(yotiAuthId, sdkId)
                .WithEndpoint(sessionCreation)
                .WithQueryParam("sdkID", sdkId)
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

        /// <summary>
        /// Creates a share session and returns the result with HTTP response headers
        /// </summary>
        internal static async Task<YotiHttpResponse<ShareSessionResult>> CreateShareSessionWithHeaders(HttpClient httpClient, Uri apiUrl, string sdkId, AsymmetricCipherKeyPair keyPair, ShareSessionRequest shareSessionRequestPayload)
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
                .WithHeader(yotiAuthId, sdkId)
                .WithEndpoint(sessionCreation)
                .WithQueryParam("sdkID", sdkId)
                .WithHttpMethod(HttpMethod.Post)
                .WithContent(body)
                .Build();

            return await shareSessionRequest.ExecuteWithHeaders(httpClient, async response =>
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DigitalIdentityException>(response);
                }

                var responseObject = await response.Content.ReadAsStringAsync();
                var deserialized = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ShareSessionResult>(responseObject));

                return deserialized;
            }).ConfigureAwait(false);
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
                .WithHeader(yotiAuthId, sdkId)
                .WithEndpoint(string.Format("{0}/{1}", sessionCreation, sessionId))
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
                .WithHeader(yotiAuthId, sdkId)
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
                .WithHeader(yotiAuthId, sdkId)
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

            string receiptUrl = Base64ToBase64URL(receiptId); 
            string endpoint = string.Format(receiptRetrieval, receiptUrl);
            
            Request ReceiptRequest = new RequestBuilder()
                .WithKeyPair(keyPair)
                .WithBaseUri(apiUrl)
                .WithHeader(yotiAuthId, sdkId)
                .WithEndpoint(endpoint)
                .WithQueryParam("sdkID", sdkId)
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
        

        public static string Base64ToBase64URL(string base64Str)
        {
            try
            {
                byte[] decodedBytes = Convert.FromBase64String(base64Str);
                string base64URL = Convert.ToBase64String(decodedBytes)
                    .Replace('+', '-')
                    .Replace('/', '_')
                    .TrimEnd('=');
                return base64URL;
            }
            catch (FormatException)
            {
                return ""; 
            }
        }

        public static async Task<SharedReceiptResponse> GetShareReceipt(HttpClient httpClient, string clientSdkId, Uri apiUrl, AsymmetricCipherKeyPair key, string receiptId)
        {
            Validation.NotNullOrEmpty(receiptId, nameof(receiptId));
            try
            {
                var receiptResponse = await GetReceipt(httpClient, receiptId, clientSdkId, apiUrl, key);
                var itemKeyId = receiptResponse.WrappedItemKeyId;
                
                var encryptedItemKeyResponse = await GetReceiptItemKey(httpClient, itemKeyId, clientSdkId, apiUrl, key);

                var receiptContentKey = CryptoEngine.UnwrapReceiptKey(receiptResponse.WrappedKey, encryptedItemKeyResponse.Value, encryptedItemKeyResponse.Iv, key);

                var (attrData, aextra, decryptAttrDataError) = DecryptReceiptContent(receiptResponse.Content, receiptContentKey);
                if (decryptAttrDataError != null)
                {
                    throw new Exception($"An unexpected error occurred: {decryptAttrDataError.Message}");
                }

                var  parsedAttributesApp = AttributeConverter.ConvertToBaseAttributes(attrData);
                var appProfile = new ApplicationProfile(parsedAttributesApp
                );
                
                var (attrOtherData, aOtherExtra, decryptOtherAttrDataError) = DecryptReceiptContent(receiptResponse.OtherPartyContent, receiptContentKey);
                if (decryptAttrDataError != null)
                {
                    throw new Exception($"An unexpected error occurred: {decryptAttrDataError.Message}");
                }

                var userProfile = new YotiProfile();
                if (attrOtherData != null)
                {
                    var  parsedAttributesUser = AttributeConverter.ConvertToBaseAttributes(attrOtherData);
                    userProfile = new YotiProfile(parsedAttributesUser);
                }
                
                
                ExtraData userExtraData = new ExtraData();
                if (aOtherExtra != null)
                {
                    userExtraData = ExtraDataConverter.ParseExtraDataProto(aOtherExtra);
                }
                ExtraData appExtraData = new ExtraData();
                if (aextra != null)
                {
                   
                    appExtraData = ExtraDataConverter.ParseExtraDataProto(aextra);
                }
                
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
                        ApplicationProfile = appProfile,
                        ExtraData = appExtraData
                    },
                    Error = receiptResponse.Error,
                    ErrorDetails = receiptResponse.ErrorDetails
                    
                };

                return sharedReceiptResponse;
            }
            catch  (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
           
            }
        }

        /// <summary>
        /// Gets a share receipt and returns the result with HTTP response headers
        /// </summary>
        public static async Task<YotiHttpResponse<SharedReceiptResponse>> GetShareReceiptWithHeaders(HttpClient httpClient, string clientSdkId, Uri apiUrl, AsymmetricCipherKeyPair key, string receiptId)
        {
            Validation.NotNullOrEmpty(receiptId, nameof(receiptId));
            
            string receiptUrl = Base64ToBase64URL(receiptId);
            string endpoint = string.Format(receiptRetrieval, receiptUrl);
            
            Request receiptRequest = new RequestBuilder()
                .WithKeyPair(key)
                .WithBaseUri(apiUrl)
                .WithHeader(yotiAuthId, clientSdkId)
                .WithEndpoint(endpoint)
                .WithQueryParam("sdkID", clientSdkId)
                .WithHttpMethod(HttpMethod.Get)
                .Build();

            // Use ExecuteWithHeaders to capture the response headers
            return await receiptRequest.ExecuteWithHeaders(httpClient, async response =>
            {
                try
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        Response.CreateYotiExceptionFromStatusCode<DigitalIdentityException>(response);
                    }

                    var responseObject = await response.Content.ReadAsStringAsync();
                    var receiptResponse = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ReceiptResponse>(responseObject));
                    
                    var itemKeyId = receiptResponse.WrappedItemKeyId;
                    var encryptedItemKeyResponse = await GetReceiptItemKey(httpClient, itemKeyId, clientSdkId, apiUrl, key);
                    var receiptContentKey = CryptoEngine.UnwrapReceiptKey(receiptResponse.WrappedKey, encryptedItemKeyResponse.Value, encryptedItemKeyResponse.Iv, key);

                    var (attrData, aextra, decryptAttrDataError) = DecryptReceiptContent(receiptResponse.Content, receiptContentKey);
                    if (decryptAttrDataError != null)
                    {
                        throw new Exception($"An unexpected error occurred: {decryptAttrDataError.Message}");
                    }

                    var parsedAttributesApp = AttributeConverter.ConvertToBaseAttributes(attrData);
                    var appProfile = new ApplicationProfile(parsedAttributesApp);
                    
                    var (attrOtherData, aOtherExtra, decryptOtherAttrDataError) = DecryptReceiptContent(receiptResponse.OtherPartyContent, receiptContentKey);
                    if (decryptOtherAttrDataError != null)
                    {
                        throw new Exception($"An unexpected error occurred: {decryptOtherAttrDataError.Message}");
                    }

                    var userProfile = new YotiProfile();
                    if (attrOtherData != null)
                    {
                        var parsedAttributesUser = AttributeConverter.ConvertToBaseAttributes(attrOtherData);
                        userProfile = new YotiProfile(parsedAttributesUser);
                    }

                    ExtraData userExtraData = new ExtraData();
                    if (aOtherExtra != null)
                    {
                        userExtraData = ExtraDataConverter.ParseExtraDataProto(aOtherExtra);
                    }
                    
                    ExtraData appExtraData = new ExtraData();
                    if (aextra != null)
                    {
                        appExtraData = ExtraDataConverter.ParseExtraDataProto(aextra);
                    }

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
                            ApplicationProfile = appProfile,
                            ExtraData = appExtraData
                        },
                        Error = receiptResponse.Error,
                        ErrorDetails = receiptResponse.ErrorDetails
                    };

                    return sharedReceiptResponse;
                }
                catch (Exception ex)
                {
                    throw new Exception($"An unexpected error occurred: {ex.Message}");
                }
            }).ConfigureAwait(false);
        }

        private static async Task<ReceiptItemKeyResponse> GetReceiptItemKey(HttpClient httpClient, string receiptItemKeyId, string sdkId, Uri apiUrl, AsymmetricCipherKeyPair keyPair)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));
            string endpoint = string.Format(receiptKeyRetrieval, receiptItemKeyId);

            Request ReceiptItemKeyRequest = new RequestBuilder()
                .WithKeyPair(keyPair)
                .WithBaseUri(apiUrl)
                .WithHeader(yotiAuthId, sdkId)
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
