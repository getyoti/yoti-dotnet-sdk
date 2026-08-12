using Newtonsoft.Json;
using System.Collections.Generic;
using Yoti.DigitalIdentity;

namespace Yoti.Auth.DigitalIdentity
{
    public class ErrorReason
    {
        [JsonProperty(PropertyName = "requirements_not_met_details")]
        public List<RequirementNotMetDetails> RequirementsNotMetDetails { get; private set; }
    }
}
