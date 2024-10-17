using Newtonsoft.Json;
using System.Collections.Generic;
using Yoti.Auth.DigitalIdentity;
using Yoti.DigitalIdentity;

namespace Yoti.Auth.DigitalIdentity
{
    public class ErrorReason
    {
        [JsonProperty("requirements_not_met_details")]
        public List<RequirementNotMetDetails> RequirementNotMetDetails { get; private set; }
        
    }
    
}
