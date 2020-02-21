using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Check;

namespace Yoti.Auth.Tests.Docs.Session.Retrieve.Check
{
    [TestClass]
    public class CheckResponseTests
    {
        [DataTestMethod]
        [DataRow("ID_DOCUMENT_AUTHENTICITY", typeof(AuthenticityCheckResponse))]
        [DataRow("ID_DOCUMENT_FACE_MATCH", typeof(FaceMatchCheckResponse))]
        [DataRow("ID_DOCUMENT_TEXT_DATA_CHECK", typeof(TextDataCheckResponse))]
        [DataRow("LIVENESS", typeof(LivenessCheckResponse))]
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
    }
}