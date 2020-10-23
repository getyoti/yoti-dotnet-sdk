﻿using JsonSubTypes;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Task
{
    /// <summary>
    /// Represents a check response that has been generated by the session
    /// </summary>
    [JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(GeneratedTextDataCheckResponse), Constants.DocScanConstants.IdDocumentTextDataCheck)]
    [JsonSubtypes.KnownSubType(typeof(GeneratedSupplementaryDocTextDataCheckResponse), Constants.DocScanConstants.SupplementaryDocumentTextDataCheck)]
    public class GeneratedCheckResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; internal set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; internal set; }
    }
}