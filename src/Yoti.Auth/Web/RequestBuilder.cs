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
        private readonly Dictionary<string, string> _queryParams = new Dictionary<string, string>();
        private UriBuilder _baseUriBuilder;
        private string _endpoint;
        private AsymmetricCipherKeyPair _keyPair;
        private HttpMethod _httpMethod;
        private byte[] _content = null;

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
        /// Setting the keyPair with a <see cref="StreamReader"/>. Use either this or
        /// <see cref="WithStreamReader(StreamReader)"/>.
        /// </summary>
        /// <param name="streamReader"></param> <param name="keyPair"></param>
        /// <returns><see cref="RequestBuilder"/></returns>
        public RequestBuilder WithKeyPair(AsymmetricCipherKeyPair keyPair)
        {
            _keyPair = keyPair;
            return this;
        }

        /// <summary>
        /// Adds a custom header to the request. See <see cref="HeadersFactory.AddHeaders(
        /// HttpRequestMessage, AsymmetricCipherKeyPair, HttpMethod, string, byte[])"/>
        /// to see which headers are already added.
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
            _content = content;
            return this;
        }

        /// <summary>
        /// Validates and builds the <see cref="Request"/>.
        /// </summary>
        /// <returns><see cref="RequestBuilder"/></returns>
        public Request Build()
        {
            Validation.NotNull(_baseUriBuilder, nameof(_baseUriBuilder));
            Validation.NotNullOrEmpty(_endpoint, nameof(_endpoint));
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

            if (_content != null)
            {
                httpRequestMessage.Content = new ByteArrayContent(_content);
            }

            httpRequestMessage = HeadersFactory.AddHeaders(
                httpRequestMessage,
                _keyPair,
                _httpMethod,
                endpointWithParameters,
                _content);

            AddCustomHeaders(httpRequestMessage);

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