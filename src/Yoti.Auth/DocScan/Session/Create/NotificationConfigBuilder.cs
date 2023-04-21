using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Create
{
    /// <summary>
    /// Builder to assist in the creation of <see cref="NotificationConfig"/>
    /// </summary>
    public class NotificationConfigBuilder
    {
        private readonly List<string> _topics = new List<string>();
        private string _authToken;
        private string _authType;
        private string _endpoint;
        
        /// <summary>
        /// Sets the authorization token to be included in callback messages
        /// </summary>
        /// <param name="authToken">the authorization token</param>
        /// <returns>The builder</returns>
        public NotificationConfigBuilder WithAuthToken(string authToken)
        {
            _authToken = authToken;  
            return this;
        }

        /// <summary>
        ///     <para>
        ///         Sets the <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Authentication#authentication_schemes">HTTP Authentication Scheme</see> to "Bearer" for callback requests to your endpoint.
        ///     </para>
        ///     <para>
        ///         The authorization token will be unchanged by the backend when sent with notifications
        ///     </para>
        ///     <para>
        ///         Can be used in conjunction with:<br/>
        ///             <see cref="WithEndpoint" /><br/>
        ///             <see cref="WithAuthToken"/>
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     <i>Backend will add an http header to callbacks of the form:<br/>
        ///     <b>Authorization: Bearer [auth_token_provided]</b></i>
        /// </remarks>
        /// <returns>The builder <see cref="NotificationConfigBuilder"/></returns>
        public NotificationConfigBuilder WithAuthTypeBearer()
        {
            _authType = Constants.DocScanConstants.Bearer;
            return this;
        }

        /// <summary>
        ///     <para>
        ///         Sets the <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Authentication#authentication_schemes">HTTP Authentication Scheme</see> to "Basic" for callback requests to your endpoint.
        ///     </para>
        ///     <para>
        ///         The authorization token will be Base64 encoded by the backend when sent with notifications.
        ///     </para>
        ///     <para>
        ///         Can be used in conjunction with:<br/>
        ///             <see cref="WithEndpoint" /><br/>
        ///             <see cref="WithAuthToken"/>
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     <i>Backend will add an http header to callbacks of the form:<br/>
        ///     <b>Authorization: Basic [auth_token_provided]</b></i>
        /// </remarks>
        /// <returns>The builder <see cref="NotificationConfigBuilder"/></returns>
        public NotificationConfigBuilder WithAuthTypeBasic()
        {
            _authType = Constants.DocScanConstants.Basic;
            return this;
        }

        /// <summary>
        /// Sets the <paramref name="endpoint"/> that callback requests should be issued to.
        /// </summary>
        /// <remarks>
        ///     <i>Usually set to an absolute <see href="https://developer.mozilla.org/en-US/docs/Glossary/URL">URL</see> using the <see href="https://developer.mozilla.org/en-US/docs/Glossary/HTTPS">HTTPS</see> scheme (see <see href="https://datatracker.ietf.org/doc/html/rfc1738">rfc1738</see> and <see href="https://datatracker.ietf.org/doc/html/rfc2818">rfc2818</see>).</i><br/>
        /// </remarks>
        /// <param name="endpoint">The endpoint which callback requests from Yoti should be issued to for Notifications.</param>
        /// <returns>The builder</returns>
        public NotificationConfigBuilder WithEndpoint(string endpoint)
        {
            _endpoint = endpoint;
            return this;
        }

        /// <summary>
        /// Adds RESOURCE_UPDATE to the list of topics that trigger notification messages
        /// </summary>
        /// <returns>The builder</returns>
        public NotificationConfigBuilder ForResourceUpdate()
        {
            return WithTopic(Constants.DocScanConstants.ResourceUpdate);
        }

        /// <summary>
        /// Adds TASK_COMPLETION to the list of topics that trigger notification messages
        /// </summary>
        /// <returns>The builder</returns>
        public NotificationConfigBuilder ForTaskCompletion()
        {
            return WithTopic(Constants.DocScanConstants.TaskCompletion);
        }

        /// <summary>
        /// Adds CHECK_COMPLETION to the list of topics that trigger notification messages
        /// </summary>
        /// <returns>The builder</returns>
        public NotificationConfigBuilder ForCheckCompletion()
        {
            return WithTopic(Constants.DocScanConstants.CheckCompletion);
        }

        /// <summary>
        /// Adds SESSION_COMPLETION to the list of topics that trigger notification messages
        /// </summary>
        /// <returns>The builder</returns>
        public NotificationConfigBuilder ForSessionCompletion()
        {
            return WithTopic(Constants.DocScanConstants.SessionCompletion);
        }

        /// <summary>
        /// Adds CLIENT_SESSION_TOKEN_DELETED to the list of topics that trigger notification messages
        /// </summary>
        /// <returns>The builder</returns>
        public NotificationConfigBuilder ForClientSessionCompletion()
        {
            return WithTopic(Constants.DocScanConstants.ClientSessionTokenDeleted);
        }

        /// <summary>
        /// Adds a topic to the list of topics that trigger notification messages
        /// </summary>
        /// <param name="topicName">A topic to trigger notification messages</param>
        /// <returns>The builder</returns>
        public NotificationConfigBuilder WithTopic(string topicName)
        {
            if (!_topics.Contains(topicName))
                _topics.Add(topicName);

            return this;
        }

        /// <summary>
        /// Builds the <see cref="NotificationConfig"/> using the supplied values
        /// </summary>
        /// <returns>The built <see cref="NotificationConfig"/> object</returns>
        public NotificationConfig Build()
        {
            return new NotificationConfig(_authToken, _endpoint, _topics, _authType);
        }
    }
}