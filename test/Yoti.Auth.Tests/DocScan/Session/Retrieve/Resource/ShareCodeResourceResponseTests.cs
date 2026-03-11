using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Source;
using Yoti.Auth.DocScan.Session.Retrieve.Resource;
using Yoti.Auth.DocScan.Session.Retrieve.Task;

namespace Yoti.Auth.Tests.DocScan.Session.Retrieve.Resource
{
    [TestClass]
    public class ShareCodeResourceResponseTests
    {
        [TestMethod]
        public void ShouldFilterVerifyShareCodeTasks()
        {
            var tasks = new List<TaskResponse>
            {
                new VerifyShareCodeTaskResponse(),
                new TaskResponse()
            };

            var shareCodes = new List<ShareCodeResourceResponse>
            {
                new ShareCodeResourceResponse { Tasks = tasks }
            };

            GetSessionResult getSessionResult = new GetSessionResult
            {
                Resources = new ResourceContainer { ShareCodes = shareCodes }
            };

            var result = getSessionResult.Resources.ShareCodes.Single();

            Assert.AreEqual(2, result.Tasks.Count);
            Assert.AreEqual(1, result.GetVerifyShareCodeTasks().Count);
        }

        [TestMethod]
        public void ShouldReturnEmptyListWhenNoVerifyShareCodeTasks()
        {
            var shareCodes = new List<ShareCodeResourceResponse>
            {
                new ShareCodeResourceResponse { Tasks = null }
            };

            GetSessionResult getSessionResult = new GetSessionResult
            {
                Resources = new ResourceContainer { ShareCodes = shareCodes }
            };

            var result = getSessionResult.Resources.ShareCodes.Single();

            Assert.IsNull(result.Tasks);
            Assert.AreEqual(0, result.GetVerifyShareCodeTasks().Count);
        }

        [TestMethod]
        public void ShouldReturnEmptyListForEmptyVerifyShareCodeTaskResponse()
        {
            var shareCodes = new List<ShareCodeResourceResponse>
            {
                new ShareCodeResourceResponse { Tasks = new List<TaskResponse>() }
            };

            GetSessionResult getSessionResult = new GetSessionResult
            {
                Resources = new ResourceContainer { ShareCodes = shareCodes }
            };

            CollectionAssert.AreEqual(
                new List<VerifyShareCodeTaskResponse>(),
                getSessionResult.Resources.ShareCodes.Single().GetVerifyShareCodeTasks());
        }

        [TestMethod]
        public void ShouldReturnEmptyListForSingleParentVerifyShareCodeTaskResponse()
        {
            var tasks = new List<TaskResponse>
            {
                new TaskResponse()
            };

            var shareCodes = new List<ShareCodeResourceResponse>
            {
                new ShareCodeResourceResponse { Tasks = tasks }
            };

            GetSessionResult getSessionResult = new GetSessionResult
            {
                Resources = new ResourceContainer { ShareCodes = shareCodes }
            };

            var result = getSessionResult.Resources.ShareCodes.Single();

            Assert.AreEqual(1, result.Tasks.Count);
            Assert.AreEqual(0, result.GetVerifyShareCodeTasks().Count);
        }

        [TestMethod]
        public void ShouldReturnCorrectShareCodeResourceProperties()
        {
            var lookupMedia = new ShareCodeMediaResponse();
            var returnedMedia = new ShareCodeMediaResponse();
            var idPhotoMedia = new ShareCodeMediaResponse();
            var fileMedia = new ShareCodeMediaResponse();
            var source = new EndUserAllowedSourceResponse();

            var shareCode = new ShareCodeResourceResponse
            {
                Source = source,
                CreatedAt = "2026-01-01T00:00:00Z",
                LastUpdated = "2026-01-02T00:00:00Z",
                LookupProfile = lookupMedia,
                ReturnedProfile = returnedMedia,
                IdPhoto = idPhotoMedia,
                File = fileMedia,
            };

            Assert.AreEqual(source, shareCode.Source);
            Assert.AreEqual("2026-01-01T00:00:00Z", shareCode.CreatedAt);
            Assert.AreEqual("2026-01-02T00:00:00Z", shareCode.LastUpdated);
            Assert.IsNotNull(shareCode.LookupProfile);
            Assert.IsNotNull(shareCode.ReturnedProfile);
            Assert.IsNotNull(shareCode.IdPhoto);
            Assert.IsNotNull(shareCode.File);
        }

        [TestMethod]
        public void ShouldDeserializeShareCodeFromJson()
        {
            var shareCodes = new List<ShareCodeResourceResponse>
            {
                new ShareCodeResourceResponse
                {
                    Source = new EndUserAllowedSourceResponse(),
                    CreatedAt = "2026-01-01T00:00:00Z",
                    LastUpdated = "2026-01-02T00:00:00Z",
                    Tasks = new List<TaskResponse>
                    {
                        new VerifyShareCodeTaskResponse()
                    }
                }
            };

            GetSessionResult getSessionResult = new GetSessionResult
            {
                Resources = new ResourceContainer { ShareCodes = shareCodes }
            };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(getSessionResult);
            GetSessionResult deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<GetSessionResult>(json);

            Assert.IsNotNull(deserialized.Resources.ShareCodes);
            Assert.AreEqual(1, deserialized.Resources.ShareCodes.Count);
        }
    }
}
