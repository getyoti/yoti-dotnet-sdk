using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Resource;
using Yoti.Auth.DocScan.Session.Retrieve.Task;

namespace Yoti.Auth.Tests.DocScan.Session.Retrieve.Task
{
    [TestClass]
    public class SupplementaryDocResourceResponseTests
    {
        [TestMethod]
        public void ShouldFilterTextExtractionTasks()
        {
            var tasks = new List<TaskResponse>
            {
                new SupplementaryDocTextExtractionTaskResponse(),
                new TaskResponse()
            };

            var supplementaryDocuments = new List<SupplementaryDocResourceResponse>
            {
                new SupplementaryDocResourceResponse { Tasks = tasks }
            };

            GetSessionResult getSessionResult = new GetSessionResult { Resources = new ResourceContainer { SupplementaryDocuments = supplementaryDocuments } };

            var result = getSessionResult.Resources.SupplementaryDocuments.Single();

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
            var supplementaryDocs = new List<SupplementaryDocResourceResponse>
            {
                new SupplementaryDocResourceResponse { Tasks = null }
            };

            GetSessionResult getSessionResult = new GetSessionResult { Resources = new ResourceContainer { SupplementaryDocuments = supplementaryDocs } };

            SupplementaryDocResourceResponse result = getSessionResult.Resources.SupplementaryDocuments.Single();

            Assert.IsNull(result.Tasks);
            Assert.AreEqual(0, result.GetTextExtractionTasks().Count);
        }

        [TestMethod]
        public void ShouldReturnEmptyListForEmptyTextExtractionTaskResponse()
        {
            var supplementaryDocs = new List<SupplementaryDocResourceResponse>
            {
                new SupplementaryDocResourceResponse { Tasks = new List<TaskResponse>() }
            };

            GetSessionResult getSessionResult = new GetSessionResult { Resources = new ResourceContainer { SupplementaryDocuments = supplementaryDocs } };

            CollectionAssert.AreEqual(new List<TextExtractionTaskResponse>(), getSessionResult.Resources.SupplementaryDocuments.Single().GetTextExtractionTasks());
        }

        [TestMethod]
        public void ShouldReturnEmptyListForSingleParentTextExtractionTaskResponse()
        {
            var tasks = new List<TaskResponse>
            {
                new TaskResponse()
            };

            var supplementaryDocuments = new List<SupplementaryDocResourceResponse>
            {
                new SupplementaryDocResourceResponse { Tasks = tasks }
            };

            GetSessionResult getSessionResult = new GetSessionResult { Resources = new ResourceContainer { SupplementaryDocuments = supplementaryDocuments } };

            CollectionAssert.AreEqual(new List<TextExtractionTaskResponse>(), getSessionResult.Resources.SupplementaryDocuments.Single().GetTextExtractionTasks());
        }

        [TestMethod]
        public void ShouldReturnCorrectSupplementaryDocResourceResponse()
        {
            FileResponse documentFile = new FileResponse { Media = new MediaResponse() };
            var supplementaryDocs = new List<SupplementaryDocResourceResponse>
            {
                new SupplementaryDocResourceResponse
                {
                    DocumentType = "PASSPORT",
                    IssuingCountry = "FRA",
                    Pages = new List<PageResponse>(),
                    DocumentFields = new DocumentFieldsResponse(),
                    DocumentFile = documentFile
                }
            };

            GetSessionResult sessionResult = new GetSessionResult { Resources = new ResourceContainer { SupplementaryDocuments = supplementaryDocs } };
            SupplementaryDocResourceResponse result = sessionResult.Resources.SupplementaryDocuments.Single();

            Assert.AreEqual("PASSPORT", result.DocumentType);
            Assert.IsNotNull(result.IssuingCountry);
            Assert.IsNotNull(result.Pages);
            Assert.IsNotNull(result.DocumentFields);
            Assert.IsNotNull(result.DocumentFile.Media);
        }
    }
}