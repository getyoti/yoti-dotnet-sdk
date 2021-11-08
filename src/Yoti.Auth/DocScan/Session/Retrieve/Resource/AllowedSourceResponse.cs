using JsonSubTypes;
using Newtonsoft.Json;
using Yoti.Auth.Constants;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Source;

namespace Yoti.Auth.DocScan.Session.Retrieve.Resource
{
    [JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(EndUserAllowedSourceResponse), DocScanConstants.EndUser)]
    [JsonSubtypes.KnownSubType(typeof(RelyingBusinessAllowedSourceResponse), DocScanConstants.RelyingBusiness)]
    [JsonSubtypes.KnownSubType(typeof(IbvAllowedSourceResponse), DocScanConstants.Ibv)]
    [JsonSubtypes.FallBackSubType(typeof(UnknownAllowedSourceResponse))]
    public abstract class AllowedSourceResponse
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; private set; }
    }
}