using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.Constants;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Check;

namespace Yoti.Auth.Tests.Docs.Session.Retrieve.Check
{
    [TestClass]
    public class CheckResponseTests
    {
        [DataTestMethod]
        [DataRow(DocScanConstants.IdDocumentAuthenticity, typeof(AuthenticityCheckResponse))]
        [DataRow(DocScanConstants.IdDocumentFaceMatch, typeof(FaceMatchCheckResponse))]
        [DataRow(DocScanConstants.IdDocumentTextDataCheck, typeof(TextDataCheckResponse))]
        [DataRow(DocScanConstants.Liveness, typeof(LivenessCheckResponse))]
        [DataRow(DocScanConstants.IdDocumentComparison, typeof(IdDocumentComparisonCheckResponse))]
        [DataRow(DocScanConstants.SupplementaryDocumentTextDataCheck, typeof(SupplementaryDocTextDataCheckResponse))]
        [DataRow(DocScanConstants.ThirdPartyIdentity, typeof(ThirdPartyIdentityCheckResponse))]
        [DataRow("OTHER", typeof(CheckResponse))]
        [DataRow("", typeof(CheckResponse))]
        [DataRow(null, typeof(CheckResponse))]
        public void CheckResponsesAreParsed(string checkResponsetypeString, Type expectedType)
        {
            var checks = new List<CheckResponse>
            {
                new CheckResponse
                {
                    Type = checkResponsetypeString
                }
            };

            var initialGetSessionResult = new GetSessionResult
            {
                Checks = checks
            };

            string json = JsonConvert.SerializeObject(initialGetSessionResult);

            GetSessionResult getSessionResult =
                JsonConvert.DeserializeObject<GetSessionResult>(json);

            Assert.IsInstanceOfType(getSessionResult.Checks.Single(), expectedType);
        }

        [TestMethod]
        public void CheckGeneratedMediaIsParsed()
        {
            dynamic generatedMediaResponse = new
            {
                id = "ca492333-35bf-4cc4-a87a-e4af67c30e67",
                type = "IMAGE"
            };

            string json = JsonConvert.SerializeObject(generatedMediaResponse);
            GeneratedMedia response = JsonConvert.DeserializeObject<GeneratedMedia>(json);

            Assert.AreEqual(generatedMediaResponse.id, response.Id);
            Assert.AreEqual(generatedMediaResponse.type, response.Type);
        }

        [TestMethod]
        public void CheckRecommendationResponseIsParsed()
        {
            dynamic recommendationResponse = new
            {
                value = "NOT_AVAILABLE",
                reason = "PICTURE_TOO_DARK",
                recovery_suggestion = "BETTER_LIGHTING"
            };

            string json = JsonConvert.SerializeObject(recommendationResponse);
            RecommendationResponse response =
                JsonConvert.DeserializeObject<RecommendationResponse>(json);

            Assert.AreEqual("NOT_AVAILABLE", response.Value);
            Assert.AreEqual("PICTURE_TOO_DARK", response.Reason);
            Assert.AreEqual("BETTER_LIGHTING", response.RecoverySuggestion);
        }

        [TestMethod]
        public void CheckBreakdownResponseIsParsed()
        {
            dynamic breakdownResponse = new
            {
                sub_check = "issuing_authority_verification",
                result = "PASS",
                details = new List<dynamic> {
                    new { name = "n1", value = "v1" },
                    new { name = "n2", value = "v2" }
                }
            };

            string json = JsonConvert.SerializeObject(breakdownResponse);
            BreakdownResponse response =
                JsonConvert.DeserializeObject<BreakdownResponse>(json);

            Assert.AreEqual("issuing_authority_verification", response.SubCheck);
            Assert.AreEqual("PASS", response.Result);
            Assert.AreEqual("n1", response.Details.First().Name);
            Assert.AreEqual("v1", response.Details.First().Value);
            Assert.AreEqual("n2", response.Details.Last().Name);
            Assert.AreEqual("v2", response.Details.Last().Value);
        }

