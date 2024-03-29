﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedWatchlistScreeningConfig : RequestedCheckConfig
    {
        [JsonProperty(PropertyName = "categories")]
        public List<string> Categories { get; }

        public RequestedWatchlistScreeningConfig(List<string> categories)
        {
            Categories = categories;
        }
    }
}