using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create
{
    /// <summary>
    /// Specifies which sections to include when creating a session overview PDF.
    /// Any option left unset (null) is omitted from the request, leaving the
    /// decision to the backend rather than the SDK.
    /// </summary>
    public class SessionOverviewPdfPayload
    {
        public SessionOverviewPdfPayload(bool? includeImages = null, bool? includeExtractedText = null, bool? includeDeviceTracking = null)
        {
            IncludeImages = includeImages;
            IncludeExtractedText = includeExtractedText;
            IncludeDeviceTracking = includeDeviceTracking;
        }

        [JsonProperty(PropertyName = "include_images", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IncludeImages { get; }

        [JsonProperty(PropertyName = "include_extracted_text", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IncludeExtractedText { get; }

        [JsonProperty(PropertyName = "include_device_tracking", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IncludeDeviceTracking { get; }
    }
}
