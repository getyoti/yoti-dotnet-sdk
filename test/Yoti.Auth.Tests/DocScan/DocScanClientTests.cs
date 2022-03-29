using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Constants;
using Yoti.Auth.DocScan;
using Yoti.Auth.DocScan.Session.Create;
using Yoti.Auth.DocScan.Session.Create.FaceCapture;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Document;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.FaceCapture;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Liveness;
using Yoti.Auth.DocScan.Session.Retrieve.CreateFaceCaptureResourceResponse;
using Yoti.Auth.DocScan.Session.Retrieve.Resource;
using Yoti.Auth.DocScan.Support;
using Yoti.Auth.Exceptions;
using Yoti.Auth.Tests.Common;
using Yoti.Auth.Tests.TestTools;

namespace Yoti.Auth.Tests.DocScan
{
    [TestClass]
    public class DocScanClientTests
    {
        private const string _sdkId = "sdkId";

        private const string _someSessionId = "someSessionId";
        private const string _someMediaId = "someMediaId";
        private string _someRequirementId = "someRequirementId";
        private string _someResourceId = "someResourceId";
        private string _someImageContentType = DocScanConstants.MimeTypeJpg;
        private static byte[] _someImageContents = new byte[] { 0x00, 0x21, 0x60, 0x1F, 0xA1 };

        private AsymmetricCipherKeyPair _keyPair;
        private CreateFaceCaptureResourcePayload _createFaceCaptureResourcePayload;
        private UploadFaceCaptureImagePayload _uploadFaceCaptureImagePayload;

        [TestInitialize]
        public void Startup()
        {
            _keyPair = KeyPair.Get();
            _createFaceCaptureResourcePayload = new CreateFaceCaptureResourcePayload(_someRequirementId);
            _uploadFaceCaptureImagePayload = new UploadFaceCaptureImagePayload(_someImageContentType, _someImageContents);
        }

        [TestMethod]
        public void ShouldFailForNullSdkId()
        {
            var exception = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                new DocScanClient(null, _keyPair);
            });

