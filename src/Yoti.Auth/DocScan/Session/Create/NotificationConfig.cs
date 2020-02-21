using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create
{
    /// <summary>
    /// Represents the configuration properties for notifications within the Doc Scan system.
    /// Notifications can be configured within a Doc Scan session to allow the clients backend to be
    /// notified of certain events, without having to constantly poll for the state of a session.
    /// </summary>
    public class NotificationConfig
    {
        public NotificationConfig(string authToken, string endpoint, List<string> topics)
        {
            AuthToken = authToken;
            Endpoint = endpoint;
            Topics = topics;
        }

        [JsonProperty(PropertyName = "auth_token")]
        public string AuthToken { get; }

        [JsonProperty(PropertyName = "endpoint")]
        public string Endpoint { get; }

        [JsonProperty(PropertyName = "topics")]
        public List<string> Topics { get; }
    }
}