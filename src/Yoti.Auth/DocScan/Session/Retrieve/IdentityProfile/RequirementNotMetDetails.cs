using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.IdentityProfile;

public class RequirementNotMetDetails
{
    [JsonProperty(PropertyName = "failure_type")]
    public string FailureType { get; private set; }
    [JsonProperty(PropertyName = "document_type")]
    public string DocumentType { get; private set; }
    [JsonProperty(PropertyName = "document_country_iso_code")]
    public string DocumentCountryIsoCode { get; private set; }
    [JsonProperty(PropertyName = "audit_id")]
    public string AuditId { get; private set; }
    [JsonProperty(PropertyName = "details")]
    public string Details { get; private set; }
}
