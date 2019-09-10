using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Web;

namespace Yoti.Auth.Aml
{
    internal interface IRemoteAmlService
    {
        Task<AmlResult> PerformCheck(HttpClient httpClient,
            AsymmetricCipherKeyPair keyPair,
            Uri apiUrl,
            string sdkId,
            byte[] httpContent);
    }
}