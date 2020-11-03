using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Resource;
using Yoti.Auth.DocScan.Session.Retrieve.Task;

namespace Yoti.Auth.Tests.DocScan.Session.Retrieve.Task
{
    [TestClass]
    public class IdDocumentResourceResponseTests
    {
        [TestMethod]
        public void ShouldFilterTextExtractionTasks()
        {
            var tasks = new List<TaskResponse>
            {
                new TextExtractionTaskResponse(),
                new TaskResponse()
            };

            var idDocuments = new List<IdDocumentResourceResponse>
            {
                new IdDocumentResourceResponse { Tasks = tasks }
            };

            GetSessionResult getSessionResult = new GetSessionResult { Resources = new ResourceContainer { IdDocuments = idDocuments } };

            var result = getSessionResult.Resources.IdDocuments.Single();

            Assert.AreEqual(
                2,
                result.Tasks.Count);

            Assert.AreEqual(
                1,
                result.GetTextExtractionTasks().Count);
        }

        [TestMethod]
        public void ShouldReturnEmptyListWhenNoTextExtractionTasks()
        {
            var idDocuments = new List<IdDocumentResourceResponse>
            {
                new IdDocumentResourceResponse { Tasks = null }
            };

            GetSessionResult getSessionResult = new GetSessionResult { Resources = new ResourceContainer { IdDocuments = idDocuments } };

            var result = getSessionResult.Resources.IdDocuments.Single();

            Assert.IsNull(result.Tasks);
            Assert.AreEqual(0, result.GetTextExtractionTasks().Count);
        }

        [TestMethod]
        public void ShouldReturnEmptyListForEmptyTextExtractionTaskResponse()
        {
            var idDocuments = new List<IdDocumentResourceResponse>
            {
                new IdDocumentResourceResponse { Tasks = new List<TaskResponse>() }
            };

            GetSessionResult getSessionResult = new GetSessionResult { Resources = new ResourceContainer { IdDocuments = idDocuments } };

            CollectionAssert.AreEqual(new List<TextExtractionTaskResponse>(), getSessionResult.Resources.IdDocuments.Single().GetTextExtractionTasks());
        }

        [TestMethod]
        public void ShouldReturnEmptyListForSingleParentTextExtractionTaskResponse()
        {
            var tasks = new List<TaskResponse>
            {
                new TaskResponse()
            };

            var idDocuments = new List<IdDocumentResourceResponse>
            {
                new IdDocumentResourceResponse { Tasks = tasks }
            };

            GetSessionResult getSessionResult = new GetSessionResult { Resources = new ResourceContainer { IdDocuments = idDocuments } };

            CollectionAssert.AreEqual(new List<TextExtractionTaskResponse>(), getSessionResult.Resources.IdDocuments.Single().GetTextExtractionTasks());
        }
    }
}