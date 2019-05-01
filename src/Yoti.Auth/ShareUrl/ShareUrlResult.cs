using System;
using Newtonsoft.Json;

namespace Yoti.Auth.ShareUrl
{
    public class ShareUrlResult
    {
#pragma warning disable 0649

        // These fields are assigned to by JSON deserialization
        [JsonProperty(PropertyName = "qrcode")]
        private readonly string _shareUrl;

        [JsonProperty(PropertyName = "ref_id")]
        private readonly string _refId;

#pragma warning restore 0649

        /// <summary>
        /// The URL that the 3rd party should use for the share
        /// </summary>
        public Uri Url
        {
            get
            {
                return new Uri(_shareUrl);
            }
        }

        /// <summary>
        /// Get the Yoti reference id for the share
        /// </summary>
        public string RefId
        {
            get
            {
                return _refId;
            }
        }
    }
}