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
        private const string errorFailedToGetSignedRequest = "Failed to get signed request: {0}";
        private const string errorFailedToExecuteRequest = "Failed to execute request: {0}";
        private const string errorFailedToReadBody = "Failed to read response body: {0}";
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

        private const string IdentitySessionReceiptRetrieval = "your-endpoint/{0}";
    private const string IdentitySessionReceiptKeyRetrieval = "your-endpoint/{0}";
    private const string ErrorFailedToGetSignedRequest = "Failed to get signed request: {0}";
    private const string ErrorFailedToExecuteRequest = "Failed to execute request: {0}";
/*
    public static async Task<ReceiptResponse> GetShareReceipt(HttpClient httpClient, string receiptId, string clientSdkId, string apiUrl, RSA key)
    {
        ReceiptResponse result = new ReceiptResponse();

        try
        {
            var receiptResponse = await GetReceipt(httpClient, receiptId, clientSdkId, apiUrl, key);
            if (receiptResponse.Error != null)
            {
                throw new Exception($"Failed to get receipt: {receiptResponse.Error}");
            }

            var itemKeyId = receiptResponse.WrappedItemKeyId;
            var encryptedItemKeyResponse = await GetReceiptItemKey(httpClient, itemKeyId, clientSdkId, apiUrl, key);

            if (encryptedItemKeyResponse.Error != null)
            {
                throw new Exception($"Failed to get receipt item key: {encryptedItemKeyResponse.Error}");
            }

            var receiptContentKey = CryptoUtil.UnwrapReceiptKey(receiptResponse.WrappedKey, encryptedItemKeyResponse.Value, encryptedItemKeyResponse.Iv, key);

            var (attrData, aextra, _) = DecryptReceiptContent(receiptResponse.Content, receiptContentKey);

            var applicationProfile = NewApplicationProfile(attrData);
            var extraDataValue = Extra.NewExtraData(aextra);

            var (uattrData, uextra, _) = DecryptReceiptContent(receiptResponse.OtherPartyContent, receiptContentKey);
            var userProfile = NewUserProfile(uattrData);
            var userExtraDataValue = Extra.NewExtraData(uextra);

            result.SharedReceipt = new SharedReceiptResponse
            {
                ID = receiptResponse.ID,
                SessionID = receiptResponse.SessionID,
                RememberMeID = receiptResponse.RememberMeID,
                ParentRememberMeID = receiptResponse.ParentRememberMeID,
                Timestamp = receiptResponse.Timestamp,
                UserContent = new UserContent
                {
                    UserProfile = userProfile,
                    ExtraData = userExtraDataValue,
                },
                ApplicationContent = new ApplicationContent
                {
                    ApplicationProfile = applicationProfile,
                    ExtraData = extraDataValue,
                },
                Error = receiptResponse.Error,
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return result;
    }
*/
/*
    private static async Task<ReceiptResponse> GetReceipt(Requests.IHttpClient httpClient, string receiptId, string clientSdkId, string apiUrl, RSA key)
    {
        var receiptUrl = req.Base64ToBase64URL(receiptId);
        var endpoint = string.Format(IdentitySessionReceiptRetrieval, receiptUrl);

        var headers = Requests.AuthHeader(clientSdkId);
        var request = new Requests.SignedRequest
        {
            Key = key,
            HttpMethod = "GET",
            BaseURL = apiUrl,
            Endpoint = endpoint,
            Headers = headers,
        }.Request();

        var response = await Requests.Execute(httpClient, request);
        response.EnsureSuccessStatusCode();

        var responseBytes = await response.Content.ReadAsByteArrayAsync();
        var receipt = JsonSerializer.Deserialize<ReceiptResponse>(responseBytes);

        return receipt;
    }

    private static async Task<ReceiptItemKeyResponse> GetReceiptItemKey(HttpClient httpClient, string receiptItemKeyId, string clientSdkId, string apiUrl, RSA key)
    {
        var endpoint = string.Format(IdentitySessionReceiptKeyRetrieval, receiptItemKeyId);
        var headers = Requests.AuthHeader(clientSdkId);
        var request = new SignedRequest
        {
            Key = key,
            HttpMethod = "GET",
            BaseURL = apiUrl,
            Endpoint = endpoint,
            Headers = headers,
        }.Request();

        var response = await Requests.Execute(httpClient, request);
        response.EnsureSuccessStatusCode();

        var responseBytes = await response.Content.ReadAsByteArrayAsync();
        var receiptItemKey = JsonSerializer.Deserialize<ReceiptItemKeyResponse>(responseBytes);

        return receiptItemKey;
    }

    private static (Yoti.Auth.ProtoBuf.Attribute.AttributeList, byte[], Exception) DecryptReceiptContent(Content content, byte[] key)
    {
        Yoti.Auth.ProtoBuf.Attribute.AttributeList attrData = null;
        byte[] aextra = null;
        Exception error = null;

        try
        {
            if (content != null)
            {
                if (content.Profile != null && content.Profile.Length > 0)
                {
                    var aattr = CryptoEngine.DecryptReceiptContent(content.Profile, key);
                    if (aattr != null)
                    {
                        attrData = new Yoti.Auth.ProtoBuf.Attribute.AttributeList();
                        // Implement MergeFrom method
                        // attrData.MergeFrom(aattr);
                    }
                }

                if (content.ExtraData != null && content.ExtraData.Length > 0)
                {
                    aextra = CryptoEngine.DecryptReceiptContent(content.ExtraData, key);
                }
            }
        }
        catch (Exception ex)
        {
            error = new Exception($"Failed to decrypt receipt content: {ex.Message}", ex);
        }

        return (attrData, aextra, error);
    }
    */
        /*
         public static async Task<ReceiptResponse> GetShareReceipt(HttpClient httpClient, string receiptId, string clientSdkId, Uri apiUrl, AsymmetricCipherKeyPair key)
    {
        ReceiptResponse receipt = new ReceiptResponse();

        try
        {
            ReceiptResponse receiptResponse = await GetReceipt(httpClient, receiptId, clientSdkId, apiUrl, key);

            if (receiptResponse.Error != null)
            {
                throw new Exception($"Failed to get receipt: {receiptResponse.Error}");
            }

            receipt.ID = receiptResponse.ID;
            receipt.SessionID = receiptResponse.SessionID;
            receipt.RememberMeID = receiptResponse.RememberMeID;
            receipt.ParentRememberMeID = receiptResponse.ParentRememberMeID;
            receipt.Timestamp = receiptResponse.Timestamp;

            string itemKeyId = receiptResponse.WrappedItemKeyId;

            ReceiptItemKeyResponse ItemKeyResponse = await GetReceiptItemKey(httpClient, itemKeyId, clientSdkId, apiUrl, key);
            
            byte[] receiptContentKey = CryptoEngine.UnwrapReceiptKey(receiptResponse.WrappedKey, ItemKeyResponse.Value, ItemKeyResponse.Iv, key);
            
            var aResult = DecryptReceiptContent(receiptResponse.Content, receiptContentKey);
            var uResult = DecryptReceiptContent(receiptResponse.OtherPartyContent, receiptContentKey);
            
            var userProfile = new YotiProfile(
                ParseProfileContent(key, receipt. receipt.WrappedReceiptKey, receipt.OtherPartyProfileContent));
            SetAddressToBeFormattedAddressIfNull(userProfile);

            var applicationProfile = new ApplicationProfile(
                ParseProfileContent(keyPair, receipt.WrappedReceiptKey, receipt.ProfileContent));
            
            ExtraData extraData = new ExtraData();

            if (!string.IsNullOrEmpty(parsedResponse.Receipt.ExtraDataContent))
            {
                extraData = CryptoEngine.DecryptExtraData(
                    receipt.WrappedReceiptKey,
                    parsedResponse.Receipt.ExtraDataContent,
                    keyPair);
            }
            
            AttributeList attrData, uattrData;
            byte[] aextra, uextra;
            
            var aResult = DecryptReceiptContent(receiptResponse.Content, receiptContentKey);
            attrData = aResult.AttrData;
            aextra = aResult.Aextra;
            
            var uResult = DecryptReceiptContent(receiptResponse.OtherPartyContent, receiptContentKey);
            uattrData = uResult.AttrData;
            uextra = uResult.Aextra;

            Yoti.Auth.ActivityDetailsParser. ParseProfileContent(keyPair, receipt.WrappedReceiptKey, receipt.ProfileContent));
            
            ApplicationProfile applicationProfile = new ApplicationProfile(attrData);
            ExtraData extraData = new ExtraData(aextra);
            ExtraData extraDataValue = extraData..NewExtraData(aextra);
            */
            /*
            receipt.UserContent = new UserContent
            {
                UserProfile = new UserProfile(uattrData),
                ExtraData = ExtraData.NewExtraData(uextra)
            };

            receipt.ApplicationContent = new ApplicationContent
            {
                ApplicationProfile = applicationProfile,
                ExtraData = extraDataValue
            };
            
            receipt.Error = receiptResponse.Error;
        }
        catch (Exception ex)
        {
            receipt.Error = String.Format($"Failed to process receipt: {ex.Message}", ex);
        }

        return receipt;
    }
     
        private static async Task<ReceiptItemKeyResponse> GetReceiptItemKey(HttpClient httpClient, string receiptItemKeyId, string sdkId, Uri apiUrl , AsymmetricCipherKeyPair keyPair)
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
        public class DecryptResult
        {
            public AttributeList AttrData { get; set; }
            public byte[] Aextra { get; set; }
            public Exception Error { get; set; }
        }
        public static DecryptResult DecryptReceiptContent(Content content, byte[] key)
        {
            DecryptResult result = new DecryptResult();

            try
            {
                if (content != null)
                {
                    if (content.Profile != null && content.Profile.Length > 0)
                    {
                        byte[] aattr = CryptoEngine.DecryptReceiptContent(content.Profile, key);
                        if (aattr != null)
                        {
                            result.AttrData = new AttributeList();
                            result.AttrData.MergeFrom(aattr);
                        }
                    }

                    if (content.ExtraData != null && content.ExtraData.Length > 0)
                    {
                        result.Aextra = CryptoEngine.DecryptReceiptContent(content.ExtraData, key);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = new Exception($"Failed to decrypt receipt content: {ex.Message}", ex);
            }

            return result;
        }
        
        */
       
    }
}