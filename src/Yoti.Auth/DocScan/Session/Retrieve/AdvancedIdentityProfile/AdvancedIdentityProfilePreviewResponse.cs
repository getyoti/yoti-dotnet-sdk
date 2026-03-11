using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Retrieve.AdvancedIdentityProfilePreview
{
    public class AdvancedIdentityProfilePreviewResponse
    {
        [JsonProperty(PropertyName = "media")]
        public MediaResponse Media { get; private set; }
    }
}
