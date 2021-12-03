namespace Yoti.Auth.DocScan.Session.Create.FaceCapture
{
    public class CreateFaceCaptureResourcePayloadBuilder
    {
        private string _requirementId;

        public CreateFaceCaptureResourcePayloadBuilder WithRequirementId(string requirementId)
        {
            _requirementId = requirementId;
            return this;
        }

        public CreateFaceCaptureResourcePayload Build()
        {
            return new CreateFaceCaptureResourcePayload(_requirementId);
        }
    }
}