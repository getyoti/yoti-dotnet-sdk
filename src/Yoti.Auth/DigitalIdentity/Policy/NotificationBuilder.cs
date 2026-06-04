using System.Collections.Generic;

namespace Yoti.Auth.DigitalIdentity.Policy
{
    public class NotificationBuilder
    {
        private string _url;
        private string _method;
        private Dictionary<string, string> _headers;
        private bool _verifyTls;
        
        /// <summary>
        /// Set the URL for the notification, required if 'notification' is defined, required
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public NotificationBuilder WithUrl(string url)
        {
            _url = url;
            return this;
        }
        
        /// <summary>
        /// Set the method for the notification, defaults to 'POST', optional
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public NotificationBuilder WithMethod(string method)
        {
            _method = method;
            return this;
        }
        
        /// <summary>
        /// Set the headers for the notification, optional
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public NotificationBuilder WithHeaders(Dictionary<string, string> headers)
        {
            _headers = headers;
            return this;
        }
        
        /// <summary>
        /// Set to false to disable TLS verification, defaults to 'true' if URL is HTTPS, optional
        /// </summary>
        /// <param name="verifyTls"></param>
        /// <returns></returns>
        public NotificationBuilder WithVerifyTls(bool verifyTls)
        {
            _verifyTls = verifyTls;
            return this;
        }
        
        public Notification Build()
        {
            Validation.NotNullOrEmpty(_url, nameof(_url));
            return new Notification(_url, _method, _headers, _verifyTls);
        }
    }
}
