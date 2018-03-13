using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yoti.Auth.Aml
{
    internal interface IRemoteAmlService
    {
        Task<AmlResult> PerformCheck(IHttpRequester httpRequester, IAmlProfile amlProfile, Dictionary<string, string> headers, string apiUrl, string endpoint, byte[] httpContent);
    }
}