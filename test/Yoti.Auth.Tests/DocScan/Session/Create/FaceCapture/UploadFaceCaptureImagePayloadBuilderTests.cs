using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Constants;
using Yoti.Auth.DocScan.Session.Create.FaceCapture;

namespace Yoti.Auth.Tests.DocScan.Session.Create.FaceCapture
{
    [TestClass]
    public class UploadFaceCaptureImagePayloadBuilderTests
    {
        private static byte[] _someImageContents = new byte[] { 0x00, 0x21, 0x60, 0x1F, 0xA1 };

        [TestMethod]
        public void ShouldBuildWithImageContentsForJpeg()
        {
            UploadFaceCaptureImagePayload result = new UploadFaceCaptureImagePayloadBuilder()
                    .WithImageContents(_someImageContents)
                    .ForJpeg()
                    .Build();

            CollectionAssert.AreEqual(_someImageContents, result.ImageContents);
            Assert.AreEqual(DocScanConstants.MimeTypeJpg, result.ImageContentType);
        }

        [TestMethod]
        public void ShouldBuildWithImageContentsForPng()
        {
            UploadFaceCaptureImagePayload result = new UploadFaceCaptureImagePayloadBuilder()
                    .WithImageContents(_someImageContents)
                    .ForPng()
                    .Build();

            CollectionAssert.AreEqual(_someImageContents, result.ImageContents);
            Assert.AreEqual(DocScanConstants.MimeTypePng, result.ImageContentType);
        }
    }
}