using Newtonsoft.Json;
using Yoti.DigitalIdentity;

namespace Yoti.Auth.DigitalIdentity
{
    public class ErrorReason
    {
        [JsonProperty(PropertyName = "requirements_not_met_details")]
        public RequirementNotMetDetails RequirementNotMetDetails { get; private set; }
    }
}
