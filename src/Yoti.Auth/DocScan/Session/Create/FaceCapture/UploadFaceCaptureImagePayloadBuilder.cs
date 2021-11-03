using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.FaceCapture
{
    public class UploadFaceCaptureImagePayloadBuilder
    {
        private string _imageContentType;
        private byte[] _imageContents;

        /// <summary>
        /// Sets the content type for uploading a JPEG image
        /// </summary>
        /// <returns>The <see cref="UploadFaceCaptureImagePayloadBuilder"/></returns>
        public UploadFaceCaptureImagePayloadBuilder ForJpeg()
        {
            _imageContentType = DocScanConstants.MimeTypeJpg;
            return this;
        }

        /// <summary>
        /// Sets the content type for uploading a PNG image
        /// </summary>
        /// <returns>The <see cref="UploadFaceCaptureImagePayloadBuilder"/></returns>
        public UploadFaceCaptureImagePayloadBuilder ForPng()
        {
            _imageContentType = DocScanConstants.MimeTypePng;
            return this;
        }

        /// <summary>
        /// Sets the contents of the image to be uploaded
        /// </summary>
        /// <param name="imageContents"></param>
        /// <returns>The <see cref="UploadFaceCaptureImagePayloadBuilder"/></returns>
        public UploadFaceCaptureImagePayloadBuilder WithImageContents(byte[] imageContents)
        {
            _imageContents = imageContents;
            return this;
        }

        /// <summary>
        /// Creates instance of <see cref="UploadFaceCaptureImagePayload"/> with the content type and contents provided
        /// </summary>
        /// <returns>The <see cref="UploadFaceCaptureImagePayload"/></returns>
        public UploadFaceCaptureImagePayload Build()
        {
            return new UploadFaceCaptureImagePayload(_imageContentType, _imageContents);
        }
    }
}