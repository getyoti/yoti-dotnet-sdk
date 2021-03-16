using System;
using System.Collections.Generic;
using JsonSubTypes;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Check
{
    [JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(AuthenticityCheckResponse), Constants.DocScanConstants.IdDocumentAuthenticity)]
    [JsonSubtypes.KnownSubType(typeof(FaceMatchCheckResponse), Constants.DocScanConstants.IdDocumentFaceMatch)]
    [JsonSubtypes.KnownSubType(typeof(TextDataCheckResponse), Constants.DocScanConstants.IdDocumentTextDataCheck)]
    [JsonSubtypes.KnownSubType(typeof(LivenessCheckResponse), Constants.DocScanConstants.Liveness)]
    [JsonSubtypes.KnownSubType(typeof(IdDocumentComparisonCheckResponse), Constants.DocScanConstants.IdDocumentComparison)]
    [JsonSubtypes.KnownSubType(typeof(SupplementaryDocTextDataCheckResponse), Constants.DocScanConstants.SupplementaryDocumentTextDataCheck)]
    [JsonSubtypes.KnownSubType(typeof(ThirdPartyIdentityCheckResponse), Constants.DocScanConstants.ThirdPartyIdentity)]
    public class CheckResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; internal set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; internal set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; internal set; }

        [JsonProperty(PropertyName = "resources_used")]
        public List<string> ResourcesUsed { get; internal set; }

        [JsonProperty(PropertyName = "generated_media")]
        public List<GeneratedMedia> GeneratedMedia { get; internal set; }

        [JsonProperty(PropertyName = "report")]
        public ReportResponse Report { get; internal set; }

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; internal set; }

        [JsonProperty(PropertyName = "last_updated")]
        public DateTime LastUpdated { get; internal set; }
    }
}