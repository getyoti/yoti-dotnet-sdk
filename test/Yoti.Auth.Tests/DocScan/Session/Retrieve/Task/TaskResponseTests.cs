using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Resource;
using Yoti.Auth.DocScan.Session.Retrieve.Task;

namespace Yoti.Auth.Tests.DocScan.Session.Retrieve.Task
{
    [TestClass]
    public class TaskResponseTests
    {
        [DataTestMethod]
        [DataRow("ID_DOCUMENT_TEXT_DATA_CHECK", typeof(GeneratedTextDataCheckResponse))]
        [DataRow("OTHER", typeof(GeneratedCheckResponse))]
        [DataRow("", typeof(GeneratedCheckResponse))]
        [DataRow(null, typeof(GeneratedCheckResponse))]
        public void GeneratedCheckResponsesAreParsed(string checkTypeString, Type expectedType)
        {
            var generatedChecks = new List<GeneratedCheckResponse>
            {
                new GeneratedCheckResponse
                {
                    Type = checkTypeString
                }
            };

            var initialGetSessionResult = new GetSessionResult
            {
                Resources = new ResourceContainer
                {
                    IdDocuments = new List<IdDocumentResourceResponse>()
                    {
                        new IdDocumentResourceResponse
                        {
                            Tasks = new List<TaskResponse>
                            {
                                new TaskResponse
                                {
                                    GeneratedChecks = generatedChecks
                                }
                            }
                        }
                    }
                }
            };

            string json = JsonConvert.SerializeObject(initialGetSessionResult);

            GetSessionResult getSessionResultWithConverter =
                JsonConvert.DeserializeObject<GetSessionResult>(json);

            Assert.IsInstanceOfType(getSessionResultWithConverter.Resources.IdDocuments.Single().Tasks.Single().GeneratedChecks.Single(), expectedType);
        }

        [TestMethod]
        public void ShouldFilterGeneratedTextDataChecks()
        {
            var checkResponses = new List<GeneratedCheckResponse>
            {
                new GeneratedCheckResponse(),
                new GeneratedTextDataCheckResponse()
            };

            TaskResponse taskResponse = new TaskResponse { GeneratedChecks = checkResponses };

            Assert.AreEqual(1, taskResponse.GetGeneratedTextDataChecks().Count);

            Assert.IsInstanceOfType(taskResponse.GetGeneratedTextDataChecks().Single(), typeof(GeneratedTextDataCheckResponse));
        }

        [TestMethod]
        public void ShouldReturnEmptyListWhenNoChecksArePresent()
        {
            TaskResponse taskResponse = new TaskResponse { GeneratedChecks = null };

            Assert.AreEqual(0, taskResponse.GetGeneratedTextDataChecks().Count);
        }

        [TestMethod]
        public void ShouldReturnEmptyListForEmptyGeneratedCheckResponse()
        {
            var emptyCheckResponses = new List<GeneratedCheckResponse>();

            TaskResponse taskResponse = new TaskResponse { GeneratedChecks = emptyCheckResponses };

            CollectionAssert.AreEqual(new List<GeneratedTextDataCheckResponse>(), taskResponse.GetGeneratedTextDataChecks());
        }

        [TestMethod]
        public void ShouldReturnEmptyListForSingleParentGeneratedCheckResponse()
        {
            var singleParentCheckResponse = new List<GeneratedCheckResponse> { new GeneratedCheckResponse() };

            TaskResponse taskResponse = new TaskResponse { GeneratedChecks = singleParentCheckResponse };

            CollectionAssert.AreEqual(new List<GeneratedTextDataCheckResponse>(), taskResponse.GetGeneratedTextDataChecks());
        }
    }
}