using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.Constants;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Resource;
using Yoti.Auth.DocScan.Session.Retrieve.Task;

namespace Yoti.Auth.Tests.DocScan.Session.Retrieve.Task
{
    [TestClass]
    public class VerifyShareCodeTaskResponseTests
    {
        [DataTestMethod]
        [DataRow(DocScanConstants.VerifyShareCodeTask, typeof(VerifyShareCodeTaskResponse))]
        [DataRow("OTHER", typeof(TaskResponse))]
        [DataRow("", typeof(TaskResponse))]
        [DataRow(null, typeof(TaskResponse))]
        public void VerifyShareCodeTaskResponsesAreParsed(string taskTypeString, Type expectedType)
        {
            var tasks = new List<TaskResponse>
            {
                new TaskResponse
                {
                    Id = "someId",
                    Type = taskTypeString
                }
            };

            var shareCodes = new List<ShareCodeResourceResponse>
            {
                new ShareCodeResourceResponse { Tasks = tasks }
            };

            var initialGetSessionResult = new GetSessionResult
            {
                Resources = new ResourceContainer { ShareCodes = shareCodes }
            };

            string json = JsonConvert.SerializeObject(initialGetSessionResult);

            GetSessionResult getSessionResultWithConverter =
                JsonConvert.DeserializeObject<GetSessionResult>(json);

            Assert.IsInstanceOfType(
                getSessionResultWithConverter.Resources.ShareCodes.Single().Tasks.Single(),
                expectedType);
        }
    }
}
