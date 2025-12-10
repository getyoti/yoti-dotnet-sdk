using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.DigitalIdentity.Resource
{
    /// <summary>
    /// Container for resources in a Digital Identity session
    /// </summary>
    public class DigitalIdentityResourceContainer
    {
        /// <summary>
        /// List of share code resources
        /// </summary>
        [JsonProperty(PropertyName = "share_codes")]
        public List<ShareCodeResource> ShareCodes { get; internal set; }
    }
}
