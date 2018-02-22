using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;

namespace Yoti.Auth.Aml
{
    internal class RemoteAmlService
    {
        private SignedMessageFactory _signedMessageFactory;
        private readonly IHttpRequester _httpRequester = new HttpRequester();

        public RemoteAmlService(SignedMessageFactory signedMessageFactory)
        {
            _signedMessageFactory = signedMessageFactory;
        }

        public async Task<AmlResult> PerformCheck(IAmlProfile amlProfile, Dictionary<string, string> headers, string apiUrl, string endpoint, byte[] httpContent)
        {
            if (amlProfile == null)
                throw new ArgumentNullException("amlProfile");

            try
            {
                HttpMethod httpMethod = HttpMethod.Post;

                Response response = await _httpRequester.DoRequest(
                    new HttpClient(),
                    httpMethod,
                    new Uri(apiUrl + endpoint),
                    headers,
                    httpContent);

                if (!response.Success)
                {
                    CreateExceptionFromStatusCode(response);
                }

                var content = response.Content;

                var amlResult = new AmlResult
                {
                };

                return amlResult;
            }
            catch (Exception ex)
            {
                if (ex is AmlException)
                    throw;

                throw new AmlException(
                    string.Format(
                        "Inner exception:{0}{1}",
                        Environment.NewLine,
                        ex.Message),
                    ex);
            }
        }

        private AmlException CreateExceptionFromStatusCode(Response response)
        {
            switch ((HttpStatusCode)response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new AmlException(
                        string.Format(
                            "Failed validation:{0}{1}",
                            Environment.NewLine,
                            response.Content));

                case HttpStatusCode.Unauthorized:
                    throw new AmlException(
                        string.Format(
                        "Failed authorization with the given key:{0}{1}",
                            Environment.NewLine,
                            response.Content));

                case HttpStatusCode.InternalServerError:
                    throw new AmlException(
                        string.Format(
                            "An unexpected error occurred on the server:{0}{1}",
                            Environment.NewLine,
                            response.Content));

                default:
                    throw new AmlException(
                        string.Format(
                            "Unexpected error:{0}{1}",
                            Environment.NewLine,
                            response.Content));
            }
        }
    }
}