        [DataTestMethod]
        [DataRow(typeof(DocumentFieldsResponse))]
        [DataRow(typeof(DocumentIdPhotoResponse))]
        [DataRow(typeof(FaceMapResponse))]
        [DataRow(typeof(FileResponse))]
        [DataRow(typeof(FrameResponse))]
        [DataRow(typeof(PageResponse))]
        public void CheckMediaResponsesAreParsed(Type requiredType)
        {
            dynamic mediaResponse = GetMediaResponse();

            var mi = typeof(CheckResponseTests).GetMethod(nameof(GetMediaResponseOfType));
            var method = mi.MakeGenericMethod(requiredType);

            var resp = method.Invoke(new CheckResponseTests(), new object[] { mediaResponse });
            IResponseWithMediaProperty response = resp as IResponseWithMediaProperty;

            AssertMediaValuesCorrect(mediaResponse, response, requiredType);
        }

        [TestMethod]
        public void CheckZoomLivenessResourceResponseIsParsed()
        {
            dynamic zoomLivenessResourceResponse = new
            {
                facemap = new { media = GetMediaResponse() },
                frames = new List<dynamic> {
                    {
                        new { media = GetMediaResponse() }
                    }
                }
            };

            string json = JsonConvert.SerializeObject(zoomLivenessResourceResponse);
            ZoomLivenessResourceResponse response =
                JsonConvert.DeserializeObject<ZoomLivenessResourceResponse>(json);

            AssertMediaValuesCorrect(zoomLivenessResourceResponse.facemap.media, response.FaceMap, typeof(FaceMapResponse));
            AssertMediaValuesCorrect((zoomLivenessResourceResponse.frames as IEnumerable<dynamic>).First().media, response.Frames.First(), typeof(FrameResponse));
        }

        [TestMethod]
        public void CheckPageResponseIsParsed()
        {
            dynamic pageResponse = new
            {
                capture_method = "CAMERA",
                media = GetMediaResponse(),
                frames = new List<dynamic> {
                    {
                        new { media = GetMediaResponse() }
                    }
                }
            };

            string json = JsonConvert.SerializeObject(pageResponse);
            PageResponse response =
                JsonConvert.DeserializeObject<PageResponse>(json);

            AssertMediaValuesCorrect(pageResponse.media, response, typeof(PageResponse));
            AssertMediaValuesCorrect((pageResponse.frames as IEnumerable<dynamic>).First().media, response.Frames.First(), typeof(FrameResponse));
        }

        public TResponseTypeWithMedia GetMediaResponseOfType<TResponseTypeWithMedia>(dynamic mediaResponse) where TResponseTypeWithMedia : IResponseWithMediaProperty
        {
            dynamic typeWithMediaProp = new { media = mediaResponse };
            string json = JsonConvert.SerializeObject(typeWithMediaProp);
            TResponseTypeWithMedia respVal = JsonConvert.DeserializeObject<TResponseTypeWithMedia>(json);
            return respVal;
        }

        private dynamic GetMediaResponse()
        {
            DateTime now = DateTime.Now;
            dynamic mediaResponse = new
            {
                id = "ca492333-35bf-4cc4-a87a-e4af67c30e67",
                type = "IMAGE",
                created = now.AddMinutes(-10),
                last_updated = now.AddMinutes(-2)
            };
            return mediaResponse;
        }

        private void AssertMediaValuesCorrect(dynamic originalData, IResponseWithMediaProperty response, Type requiredType)
        {
            Assert.AreEqual(originalData.id, response.Media.Id);
            Assert.AreEqual(originalData.type, response.Media.Type);
            Assert.AreEqual(originalData.created, response.Media.Created);
            Assert.AreEqual(originalData.last_updated, response.Media.LastUpdated);
            Assert.IsInstanceOfType(response, requiredType);
        }
    }
}