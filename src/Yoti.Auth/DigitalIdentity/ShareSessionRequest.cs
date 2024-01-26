using System.Collections.Generic;
using Newtonsoft.Json;
using Yoti.Auth.DigitalIdentity.Extensions;
using Yoti.Auth.DigitalIdentity.Policy;

namespace Yoti.Auth.DigitalIdentity
{
    public class ShareSessionRequest
    {
        
        [JsonProperty(PropertyName = "policy")]
        private readonly Policy.Policy _dynamicPolicy;

        [JsonProperty(PropertyName = "extensions")]
        private readonly List<BaseExtension> _extensions;

        [JsonProperty(PropertyName = "subject")]
        private readonly object _subject;

        [JsonProperty(PropertyName = "redirectUri")]
        public string _redirectUri { get; set; } 

        [JsonProperty(PropertyName = "notification")]
        public Notification _notification { get; set; } 

      

        [JsonIgnore]
        public Policy.Policy DynamicPolicy
        {
            get
            {
                return _dynamicPolicy;
            }
        }

        [JsonIgnore]
        public List<BaseExtension> Extensions
        {
            get
            {
                return _extensions;
            }
        }


        [JsonIgnore]
        public object Subject
        {
            get
            {
                return _subject;
            }
        }

        [JsonIgnore]
        public string RedirectUri
        {
            get
            {
                return _redirectUri;
            }
        }

        [JsonIgnore]
        public Notification Notification
        {
            get
            {
                return _notification;
            }
        }

        public ShareSessionRequest(Policy.Policy dynamicPolicy, string redirectUri, Notification notification = null, List<BaseExtension> extensions = null, object subject = null)
        {
            _redirectUri = redirectUri;
            _notification = notification;
            _dynamicPolicy = dynamicPolicy;
            _extensions = extensions ?? new List<BaseExtension>();
            _subject = subject;
        }
    }
}