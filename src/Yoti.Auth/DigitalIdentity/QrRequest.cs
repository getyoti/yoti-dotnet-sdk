using System.Collections.Generic;
using Newtonsoft.Json;
using Yoti.Auth.DigitalIdentity.Extensions;
using Yoti.Auth.DigitalIdentity.Policy;

namespace Yoti.Auth.DigitalIdentity
{
    public class QrRequest
    {
        [JsonProperty(PropertyName = "transport", NullValueHandling = NullValueHandling.Ignore)]
        private readonly string _transport;

        [JsonProperty(PropertyName = "displayMode", NullValueHandling = NullValueHandling.Ignore)]
        private readonly string _displayMode;

        [JsonIgnore]
        public string DisplayMode
        {
            get
            {
                return _displayMode;
            }
        }

        [JsonIgnore]
        public string Transport
        {
            get
            {
                return _transport;
            }
        }

        public QrRequest(string transport = null, string displayMode = null)
        {
            _transport = transport;
            _displayMode = displayMode;
        }
    }
}