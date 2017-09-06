using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yoti.Auth
{
    internal interface IHttpRequester
    {
        Task<Response> DoRequest(Uri uri, Dictionary<string, string> headers);
    }
}