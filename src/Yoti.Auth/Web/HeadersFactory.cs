﻿using System.Net.Http;
using System.Reflection;
using Org.BouncyCastle.Crypto;

namespace Yoti.Auth.Web
{
    internal static class HeadersFactory
    {
        internal static HttpRequestMessage AddHeaders(HttpRequestMessage httpRequestMessage, AsymmetricCipherKeyPair keyPair, HttpMethod httpMethod, string endpoint, byte[] httpContent)
        {
            string authDigest = SignedMessageFactory.SignMessage(httpMethod, endpoint, keyPair, httpContent);
            string SDKVersion = typeof(YotiClientEngine).GetTypeInfo().Assembly.GetName().Version.ToString();

            return PutHeaders(httpRequestMessage, authDigest, SDKVersion);
        }

        internal static HttpRequestMessage PutHeaders(HttpRequestMessage httpRequestMessage, string authDigest, string SDKVersion)
        {
            httpRequestMessage.Headers.Add(Constants.Web.DigestHeader, authDigest);
            httpRequestMessage.Headers.Add(Constants.Web.YotiSdkHeader, Constants.Web.SdkIdentifier);
            httpRequestMessage.Headers.Add(Constants.Web.YotiSdkVersionHeader, $"{Constants.Web.SdkIdentifier}-{SDKVersion}");

            return httpRequestMessage;
        }
    }
}