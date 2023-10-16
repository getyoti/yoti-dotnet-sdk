using System.Collections.Generic;
using Newtonsoft.Json;
using Yoti.Auth.DigitalIdentity.Extensions;
using Yoti.Auth.DigitalIdentity.Policy;

namespace Yoti.Auth.DigitalIdentity
{
    public class ShareSessionRequest
    {
        
        [JsonProperty(PropertyName = "transport")]
        private readonly string _transport;

        [JsonProperty(PropertyName = "displayMode")]
        private readonly string _displayMode;

        [JsonIgnore]
        public string Transport
        {
            get
            {
                return _transport;
            }
        }

        [JsonIgnore]
        public string DisplayMode
        {
            get
            {
                return _displayMode;
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