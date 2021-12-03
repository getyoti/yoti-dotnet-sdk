using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using Org.BouncyCastle.Crypto;

namespace Yoti.Auth.Web
{
    public class RequestBuilder
    {
        private readonly Dictionary<string, string> _customHeaders = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _customContentHeaders = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _queryParams = new Dictionary<string, string>();
        private UriBuilder _baseUriBuilder;
        private string _endpoint;
        private AsymmetricCipherKeyPair _keyPair;
        private HttpMethod _httpMethod;
        private byte[] _content;
        private MultipartFormDataContent _multipartFormDataContent;

        /// <summary>
        /// Builds a (signed) request.
        /// </summary>
        public RequestBuilder()
        {
        }

        /// <summary>
        /// Uses a Base Uri to build the request (required).
        /// </summary>
        /// <param name="baseUri"></param>
        /// <returns><see cref="RequestBuilder"/></returns>
        public RequestBuilder WithBaseUri(Uri baseUri)
        {
            Validation.NotNull(baseUri, nameof(baseUri));

            _baseUriBuilder = new UriBuilder(baseUri);
            return this;
        }

        /// <summary>
        /// (Required) Adds an endpoint to the builder. This endpoint can be with or without a leading slash.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns><see cref="RequestBuilder"/></returns>
        public RequestBuilder WithEndpoint(string endpoint)
        {
            Validation.NotNullOrEmpty(endpoint, nameof(endpoint));

            if (!endpoint.StartsWith("/", StringComparison.Ordinal))
                endpoint = "/" + endpoint;

            _endpoint = endpoint;
            return this;
        }

        /// <summary>
        /// Adds a query parameter which will be added to the Uri as a query (i.e. "&key=value").
        /// </summary>
        /// <param name="key"></param> <param name="value"></param>
        /// <returns><see cref="RequestBuilder"/></returns>
        public RequestBuilder WithQueryParam(string key, string value)
        {
            _queryParams.Add(key, value);
            return this;
        }

        /// <summary>
        /// Setting the keyPair with a <see cref="StreamReader"/>. Use either this or
        /// <see cref="WithKeyPair(AsymmetricCipherKeyPair)"/>.
        /// </summary>
        /// <param name="streamReader"></param>
        /// <returns><see cref="RequestBuilder"/></returns>
        public RequestBuilder WithStreamReader(StreamReader streamReader)
        {
            _keyPair = CryptoEngine.LoadRsaKey(streamReader);
            return this;
        }

        /// <summary>
        /// Setting the keyPair with a <see cref="AsymmetricCipherKeyPair"/>. Use either this or
        /// <see cref="WithStreamReader(StreamReader)"/>.
        /// </summary>
        /// <param name="keyPair"></param>
        /// <returns><see cref="RequestBuilder"/></returns>
        public RequestBuilder WithKeyPair(AsymmetricCipherKeyPair keyPair)
        {
            _keyPair = keyPair;
            return this;
        }

        /// <summary>
        /// Adds a custom header to the request. See <see cref="HeadersFactory.AddHeaders(
        /// HttpRequestMessage, AsymmetricCipherKeyPair, HttpMethod, string, byte[])"/>
        /// to see which headers are already added. To add headers pertaining to the
        /// content, use <see cref="WithContentHeader(string, string)"/> instead of this
        /// method.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns><see cref="RequestBuilder"/></returns>
        public RequestBuilder WithHeader(string name, string value)
        {
            _customHeaders[name] = value;
            return this;
        }

