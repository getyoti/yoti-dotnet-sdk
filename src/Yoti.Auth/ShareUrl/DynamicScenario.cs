﻿using System.Collections.Generic;
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

        [JsonProperty(PropertyName = "subject")]
        private readonly object _subject;

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


        [JsonIgnore]
        public object Subject
        {
            get
            {
                return _subject;
            }
        }

        public DynamicScenario(string callbackEndpoint, DynamicPolicy dynamicPolicy, List<BaseExtension> extensions = null, object subject = null)
        {
            _callbackEndpoint = callbackEndpoint;
            _dynamicPolicy = dynamicPolicy;
            _extensions = extensions ?? new List<BaseExtension>();
            _subject = subject;
        }
    }
}