using Newtonsoft.Json;

namespace Yoti.Auth.DataObjects
{
    internal class ReceiptDO
    {
        [JsonProperty(PropertyName = "receipt_id")]
        public string ReceiptID { get; set; }

        [JsonProperty(PropertyName = "other_party_profile_content")]
        public string OtherPartyProfileContent { get; set; }

        [JsonProperty(PropertyName = "profile_content")]
        public string ProfileContent { get; set; }

        [JsonProperty(PropertyName = "other_party_extra_data_content")]
        public string OtherPartyExtraDataContent { get; set; }

        [JsonProperty(PropertyName = "extra_data_content")]
        public string ExtraDataContent { get; set; }

        [JsonProperty(PropertyName = "wrapped_receipt_key")]
        public string WrappedReceiptKey { get; set; }

        [JsonProperty(PropertyName = "policy_uri")]
        public string PolicyURI { get; set; }

        [JsonProperty(PropertyName = "personal_key")]
        public string PersonalKey { get; set; }

        [JsonProperty(PropertyName = "remember_me_id")]
        public string RememberMeID { get; set; }

        [JsonProperty(PropertyName = "parent_remember_me_id")]
        public string ParentRememberMeID { get; set; }

        [JsonProperty(PropertyName = "sharing_outcome")]
        public string SharingOutcome { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public string Timestamp { get; set; }
    }
}