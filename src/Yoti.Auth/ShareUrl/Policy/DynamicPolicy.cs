using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.ShareUrl.Policy
{
    /// <summary>
    /// Set of data required to request a sharing transaction
    /// </summary>
    public class DynamicPolicy
    {
        [JsonProperty(PropertyName = "wanted")]
        private readonly ICollection<WantedAttribute> _wantedAttributes;

        [JsonProperty(PropertyName = "wanted_auth_types")]
        private readonly List<int> _wantedAuthTypes;

        [JsonProperty(PropertyName = "wanted_remember_me")]
        private readonly bool _wantedRememberMeId;

#pragma warning disable 0414 //"Value never used" warning: the JsonProperty is used when creating the DynamicPolicy JSON

        [JsonProperty(PropertyName = "wanted_remember_me_optional")]
        private readonly bool _isWantedRememberMeIdOptional;

#pragma warning restore 0414

        public DynamicPolicy(
            ICollection<WantedAttribute> wantedAttributes,
            List<int> wantedAuthTypes,
            bool wantedRememberMeId)
        {
            _wantedAttributes = wantedAttributes;
            _wantedAuthTypes = wantedAuthTypes;
            _wantedRememberMeId = wantedRememberMeId;
            _isWantedRememberMeIdOptional = false;
        }

        /// <summary>
        /// Set of required <see cref="WantedAttribute"/>
        /// </summary>
        [JsonIgnore]
        public ICollection<WantedAttribute> WantedAttributes
        {
            get
            {
                return _wantedAttributes;
            }
        }

        /// <summary>
        /// Type of authentications
        /// </summary>
        [JsonIgnore]
        public List<int> WantedAuthTypes
        {
            get
            {
                return _wantedAuthTypes;
            }
        }

        /// <summary>
        /// Is RememberMeId wanted in the policy
        /// </summary>
        [JsonIgnore]
        public bool WantedRememberMeId
        {
            get
            {
                return _wantedRememberMeId;
            }
        }
    }
}