        /// <summary>
        /// Adds a custom content header to the request. To add headers pertaining to
        /// the <see cref="HttpRequestMessage"/>, rather than the content, use <see cref="WithHeader(
        /// string, string)"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns><see cref="RequestBuilder"/></returns>
        public RequestBuilder WithContentHeader(string name, string value)
        {
            _customContentHeaders[name] = value;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="HttpMethod"/> which the request will use (required).
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <returns><see cref="RequestBuilder"/></returns>
        public RequestBuilder WithHttpMethod(HttpMethod httpMethod)
        {
            _httpMethod = httpMethod;
            return this;
        }

        /// <summary>
        /// Adds content to the request.
        /// </summary>
        /// <param name="content"></param>
        /// <returns><see cref="RequestBuilder"/></returns>
        public RequestBuilder WithContent(byte[] content)
        {
            Validation.IsNull(_multipartFormDataContent, nameof(_multipartFormDataContent));
           
            _content = content;
            return this;
        }

        /// <summary>
        /// Sets the boundary to be used on the multipart request.
        /// </summary>
        /// <remarks>
        ///     Use with <see cref="WithMultipartBinaryContent"/>
        /// </remarks>
        /// <param name="multipartBoundaryName">The multipart boundary name to use to split the content</param>
        /// <returns>The <see cref="RequestBuilder"/></returns>
        public RequestBuilder WithMultipartBoundary(string multipartBoundaryName)
        {
            Validation.NotNullOrWhiteSpace(multipartBoundaryName, nameof(multipartBoundaryName));
            Validation.IsNull(_content, nameof(_content));

            _multipartFormDataContent = new MultipartFormDataContent(multipartBoundaryName); 
            return this;
        }

        /// <summary>
        /// Adds binary content to the multipart request.
        /// </summary>
        /// <remarks>
        ///     Use with <see cref="WithMultipartBoundary"/>
        /// </remarks>
        /// <param name="name">The name of the binary content</param>
        /// <param name="payload">The payload of the binary content</param>
        /// <param name="contentType">The content type of the binary content</param>
        /// <param name="fileName">The filename of the binary content</param>
        /// <returns>The <see cref="RequestBuilder"/></returns>
        public RequestBuilder WithMultipartBinaryContent(
            string name,
            byte[] payload,
            string contentType, 
            string fileName)
        {
            Validation.NotNull(_multipartFormDataContent, nameof(_multipartFormDataContent));
            Validation.NotNullOrWhiteSpace(name, nameof(name));
            Validation.NotNull(payload, nameof(payload));
            Validation.NotNull(contentType, nameof(contentType));
            Validation.NotNullOrWhiteSpace(fileName, nameof(fileName)); 
            
            var binaryContent = new ByteArrayContent(payload);
            binaryContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            _multipartFormDataContent.Add(binaryContent, name, fileName);
            return this;
        }

        /// <summary>
        /// Validates and builds the <see cref="Request"/>.
        /// </summary>
        /// <returns><see cref="RequestBuilder"/></returns>
        public Request Build()
        {
            Validation.NotNull(_baseUriBuilder, nameof(_baseUriBuilder));
            Validation.NotNullOrWhiteSpace(_endpoint, nameof(_endpoint));
            Validation.NotNull(_keyPair, nameof(_keyPair));
            Validation.NotNull(_httpMethod, nameof(_httpMethod));

            if (!_baseUriBuilder.Path.EndsWith("/", StringComparison.Ordinal))
                _baseUriBuilder.Path += "/";

            string endpointWithParameters = AddQueryParametersToEndpoint();

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = GenerateRequestUri(endpointWithParameters),
                Method = _httpMethod
            };

            byte[] contentForHeaderCreation = null;

            if (_content != null)
            {
                var byteContent = new ByteArrayContent(_content);
                httpRequestMessage.Content = byteContent;
                contentForHeaderCreation = byteContent.ReadAsByteArrayAsync().Result;
            }
            else if(_multipartFormDataContent != null)
            {
                httpRequestMessage.Content = _multipartFormDataContent;
                contentForHeaderCreation = _multipartFormDataContent.ReadAsByteArrayAsync().Result;
            }

            httpRequestMessage = HeadersFactory.AddHeaders(
                httpRequestMessage,
                _keyPair,
                _httpMethod,
                endpointWithParameters,
                contentForHeaderCreation);

            AddCustomHeaders(httpRequestMessage);
            AddCustomContentHeaders(httpRequestMessage);

            return new Request(httpRequestMessage);
        }

        private Uri GenerateRequestUri(string endpointWithParameters)
        {
            //we need root character '/' for signing, but if we include it in the Uri, the path part of the Uri is removed.
            string endpointWithoutRootCharacter = endpointWithParameters.Substring(1); //remove 'root' character
            return new Uri(_baseUriBuilder.Uri, endpointWithoutRootCharacter);
        }

        private void AddCustomHeaders(HttpRequestMessage httpRequestMessage)
        {
            foreach (var header in _customHeaders)
            {
                httpRequestMessage.Headers.Add(header.Key, header.Value);
            }
        }

        private void AddCustomContentHeaders(HttpRequestMessage httpRequestMessage)
        {
            if (_customContentHeaders.Count > 0 && httpRequestMessage.Content == null)
            {
                throw new InvalidOperationException(Properties.Resources.NullHTTPContent);
            }

            foreach (var header in _customContentHeaders)
            {
                httpRequestMessage.Content.Headers.Add(header.Key, header.Value);
            }
        }

        private string AddQueryParametersToEndpoint()
        {
            var endpointBuilder = new StringBuilder(_endpoint + "?");
            if (_queryParams != null)
            {
                foreach (var param in _queryParams)
                {
                    endpointBuilder.Append($"{param.Key}={param.Value}&");
                }
            }
            endpointBuilder.Append($"timestamp={GetTimestamp()}&nonce={CryptoEngine.GenerateNonce()}");
            return endpointBuilder.ToString();
        }

        private static object GetTimestamp()
        {
            var milliseconds = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

            return milliseconds.ToString(CultureInfo.InvariantCulture);
        }
    }
}