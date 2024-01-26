using Newtonsoft.Json;

namespace Yoti.Auth.DigitalIdentity.Policy
{
    public class Constraint
    {
        [JsonRequired]
        [JsonProperty(PropertyName = "type")]
        public string ConstraintType { get; private set; }

        public Constraint(string constraintType)
        {
            ConstraintType = constraintType;
        }
    }
}