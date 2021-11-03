using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Create.FaceCapture;

namespace Yoti.Auth.Tests.DocScan.Session.Create.FaceCapture
{
    [TestClass]
    public class CreateFaceCaptureResourcePayloadBuilderTests
    {
        private const string _someRequirementId = "someRequirementId";

        [TestMethod]
        public void ShouldBuildWithRequirementId()
        {
            CreateFaceCaptureResourcePayload result = new CreateFaceCaptureResourcePayloadBuilder()
                    .WithRequirementId(_someRequirementId)
                    .Build();

            Assert.AreEqual(_someRequirementId, result.RequirementId);
        }
    }
}