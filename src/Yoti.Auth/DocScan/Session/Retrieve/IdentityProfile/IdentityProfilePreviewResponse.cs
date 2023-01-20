using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Retrieve.IdentityProfilePreview
{
    public class IdentityProfilePreviewResponse
    {
        [JsonProperty(PropertyName = "media")]
        public MediaResponse Media { get; private set; }
    }
}
