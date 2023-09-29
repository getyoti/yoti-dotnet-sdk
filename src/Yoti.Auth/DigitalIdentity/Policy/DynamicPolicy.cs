using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.DigitalIdentity.Policy
{
    /// <summary>
    /// Set of data required to request a sharing transaction
    /// </summary>
    public class DynamicPolicy
    {
        internal const int SelfieAuthType = 1;
        internal const int PinAuthType = 2;

        [JsonProperty(PropertyName = "wanted")]
        private readonly ICollection<WantedAttribute> _wantedAttributes;

        [JsonProperty(PropertyName = "wanted_auth_types")]
        private readonly HashSet<int> _wantedAuthTypes;

        [JsonProperty(PropertyName = "wanted_remember_me")]
        private readonly bool _wantedRememberMeId;

#pragma warning disable 0414 //"Value never used" warning: the JsonProperty is used when creating the DynamicPolicy JSON

        [JsonProperty(PropertyName = "wanted_remember_me_optional")]
        private readonly bool _isWantedRememberMeIdOptional;

#pragma warning restore 0414

        [JsonProperty(PropertyName = "identity_profile_requirements")]
        private readonly object _identityProfileRequirements;

        public DynamicPolicy(
                 ICollection<WantedAttribute> wantedAttributes,
                 HashSet<int> wantedAuthTypes,
                 bool wantedRememberMeId,
                 object identityProfileRequirements = null
            )
        {
            _wantedAttributes = wantedAttributes;
            _wantedAuthTypes = wantedAuthTypes;
            _wantedRememberMeId = wantedRememberMeId;
            _isWantedRememberMeIdOptional = false;
            _identityProfileRequirements = identityProfileRequirements;
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
        public HashSet<int> WantedAuthTypes
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

        /// <summary>
        /// IdentityProfileRequirements requested in the policy
        /// </summary>
        [JsonIgnore]
        public object IdentityProfileRequirements
        {
            get
            {
                return _identityProfileRequirements;
            }
        }
    }
}