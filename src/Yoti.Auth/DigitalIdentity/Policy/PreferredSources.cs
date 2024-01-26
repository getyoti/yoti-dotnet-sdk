using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.DigitalIdentity.Policy
{
    public class PreferredSources
    {
        [JsonProperty(PropertyName = "anchors")]
        public List<WantedAnchor> WantedAnchors { get; private set; }

        [JsonProperty(PropertyName = "soft_preference")]
        public bool SoftPreference { get; private set; }

        public PreferredSources(List<WantedAnchor> wantedAnchors, bool softPreference = false)
        {
            WantedAnchors = wantedAnchors;
            SoftPreference = softPreference;
        }
    }
}