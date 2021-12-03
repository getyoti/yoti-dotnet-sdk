namespace Yoti.Auth.DocScan.Session.Create.FaceCapture
{
    public class UploadFaceCaptureImagePayload
    {
        public string ImageContentType { get; }
        public byte[] ImageContents { get; }

        public UploadFaceCaptureImagePayload(string imageContentType, byte[] imageContents)
        {
            ImageContentType = imageContentType;
            ImageContents = imageContents;
        }
    }
}