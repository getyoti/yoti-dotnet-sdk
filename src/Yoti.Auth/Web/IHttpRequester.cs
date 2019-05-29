using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Yoti.Auth.Web
{
    internal interface IHttpRequester
    {
        Task<Response> DoRequest(HttpClient httpClient, HttpMethod httpMethod, Uri uri, Dictionary<string, string> headers, byte[] byteContent);
    }
}