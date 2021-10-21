using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.Constants;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Resource;

namespace Yoti.Auth.Tests.Docs.Session.Retrieve.Check
{
    [TestClass]
    public class LivenessResourceResponseTests
    {
        [DataTestMethod]
        [DataRow(DocScanConstants.Zoom, typeof(ZoomLivenessResourceResponse))]
        [DataRow("OTHER", typeof(LivenessResourceResponse))]
        [DataRow("", typeof(LivenessResourceResponse))]
        [DataRow(null, typeof(LivenessResourceResponse))]
        public void LivenessResourceResponsesAreParsed(string livenessResourceResponseTypeString, Type expectedType)
        {
            var livenessCapture = new List<LivenessResourceResponse>
            {
                new LivenessResourceResponse
                {
                    LivenessType = livenessResourceResponseTypeString
                }
            };

            var resourceContainer = new ResourceContainer { LivenessCapture = livenessCapture };

            var initialGetSessionResult = new GetSessionResult
            {
                Resources = resourceContainer
            };

            string json = JsonConvert.SerializeObject(initialGetSessionResult);

            GetSessionResult getSessionResultWithConverter =
                JsonConvert.DeserializeObject<GetSessionResult>(json);

            Assert.IsInstanceOfType(getSessionResultWithConverter.Resources.LivenessCapture.Single(), expectedType);
        }
    }
}