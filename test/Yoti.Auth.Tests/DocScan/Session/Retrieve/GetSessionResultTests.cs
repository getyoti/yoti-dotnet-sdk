using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Check;
using Yoti.Auth.DocScan.Session.Retrieve.IdentityProfilePreview;

namespace Yoti.Auth.Tests.DocScan.Session.Retrieve
{
    [TestClass]
    public class GetSessionResultTests
    {
        [TestMethod]
        public void AuthenticityChecksShouldReturnEmptyCollectionWhenNoneOfTypeArePresent()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new FaceMatchCheckResponse()
                }
            };

            Assert.AreEqual(0, getSessionResult.GetAuthenticityChecks().Count);
        }

        [TestMethod]
        public void AuthenticityChecksShouldFilterChecks()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new AuthenticityCheckResponse(),
                    new FaceMatchCheckResponse()
                }
            };

            Assert.AreEqual(1, getSessionResult.GetAuthenticityChecks().Count);
            Assert.IsInstanceOfType(getSessionResult.GetAuthenticityChecks().First(), typeof(AuthenticityCheckResponse));
        }

        [TestMethod]
        public void ChecksShouldReturnEmptyListWhenNotPresent()
        {
            var getSessionResult = new GetSessionResult();

            Assert.AreEqual(0, getSessionResult.GetAuthenticityChecks().Count);
            Assert.AreEqual(0, getSessionResult.GetFaceMatchChecks().Count);
            Assert.AreEqual(0, getSessionResult.GetIdDocumentComparisonChecks().Count);
            Assert.AreEqual(0, getSessionResult.GetLivenessChecks().Count);
#pragma warning disable CS0618 // Type or member is obsolete
            Assert.AreEqual(0, getSessionResult.GetTextDataChecks().Count);
#pragma warning restore CS0618 // Type or member is obsolete
            Assert.AreEqual(0, getSessionResult.GetIdDocumentTextDataChecks().Count);
            Assert.AreEqual(0, getSessionResult.GetSupplementaryDocTextDataChecks().Count);
            Assert.AreEqual(0, getSessionResult.GetThirdPartyIdentityFraudOneChecks().Count);
            Assert.AreEqual(0, getSessionResult.GetThirdPartyIdentityChecks().Count);
            Assert.AreEqual(0, getSessionResult.GetWatchlistScreeningChecks().Count);
            Assert.AreEqual(0, getSessionResult.GetWatchlistAdvancedCaChecks().Count);
        }

        [TestMethod]
        public void FaceMatchChecksShouldReturnEmptyCollectionWhenNoneOfTypeArePresent()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new AuthenticityCheckResponse()
                }
            };

            Assert.AreEqual(0, getSessionResult.GetFaceMatchChecks().Count);
        }

        [TestMethod]
        public void FaceMatchChecksShouldFilterChecks()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new FaceMatchCheckResponse(),
                    new TextDataCheckResponse()
                }
            };

            Assert.AreEqual(1, getSessionResult.GetFaceMatchChecks().Count);
            Assert.IsInstanceOfType(getSessionResult.GetFaceMatchChecks().First(), typeof(FaceMatchCheckResponse));
        }

        [TestMethod]
        public void TextDataChecksShouldReturnEmptyCollectionWhenNoneOfTypeArePresent()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new AuthenticityCheckResponse()
                }
            };

            Assert.AreEqual(0, getSessionResult.GetIdDocumentTextDataChecks().Count);
        }

        [TestMethod]
        public void TextDataChecksShouldFilterChecks()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new TextDataCheckResponse(),
                    new LivenessCheckResponse()
                }
            };

            Assert.AreEqual(1, getSessionResult.GetIdDocumentTextDataChecks().Count);
            Assert.IsInstanceOfType(getSessionResult.GetIdDocumentTextDataChecks().First(), typeof(TextDataCheckResponse));
        }

        [TestMethod]
        public void LivenessChecksShouldReturnEmptyCollectionWhenNoneOfTypeArePresent()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new AuthenticityCheckResponse()
                }
            };

            Assert.AreEqual(0, getSessionResult.GetLivenessChecks().Count);
        }

        [TestMethod]
        public void LivenessChecksShouldFilterChecks()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new LivenessCheckResponse(),
                    new AuthenticityCheckResponse()
                }
            };

            Assert.AreEqual(1, getSessionResult.GetLivenessChecks().Count);
            Assert.IsInstanceOfType(getSessionResult.GetLivenessChecks().First(), typeof(LivenessCheckResponse));
        }

        [TestMethod]
        public void IdDocumentComaprisonChecksShouldReturnEmptyCollectionWhenNoneOfTypeArePresent()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new AuthenticityCheckResponse()
                }
            };

            Assert.AreEqual(0, getSessionResult.GetIdDocumentComparisonChecks().Count);
        }

        [TestMethod]
        public void IdDocumentComaprisonChecksShouldFilterChecks()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new LivenessCheckResponse(),
                    new IdDocumentComparisonCheckResponse()
                }
            };

            Assert.AreEqual(1, getSessionResult.GetIdDocumentComparisonChecks().Count);
            Assert.IsInstanceOfType(getSessionResult.GetIdDocumentComparisonChecks().First(), typeof(IdDocumentComparisonCheckResponse));
        }

        [TestMethod]
        public void SupplementaryDocTextDataChecksShouldReturnEmptyCollectionWhenNoneOfTypeArePresent()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new AuthenticityCheckResponse()
                }
            };

            Assert.AreEqual(0, getSessionResult.GetSupplementaryDocTextDataChecks().Count);
        }

        [TestMethod]
        public void ThirdPartyIdentityChecksShouldReturnEmptyCollectionWhenNoneOfTypeArePresent()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new AuthenticityCheckResponse()
                }
            };

            Assert.AreEqual(0, getSessionResult.GetThirdPartyIdentityChecks().Count);
        }

        [TestMethod]
        public void ThirdPartyIdentityFraudOneChecksShouldReturnEmptyCollectionWhenNoneOfTypeArePresent()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new AuthenticityCheckResponse()
                }
            };

            Assert.AreEqual(0, getSessionResult.GetThirdPartyIdentityFraudOneChecks().Count);
        }

        [TestMethod]
        public void WatchlistScreeningChecksShouldReturnEmptyCollectionWhenNoneOfTypeArePresent()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new AuthenticityCheckResponse()
                }
            };

            Assert.AreEqual(0, getSessionResult.GetWatchlistScreeningChecks().Count);
        }

        [TestMethod]
        public void WatchlistAdvancedCaChecksShouldReturnEmptyCollectionWhenNoneOfTypeArePresent()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new AuthenticityCheckResponse()
                }
            };

            Assert.AreEqual(0, getSessionResult.GetWatchlistAdvancedCaChecks().Count);
        }

        [TestMethod]
        public void SupplementaryDocTextDataChecksShouldFilterChecks()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new LivenessCheckResponse(),
                    new SupplementaryDocTextDataCheckResponse()
                }
            };

            Assert.AreEqual(1, getSessionResult.GetSupplementaryDocTextDataChecks().Count);
            Assert.IsInstanceOfType(getSessionResult.GetSupplementaryDocTextDataChecks().First(), typeof(SupplementaryDocTextDataCheckResponse));
        }

        [TestMethod]
        public void ThirdPartyIdentityChecksShouldFilterChecks()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new LivenessCheckResponse(),
                    new ThirdPartyIdentityCheckResponse()
                }
            };

            Assert.AreEqual(1, getSessionResult.GetThirdPartyIdentityChecks().Count);
            Assert.IsInstanceOfType(getSessionResult.GetThirdPartyIdentityChecks().First(), typeof(ThirdPartyIdentityCheckResponse));
        }

        [TestMethod]
        public void ThirdPartyIdentityFraudOneChecksShouldFilterChecks()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new LivenessCheckResponse(),
                    new ThirdPartyIdentityFraudOneCheckResponse()
                }
            };

            Assert.AreEqual(1, getSessionResult.GetThirdPartyIdentityFraudOneChecks().Count);
            Assert.IsInstanceOfType(getSessionResult.GetThirdPartyIdentityFraudOneChecks().First(), typeof(ThirdPartyIdentityFraudOneCheckResponse));
        }

        [TestMethod]
        public void WatchlistScreeningChecksShouldFilterChecks()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new LivenessCheckResponse(),
                    new WatchlistScreeningCheckResponse()
                }
            };

            Assert.AreEqual(1, getSessionResult.GetWatchlistScreeningChecks().Count);
            Assert.IsInstanceOfType(getSessionResult.GetWatchlistScreeningChecks().First(), typeof(WatchlistScreeningCheckResponse));
        }

        [TestMethod]
        public void WatchlistAdvancedCaChecksShouldFilterChecks()
        {
            var getSessionResult = new GetSessionResult
            {
                Checks = new List<CheckResponse>
                {
                    new LivenessCheckResponse(),
                    new WatchlistAdvancedCaCheckResponse()
                }
            };

            Assert.AreEqual(1, getSessionResult.GetWatchlistAdvancedCaChecks().Count);
            Assert.IsInstanceOfType(getSessionResult.GetWatchlistAdvancedCaChecks().First(), typeof(WatchlistAdvancedCaCheckResponse));
        }

        [TestMethod]
        public void CheckIdentityProfilePreviewResponseIsParsed()
        {
            dynamic identityProfilePreviewResponse = new
            {
                 media = GetMediaResponse() 
            };

            string json = JsonConvert.SerializeObject(identityProfilePreviewResponse);
            IdentityProfilePreviewResponse response =
                JsonConvert.DeserializeObject<IdentityProfilePreviewResponse>(json);

            AssertMediaValuesCorrect(identityProfilePreviewResponse.media, response.Media, typeof(MediaResponse));
        }

        private void AssertMediaValuesCorrect(dynamic originalData, MediaResponse response, Type requiredType)
        {
            Assert.AreEqual(originalData.id, response.Id);
            Assert.AreEqual(originalData.type, response.Type);
            Assert.AreEqual(originalData.created, response.Created);
            Assert.AreEqual(originalData.last_updated, response.LastUpdated);
            Assert.IsInstanceOfType(response, requiredType);
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

        [TestMethod]
        public void ShouldParseAllChecks()
        {
            var checks = new List<CheckResponse>
            {
                new CheckResponse
                {
                    Type = "check1"
                },
                new CheckResponse
                {
                    Type = "ID_DOCUMENT_AUTHENTICITY"
                }
            };

            var getSessionResult = new GetSessionResult
            {
                Checks = checks
            };

            Assert.AreEqual(2, getSessionResult.Checks.Count);
        }
    }
}