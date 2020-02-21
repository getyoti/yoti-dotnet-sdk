using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Create
{
    public class NotificationConfigBuilder
    {
        private readonly List<string> _topics = new List<string>();
        private string _authToken;
        private string _endpoint;

        /// <summary>
        /// Sets the authorization token to be included in call-back messages
        /// </summary>
        /// <param name="authToken">the authorization token</param>
        /// <returns>The builder</returns>
        public NotificationConfigBuilder WithAuthToken(string authToken)
        {
            _authToken = authToken;
            return this;
        }

        /// <summary>
        /// Sets the endpoint that notifications should be sent to
        /// </summary>
        /// <param name="endpoint">the endpoint</param>
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
        /// Adds a topic to the list of topics that trigger notification messages
        /// </summary>
        /// <param name="topicName"></param>
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
            return new NotificationConfig(_authToken, _endpoint, _topics);
        }
    }
}