using System.Collections.Generic;
using Yoti.Auth.DigitalIdentity.Extensions;
using Yoti.Auth.DigitalIdentity.Policy;

namespace Yoti.Auth.DigitalIdentity
{
    public class ShareSessionRequestBuilder
    {
        private string _callbackEndpoint;
        private DynamicPolicy _dynamicPolicy;
        private readonly List<BaseExtension> _extensions = new List<BaseExtension>();
        private object _subject;

        /// <summary>
        /// The device's callback endpoint. Must be a URL relative to the Application Domain
        /// specified in Yoti Hub
        /// </summary>
        /// <param name="callbackEndpoint"></param>
        /// <returns><see cref="DynamicScenarioBuilder"/> with a Callback Endpoint added</returns>
        public ShareSessionRequestBuilder  WithCallbackEndpoint(string callbackEndpoint)
        {
            _callbackEndpoint = callbackEndpoint;
            return this;
        }

        /// <summary>
        /// The customisable <see cref="DynamicPolicy"/> to use in the share
        /// </summary>
        /// <param name="dynamicPolicy"></param>
        /// <returns><see cref="ShareSessionRequestBuilder"/> with a Dynamic Policy added</returns>
        public ShareSessionRequestBuilder WithPolicy(DynamicPolicy dynamicPolicy)
        {
            _dynamicPolicy = dynamicPolicy;
            return this;
        }

        /// <summary>
        /// <see cref="Extension{T}"/> to be activated for the application
        /// </summary>
        /// <param name="extension"><see cref="Extension{T}"/> to add</param>
        /// <returns><see cref="DynamicScenarioBuilder"/> with an extension added</returns>
        public ShareSessionRequestBuilder WithExtension(BaseExtension extension)
        {
            _extensions.Add(extension);
            return this;
        }

        /// <summary>
        /// The subject object
        /// </summary>
        /// <param name="subject">The object describing the subject</param>
        /// <returns><see cref="DynamicScenarioBuilder"/> with the subject details provided</returns>
        public ShareSessionRequestBuilder WithSubject(object subject)
        {
            _subject = subject;
            return this;
        }

        public ShareSessionRequest Build()
        {
            return new ShareSessionRequest(_callbackEndpoint, _dynamicPolicy, _extensions, _subject);
        }
    }
}