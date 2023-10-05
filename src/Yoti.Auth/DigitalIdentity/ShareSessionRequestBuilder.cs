using System.Collections.Generic;
using Yoti.Auth.DigitalIdentity.Extensions;
using Yoti.Auth.DigitalIdentity.Policy;

namespace Yoti.Auth.DigitalIdentity
{
    public class ShareSessionRequestBuilder
    {
        private string _redirectUri;
        private Policy.Policy _dynamicPolicy;
        private Notification _notification;
        private readonly List<BaseExtension> _extensions = new List<BaseExtension>();
        private object _subject;

        /// <summary>
        /// The device's redirect url. Must be a URL relative to the Application Domain
        /// specified in Yoti Hub
        /// </summary>
        /// <param name="redirectUri"></param>
        /// <returns><see cref="ShareSessionRequestBuilder"/> with a Redirect Uri added</returns>
        public ShareSessionRequestBuilder  WithRedirectUri(string redirectUri)
        {
            _redirectUri = redirectUri;
            return this;
        }

        /// <summary>
        /// The customisable <see cref="DynamicPolicy"/> to use in the share
        /// </summary>
        /// <param name="dynamicPolicy"></param>
        /// <returns><see cref="ShareSessionRequestBuilder"/> with a Dynamic Policy added</returns>
        public ShareSessionRequestBuilder WithPolicy(Policy.Policy dynamicPolicy)
        {
            _dynamicPolicy = dynamicPolicy;
            return this;
        }

        /// <summary>
        /// The customisable <see cref="Notification"/> to use in the ShareSession
        /// </summary>
        /// <param name="notification"></param>
        /// <returns><see cref="ShareSessionRequestBuilder"/> with a Notification added</returns>
        public ShareSessionRequestBuilder WithNotification(Notification notification)
        {

            _notification = notification;
            return this;
        }

        /// <summary>
        /// <see cref="Extension{T}"/> to be activated for the application
        /// </summary>
        /// <param name="extension"><see cref="Extension{T}"/> to add</param>
        /// <returns><see cref="ShareSessionRequestBuilder"/> with an extension added</returns>
        public ShareSessionRequestBuilder WithExtension(BaseExtension extension)
        {
            _extensions.Add(extension);
            return this;
        }

        /// <summary>
        /// The subject object
        /// </summary>
        /// <param name="subject">The object describing the subject</param>
        /// <returns><see cref="ShareSessionRequestBuilder"/> with the subject details provided</returns>
        public ShareSessionRequestBuilder WithSubject(object subject)
        {
            _subject = subject;
            return this;
        }

        public ShareSessionRequest Build()
        {
            return new ShareSessionRequest(_dynamicPolicy,_redirectUri, _notification, _extensions, _subject );
        }
    }
}