            Assert.IsTrue(exception.Message.Contains("sdkId"));
        }

        [TestMethod]
        public void ShouldFailForNullKeyPair()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new DocScanClient(_sdkId, keyPair: null);
            });

            Assert.IsTrue(exception.Message.Contains("keyPair"));
        }

        [TestMethod]
        public void ShouldFailForInvalidKeyPair()
        {
            Assert.ThrowsException<FormatException>(() =>
            {
                new DocScanClient(_sdkId, KeyPair.GetInvalidFormatKeyStream());
            });
        }

        [TestMethod]
        public void ShouldNotFailForValidKeyPair()
        {
            var docScanClient = new DocScanClient(_sdkId, KeyPair.GetValidKeyStream(), null, null);
            Assert.IsNotNull(docScanClient);
        }

        [TestMethod]
        public void CreateSessionShouldSucceed()
        {
            string clientSessionToken = "e8b1c85f-f9e7-405b-88eb-f1c318371f16";
            int clientSessionTokenTtl = 500;
            string sessionId = "4edd81c4-c612-4b0c-b385-7c90f0de6845";

            CreateSessionResult createSessionResult = new CreateSessionResult
            {
                ClientSessionToken = clientSessionToken,
                ClientSessionTokenTtl = clientSessionTokenTtl,
                SessionId = sessionId
            };

            string jsonResponse = JsonConvert.SerializeObject(createSessionResult);

            var successResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse),
            };

            Mock<HttpMessageHandler> handlerMock = Auth.Tests.Common.Http.SetupMockMessageHandler(successResponse);
            var httpClient = new HttpClient(handlerMock.Object);

            DocScanClient docScanClient = new DocScanClient(_sdkId, _keyPair, httpClient);

            CreateSessionResult result = docScanClient.CreateSession(
                new SessionSpecificationBuilder().Build());

            Assert.AreEqual(clientSessionToken, result.ClientSessionToken);
            Assert.AreEqual(clientSessionTokenTtl, result.ClientSessionTokenTtl);
            Assert.AreEqual(sessionId, result.SessionId);
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.Unauthorized)]
        [DataRow(HttpStatusCode.InternalServerError)]
        [DataRow(HttpStatusCode.RequestTimeout)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.Forbidden)]
        public void CreateSessionShouldThrowForNonSuccessStatusCode(HttpStatusCode httpStatusCode)
        {
            DocScanClient docScanClient = SetupDocScanClientResponse(httpStatusCode);

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                CreateSessionResult result = docScanClient.CreateSession(
                    new SessionSpecificationBuilder().Build());
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<DocScanException>(aggregateException));
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.Unauthorized)]
        [DataRow(HttpStatusCode.InternalServerError)]
        [DataRow(HttpStatusCode.RequestTimeout)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.Forbidden)]
        public void DeleteMediaContentShouldThrowForNonSuccessStatusCode(HttpStatusCode httpStatusCode)
        {
            DocScanClient docScanClient = SetupDocScanClientResponse(httpStatusCode);

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                docScanClient.DeleteMediaContent("someSessionId", "someMediaId");
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<DocScanException>(aggregateException));
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.Unauthorized)]
        [DataRow(HttpStatusCode.InternalServerError)]
        [DataRow(HttpStatusCode.RequestTimeout)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.Forbidden)]
        public void DeleteMediaContentAsyncShouldThrowForNonSuccessStatusCode(HttpStatusCode httpStatusCode)
        {
            DocScanClient docScanClient = SetupDocScanClientResponse(httpStatusCode);

            Assert.ThrowsExceptionAsync<DocScanException>(async () =>
            {
                await docScanClient.DeleteMediaContentAsync("someSessionId", "someMediaId");
            });
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.Unauthorized)]
        [DataRow(HttpStatusCode.InternalServerError)]
        [DataRow(HttpStatusCode.RequestTimeout)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.Forbidden)]
        public void DeleteSessionShouldThrowForNonSuccessStatusCode(HttpStatusCode httpStatusCode)
        {
            DocScanClient docScanClient = SetupDocScanClientResponse(httpStatusCode);

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                docScanClient.DeleteSession("someSessionId");
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<DocScanException>(aggregateException));
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.Unauthorized)]
        [DataRow(HttpStatusCode.InternalServerError)]
        [DataRow(HttpStatusCode.RequestTimeout)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.Forbidden)]
        public void DeleteSessionAsyncShouldThrowForNonSuccessStatusCode(HttpStatusCode httpStatusCode)
        {
            DocScanClient docScanClient = SetupDocScanClientResponse(httpStatusCode);

            Assert.ThrowsExceptionAsync<DocScanException>(async () =>
            {
                await docScanClient.DeleteSessionAsync("someSessionId");
            });
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.Unauthorized)]
        [DataRow(HttpStatusCode.InternalServerError)]
        [DataRow(HttpStatusCode.RequestTimeout)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.Forbidden)]
        public void GetMediaContentShouldThrowForNonSuccessStatusCode(HttpStatusCode httpStatusCode)
        {
            DocScanClient docScanClient = SetupDocScanClientResponse(httpStatusCode);

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                docScanClient.GetMediaContent("someSessionId", "someMediaId");
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<DocScanException>(aggregateException));
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.Unauthorized)]
        [DataRow(HttpStatusCode.InternalServerError)]
        [DataRow(HttpStatusCode.RequestTimeout)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.Forbidden)]
        public void GetSessionShouldThrowForNonSuccessStatusCode(HttpStatusCode httpStatusCode)
        {
            DocScanClient docScanClient = SetupDocScanClientResponse(httpStatusCode);

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                docScanClient.GetSession("someSessionId");
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<DocScanException>(aggregateException));
        }

        [TestMethod]
        public void GetSessionSuccessShouldReturnResult()
        {
            var getSessionResult = new GetSessionResult { State = "COMPLETED" };

            string jsonResponse = JsonConvert.SerializeObject(getSessionResult);

            var successResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse),
            };

            Mock<HttpMessageHandler> handlerMock = Auth.Tests.Common.Http.SetupMockMessageHandler(successResponse);
            var httpClient = new HttpClient(handlerMock.Object);

            DocScanClient docScanClient = new DocScanClient(_sdkId, _keyPair, httpClient);

            GetSessionResult result = docScanClient.GetSession("some-session-id");

            Assert.AreEqual("COMPLETED", result.State);
            Assert.IsNull(result.BiometricConsentTimestamp);
        }

        [TestMethod]
        public void ShouldReturnBiometricConsentTimestamp()
        {
            var getSessionResult = new GetSessionResult
            {
                BiometricConsentTimestamp = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc)
            };

            string jsonResponse = JsonConvert.SerializeObject(getSessionResult);

            var successResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse),
            };

            Mock<HttpMessageHandler> handlerMock = Http.SetupMockMessageHandler(successResponse);
            var httpClient = new HttpClient(handlerMock.Object);

            DocScanClient docScanClient = new DocScanClient(_sdkId, _keyPair, httpClient);

            GetSessionResult result = docScanClient.GetSession("some-session-id");

            Assert.AreEqual(new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc), result.BiometricConsentTimestamp);
        }

        [TestMethod]
        public void DeleteSessionSuccessShouldSucceed()
        {
            DocScanClient docScanClient = SetupDocScanClientResponse(HttpStatusCode.OK);

            docScanClient.DeleteSession("some-session-id");
        }

        [TestMethod]
        public void DeleteMediaContentSuccessShouldSucceed()
        {
            DocScanClient docScanClient = SetupDocScanClientResponse(HttpStatusCode.OK);

            docScanClient.DeleteMediaContent("some-session-id", "some-media-id");
        }

        [TestMethod]
        public void GetMediaContentShouldReturnMedia()
        {
            string contentTypeImageJpeg = "image/jpeg";

            byte[] imageBytes = Encoding.UTF8.GetBytes("image-body");
            var successResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(imageBytes),
            };
            successResponse.Content.Headers.Add(Constants.Api.ContentTypeHeader, contentTypeImageJpeg);

            Mock<HttpMessageHandler> handlerMock = Auth.Tests.Common.Http.SetupMockMessageHandler(successResponse);
            var httpClient = new HttpClient(handlerMock.Object);

            DocScanClient docScanClient = new DocScanClient(_sdkId, _keyPair, httpClient);

            MediaValue mediaValue = docScanClient.GetMediaContent(_someSessionId, _someMediaId);

            Assert.IsTrue(imageBytes.SequenceEqual(mediaValue.GetContent()));
            Assert.AreEqual(contentTypeImageJpeg, mediaValue.GetMIMEType());
        }

        [TestMethod]
        public void GetMediaContentShouldReturnNullFor204Response()
        {
            var noContentResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent,
                Content = null,
            };

            Mock<HttpMessageHandler> handlerMock = Auth.Tests.Common.Http.SetupMockMessageHandler(noContentResponse);
            var httpClient = new HttpClient(handlerMock.Object);

            DocScanClient docScanClient = new DocScanClient(_sdkId, _keyPair, httpClient);

            MediaValue mediaValue = docScanClient.GetMediaContent(_someSessionId, _someMediaId);

            Assert.IsNull(mediaValue);
        }

        [TestMethod]
        public void GetSupportedDocumentShouldSucceed()
        {
            var passport = new SupportedDocument("PASSPORT");
            var drivingLicence = new SupportedDocument("DRIVING_LICENCE");

            var supportedDocumentsResponse = new SupportedDocumentsResponse(
                new List<SupportedCountry>{
                    new SupportedCountry(
                    "FRA",
                    new List<SupportedDocument> { passport, drivingLicence })
                });

            string jsonResponse = JsonConvert.SerializeObject(supportedDocumentsResponse);

            var successResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse),
            };

            Mock<HttpMessageHandler> handlerMock = Auth.Tests.Common.Http.SetupMockMessageHandler(successResponse);
            var httpClient = new HttpClient(handlerMock.Object);

            DocScanClient docScanClient = new DocScanClient(_sdkId, _keyPair, httpClient);

            SupportedDocumentsResponse result = docScanClient.GetSupportedDocuments();

            Assert.AreEqual(1, result.SupportedCountries.Count);
            Assert.AreEqual("FRA", result.SupportedCountries[0].Code);
            Assert.AreEqual("PASSPORT", result.SupportedCountries[0].SupportedDocuments[0].Type);
            Assert.AreEqual("DRIVING_LICENCE", result.SupportedCountries[0].SupportedDocuments[1].Type);
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.Unauthorized)]
        [DataRow(HttpStatusCode.InternalServerError)]
        [DataRow(HttpStatusCode.RequestTimeout)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.Forbidden)]
        public void GetSupportedDocumentsShouldThrowForNonSuccessStatusCode(HttpStatusCode httpStatusCode)
        {
            DocScanClient docScanClient = SetupDocScanClientResponse(httpStatusCode);

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                docScanClient.GetSupportedDocuments();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<DocScanException>(aggregateException));
        }

        [TestMethod]
        public void ConstructClientWithNullHttpClientShouldSucceed()
        {
            var docScanClient = new DocScanClient(_sdkId, _keyPair, null);
            Assert.IsNotNull(docScanClient);
        }

        [TestMethod]
        public void CreateFaceCaptureResourceShouldSucceed()
        {
            string id = "someId";
            int frames = 4;
            dynamic createFaceCaptureResourceResponse = new { id, frames };
            DocScanClient docScanClient = SetupDocScanClient(createFaceCaptureResourceResponse);

            CreateFaceCaptureResourceResponse result = docScanClient.CreateFaceCaptureResource(_someSessionId, _createFaceCaptureResourcePayload);

            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(frames, result.Frames);
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.Unauthorized)]
        [DataRow(HttpStatusCode.InternalServerError)]
        [DataRow(HttpStatusCode.RequestTimeout)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.Forbidden)]
        public void CreateFaceCaptureResourceShouldThrowForNonSuccessStatusCode(HttpStatusCode httpStatusCode)
        {
            DocScanClient docScanClient = SetupDocScanClientResponse(httpStatusCode);

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                docScanClient.CreateFaceCaptureResource(_someSessionId, _createFaceCaptureResourcePayload);
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<DocScanException>(aggregateException));
        }

        [TestMethod]
        public void UploadFaceCaptureImageShouldSucceed()
        {
            DocScanClient docScanClient = SetupDocScanClientResponse(HttpStatusCode.OK);

            Action act = () => docScanClient.UploadFaceCaptureImage(_someSessionId, _someResourceId, _uploadFaceCaptureImagePayload);

            Assert.That.DoesNotThrowException(act);
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.Unauthorized)]
        [DataRow(HttpStatusCode.InternalServerError)]
        [DataRow(HttpStatusCode.RequestTimeout)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.Forbidden)]
        public void UploadFaceCaptureImageShouldThrowForNonSuccessStatusCode(HttpStatusCode httpStatusCode)
        {
            DocScanClient docScanClient = SetupDocScanClientResponse(httpStatusCode);

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                docScanClient.UploadFaceCaptureImage(_someSessionId, _someResourceId, _uploadFaceCaptureImagePayload);
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<DocScanException>(aggregateException));
        }

        [TestMethod]
        public void GetSessionConfigurationShouldSucceed()
        {
            int clientSessionTokenTtl = 3600;
            var requestedChecks = new List<string> { "check1", "check2" };
            string biometricConsent = "someBiometricConsent";
            var requiredIdDocumentResourceResponse = Mock.Of<RequiredIdDocumentResourceResponse>(ctx => ctx.Type == DocScanConstants.IdDocument);
            var requiredSupplementaryDocumentResourceResponse = Mock.Of<RequiredSupplementaryDocumentResourceResponse>(ctx => ctx.Type == DocScanConstants.SupplementaryDocument);
            var requiredLivenessResourceResponse = Mock.Of<RequiredLivenessResourceResponse>(ctx => ctx.Type == DocScanConstants.Liveness);
            var requiredZoomLivenessResourceResponse = Mock.Of<RequiredZoomLivenessResourceResponse>(ctx => ctx.Type == DocScanConstants.Liveness
                && ctx.LivenessType == DocScanConstants.Zoom);
            var relyingBusinessAllowedSourceResponse = Mock.Of<AllowedSourceResponse>(ctx => ctx.Type == DocScanConstants.RelyingBusiness);
            var allowedSources = new List<AllowedSourceResponse> {
               relyingBusinessAllowedSourceResponse
            };
            var id = "someId";
            var state = "someState";
            var requiredFaceCaptureResourceResponse = Mock.Of<RequiredFaceCaptureResourceResponse>(ctx => ctx.Type == DocScanConstants.FaceCapture
                && ctx.AllowedSources == allowedSources
                && ctx.Id == id
                && ctx.State == state);
            List<RequiredResourceResponse> requiredResourceResponses = new List<RequiredResourceResponse>
            {
                requiredIdDocumentResourceResponse,
                requiredSupplementaryDocumentResourceResponse,
                requiredLivenessResourceResponse,
                requiredZoomLivenessResourceResponse,
                requiredFaceCaptureResourceResponse
            };
            dynamic captureResponse = new
            {
                biometric_consent = biometricConsent,
                required_resources = requiredResourceResponses
            };
            dynamic sessionConfigurationResponse = new
            {
                client_session_token_ttl = clientSessionTokenTtl,
                session_id = _someSessionId,
                requested_checks = requestedChecks,
                capture = captureResponse
            };
            DocScanClient docScanClient = SetupDocScanClient(sessionConfigurationResponse);

            SessionConfigurationResponse result = docScanClient.GetSessionConfiguration(_someSessionId);

            var faceCaptureResourceRequirement = result.Capture.GetFaceCaptureResourceRequirements().First();

            Assert.AreEqual(clientSessionTokenTtl, result.ClientSessionTokenTtl);
            Assert.AreEqual(_someSessionId, result.SessionId);
            CollectionAssert.AreEqual(requestedChecks, result.RequestedChecks);
            Assert.AreEqual(biometricConsent, result.Capture.BiometricConsent);
            Assert.AreEqual(requiredResourceResponses.Count, result.Capture.RequiredResources.Count);
            Assert.AreEqual(2, result.Capture.GetDocumentResourceRequirements().Count);
            Assert.AreEqual(1, result.Capture.GetIdDocumentResourceRequirements().Count);
            Assert.AreEqual(1, result.Capture.GetSupplementaryResourceRequirements().Count);
            Assert.AreEqual(2, result.Capture.GetLivenessResourceRequirements().Count);
            Assert.AreEqual(1, result.Capture.GetZoomLivenessResourceRequirements().Count);
            Assert.AreEqual(1, result.Capture.GetFaceCaptureResourceRequirements().Count);
            Assert.IsTrue(faceCaptureResourceRequirement.IsRelyingBusinessAllowed);
            Assert.AreEqual(id, faceCaptureResourceRequirement.Id);
            Assert.AreEqual(state, faceCaptureResourceRequirement.State);
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.Unauthorized)]
        [DataRow(HttpStatusCode.InternalServerError)]
        [DataRow(HttpStatusCode.RequestTimeout)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.Forbidden)]
        public void GetSessionConfigurationShouldThrowForNonSuccessStatusCode(HttpStatusCode httpStatusCode)
        {
            DocScanClient docScanClient = SetupDocScanClientResponse(httpStatusCode);

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                docScanClient.GetSessionConfiguration(_someSessionId);
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<DocScanException>(aggregateException));
        }


        [TestMethod]
        public void ShouldParseIdentityProfileResponse()
        {
            string mediaId = "c69ff2db-6caf-4e74-8386-037711bbc8d7";
            string getSessionResult;
            using (StreamReader r = File.OpenText("TestData/GetSessionResultWithIdentityProfile.json"))
            {
                getSessionResult = r.ReadToEnd();
            }

            var successResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(getSessionResult),
            };

            Mock<HttpMessageHandler> handlerMock = Auth.Tests.Common.Http.SetupMockMessageHandler(successResponse);
            var httpClient = new HttpClient(handlerMock.Object);

            DocScanClient docScanClient = new DocScanClient(_sdkId, _keyPair, httpClient);

            GetSessionResult result = docScanClient.GetSession("some-session-id");

            Assert.AreEqual("DONE", result.IdentityProfile.Result);
            Assert.AreEqual("someStringHere", result.IdentityProfile.SubjectId);
            Assert.AreEqual("MANDATORY_DOCUMENT_COULD_NOT_BE_PROVIDED", result.IdentityProfile.FailureReason.ReasonCode);

            Assert.AreEqual("UK_TFIDA", result.IdentityProfile.Report["trust_framework"]);
            JToken expectedSchemesCompliance = JToken.FromObject(
            new[]
            {
                new
                {
                    scheme = new
                    {
                        type=  "DBS",
                        objective=  "STANDARD"
                    },
                    requirements_met = true,
                    requirements_not_met_info = "some string here"
                }
            });
            Assert.IsTrue(JToken.DeepEquals(
                JToken.FromObject(expectedSchemesCompliance),
                result.IdentityProfile.Report["schemes_compliance"]));

            Assert.AreEqual(mediaId, result.IdentityProfile.Report["media"]["id"]);
        }

        private DocScanClient SetupDocScanClient(dynamic responseContent)
        {
            string jsonResponse = JsonConvert.SerializeObject(responseContent,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
                );
            var successResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse),
            };
            Mock<HttpMessageHandler> handlerMock = Http.SetupMockMessageHandler(successResponse);
            return new DocScanClient(_sdkId, _keyPair, new HttpClient(handlerMock.Object));
        }

        private DocScanClient SetupDocScanClientResponse(HttpStatusCode httpStatusCode)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = httpStatusCode,
                Content = new StringContent("{}"),
            };

            Mock<HttpMessageHandler> handlerMock = Auth.Tests.Common.Http.SetupMockMessageHandler(response);
            var httpClient = new HttpClient(handlerMock.Object);

            return new DocScanClient(_sdkId, _keyPair, httpClient);
        }
    }
}