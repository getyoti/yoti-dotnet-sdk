using System.Collections.Generic;
using Newtonsoft.Json;
using Yoti.Auth.ShareUrl.Extensions;
using Yoti.Auth.ShareUrl.Policy;

namespace Yoti.Auth.ShareUrl
{
    public class DynamicScenario
    {
        [JsonProperty(PropertyName = "callback_endpoint")]
        private readonly string _callbackEndpoint;

        [JsonProperty(PropertyName = "policy")]
        private readonly DynamicPolicy _dynamicPolicy;

        [JsonProperty(PropertyName = "extensions")]
        private readonly List<BaseExtension> _extensions;

        [JsonIgnore]
        public string CallbackEndpoint
        {
            get
            {
                return _callbackEndpoint;
            }
        }

        [JsonIgnore]
        public DynamicPolicy DynamicPolicy
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

        public DynamicScenario(string callbackEndpoint, DynamicPolicy dynamicPolicy, List<BaseExtension> extensions = null)
        {
            _callbackEndpoint = callbackEndpoint;
            _dynamicPolicy = dynamicPolicy;
            _extensions = extensions ?? new List<BaseExtension>();
        }
    }
}