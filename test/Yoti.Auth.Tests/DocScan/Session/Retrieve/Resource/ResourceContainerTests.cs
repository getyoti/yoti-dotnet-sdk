using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Check;
using Yoti.Auth.DocScan.Session.Retrieve.Resource;

namespace Yoti.Auth.Tests.DocScan.Session.Retrieve.Resource
{
    [TestClass]
    public class ResourceContainerTests
    {
        [TestMethod]
        public void FilterForCheckShouldThrowWhenCheckResponseIsNull()
        {
            var resourceContainer = new ResourceContainer();

            Assert.ThrowsExactly<ArgumentNullException>(() => resourceContainer.FilterForCheck(null));
        }

        [TestMethod]
        public void FilterForCheckShouldHandleNullResourceLists()
        {
            var resourceContainer = new ResourceContainer();
            var checkResponse = new AuthenticityCheckResponse { ResourcesUsed = new List<string> { "some-resource-id" } };

            var result = resourceContainer.FilterForCheck(checkResponse);

            Assert.AreEqual(0, result.IdDocuments.Count);
            Assert.AreEqual(0, result.SupplementaryDocuments.Count);
            Assert.AreEqual(0, result.LivenessCapture.Count);
            Assert.AreEqual(0, result.FaceCapture.Count);
            Assert.AreEqual(0, result.ShareCodes.Count);
            Assert.AreEqual(0, result.ApplicantProfiles.Count);
        }

        [TestMethod]
        public void FilterForCheckShouldHandleNullResourcesUsed()
        {
            var resourceContainer = new ResourceContainer
            {
                IdDocuments = new List<IdDocumentResourceResponse> { new IdDocumentResourceResponse { Id = "id-document-1" } }
            };
            var checkResponse = new AuthenticityCheckResponse();

            var result = resourceContainer.FilterForCheck(checkResponse);

            Assert.AreEqual(0, result.IdDocuments.Count);
        }

        [TestMethod]
        public void FilterForCheckShouldReturnOnlyResourcesReferencedByCheck()
        {
            var idDocument1 = new IdDocumentResourceResponse { Id = "id-document-1" };
            var idDocument2 = new IdDocumentResourceResponse { Id = "id-document-2" };
            var supplementaryDocument1 = new SupplementaryDocResourceResponse { Id = "supplementary-document-1" };
            var supplementaryDocument2 = new SupplementaryDocResourceResponse { Id = "supplementary-document-2" };
            var liveness1 = new ZoomLivenessResourceResponse { Id = "liveness-1" };
            var liveness2 = new StaticLivenessResourceResponse { Id = "liveness-2" };
            var faceCapture1 = new FaceCaptureResourceResponse { Id = "face-capture-1" };
            var faceCapture2 = new FaceCaptureResourceResponse { Id = "face-capture-2" };
            var shareCode1 = new ShareCodeResourceResponse { Id = "share-code-1" };
            var shareCode2 = new ShareCodeResourceResponse { Id = "share-code-2" };
            var applicantProfile1 = new ApplicantProfileResourceResponse { Id = "applicant-profile-1" };
            var applicantProfile2 = new ApplicantProfileResourceResponse { Id = "applicant-profile-2" };

            var resourceContainer = new ResourceContainer
            {
                IdDocuments = new List<IdDocumentResourceResponse> { idDocument1, idDocument2 },
                SupplementaryDocuments = new List<SupplementaryDocResourceResponse> { supplementaryDocument1, supplementaryDocument2 },
                LivenessCapture = new List<LivenessResourceResponse> { liveness1, liveness2 },
                FaceCapture = new List<FaceCaptureResourceResponse> { faceCapture1, faceCapture2 },
                ShareCodes = new List<ShareCodeResourceResponse> { shareCode1, shareCode2 },
                ApplicantProfiles = new List<ApplicantProfileResourceResponse> { applicantProfile1, applicantProfile2 }
            };

            var checkResponse = new AuthenticityCheckResponse
            {
                ResourcesUsed = new List<string>
                {
                    "id-document-1",
                    "supplementary-document-1",
                    "liveness-1",
                    "face-capture-1",
                    "share-code-1",
                    "applicant-profile-1"
                }
            };

            var result = resourceContainer.FilterForCheck(checkResponse);

            CollectionAssert.AreEqual(new[] { idDocument1 }, result.IdDocuments);
            CollectionAssert.AreEqual(new[] { supplementaryDocument1 }, result.SupplementaryDocuments);
            CollectionAssert.AreEqual(new[] { liveness1 }, result.LivenessCapture);
            CollectionAssert.AreEqual(new[] { faceCapture1 }, result.FaceCapture);
            CollectionAssert.AreEqual(new[] { shareCode1 }, result.ShareCodes);
            CollectionAssert.AreEqual(new[] { applicantProfile1 }, result.ApplicantProfiles);
        }
    }
}
