using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Check;

namespace Yoti.Auth.Tests.DocScan.Session.Retrieve
{
    [TestClass]
    public class GetSessionResultTests
    {
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

            Assert.AreEqual(1, getSessionResult.GetTextDataChecks().Count);
            Assert.IsInstanceOfType(getSessionResult.GetTextDataChecks().First(), typeof(TextDataCheckResponse));
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