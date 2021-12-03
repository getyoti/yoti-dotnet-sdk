using JsonSubTypes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Yoti.Auth.Constants;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Document;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.FaceCapture;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Liveness;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Source;
using Yoti.Auth.DocScan.Session.Retrieve.Resource;

namespace Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture
{
    [JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(RequiredIdDocumentResourceResponse), DocScanConstants.IdDocument)]
    [JsonSubtypes.KnownSubType(typeof(RequiredSupplementaryDocumentResourceResponse), DocScanConstants.SupplementaryDocument)]
    [JsonSubtypes.KnownSubType(typeof(RequiredLivenessResourceResponse), DocScanConstants.Liveness)]
    [JsonSubtypes.KnownSubType(typeof(RequiredFaceCaptureResourceResponse), DocScanConstants.FaceCapture)]
    [JsonSubtypes.FallBackSubType(typeof(UnknownRequiredResourceResponse))]
    public abstract class RequiredResourceResponse
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; internal set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; internal set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; internal set; }

        [JsonProperty(PropertyName = "allowed_sources")]
        public List<AllowedSourceResponse> AllowedSources { get; internal set; }

        /// <summary>
        /// Is the Relying Business allowed to upload resources to satisfy the requirement
        /// </summary>
        public bool IsRelyingBusinessAllowed => AllowedSources != null && AllowedSources.OfType<RelyingBusinessAllowedSourceResponse>().Any();
    }
}