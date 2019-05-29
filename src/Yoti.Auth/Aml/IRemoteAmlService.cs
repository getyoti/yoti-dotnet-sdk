﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Yoti.Auth.Web;

namespace Yoti.Auth.Aml
{
    internal interface IRemoteAmlService
    {
        Task<AmlResult> PerformCheck(HttpClient httpClient, IHttpRequester httpRequester, Dictionary<string, string> headers, string apiUrl, string endpoint, byte[] httpContent);
    }
}