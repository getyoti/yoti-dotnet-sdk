using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Yoti.Auth.Sandbox.Profile.Request.Attribute;

namespace Yoti.Auth.Sandbox.Profile.Request
{
    public class YotiTokenRequest
    {
        public YotiTokenRequest(string rememberMeId, ReadOnlyCollection<SandboxAttribute> sandboxAttributes)
        {
            RememberMeId = rememberMeId;
            SandboxAttributes = sandboxAttributes;
        }

        [JsonProperty(PropertyName = "remember_me_id")]
        public string RememberMeId { get; private set; }

        [JsonProperty(PropertyName = "profile_attributes")]
        public ReadOnlyCollection<SandboxAttribute> SandboxAttributes { get; }

        public static YotiTokenRequestBuilder Builder()
        {
            return new YotiTokenRequestBuilder();
        }
    }
}