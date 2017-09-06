namespace Yoti.Auth.DataObjects
{
    internal class ReceiptDO
    {
        public string receipt_id { get; set; }
        public string other_party_profile_content { get; set; }
        public string profile_content { get; set; }
        public string other_party_extra_data_content { get; set; }
        public string extra_data_content { get; set; }
        public string wrapped_receipt_key { get; set; }
        public string policy_uri { get; set; }
        public string personal_key { get; set; }
        public string remember_me_id { get; set; }
        public string sharing_outcome { get; set; }
        public string timestamp { get; set; }
    }
}