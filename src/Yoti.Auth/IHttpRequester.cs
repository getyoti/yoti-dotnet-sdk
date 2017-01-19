using AttrpubapiV1;
using CompubapiV1;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Yoti.Auth.DataObjects;

namespace Yoti.Auth
{
    internal interface IHttpRequester
    {
        Task<Response> DoRequest(Uri uri, Dictionary<string, string> headers);
    }
}