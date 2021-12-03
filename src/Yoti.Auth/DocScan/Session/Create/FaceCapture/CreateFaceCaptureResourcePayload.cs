using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.FaceCapture
{
    public class CreateFaceCaptureResourcePayload
    {
        public CreateFaceCaptureResourcePayload(string requirementId)
        {
            RequirementId = requirementId;
        }

        [JsonProperty(PropertyName = "requirement_id")]
        public string RequirementId { get; }
    }
}