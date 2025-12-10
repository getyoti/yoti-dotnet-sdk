using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Task;

namespace Yoti.Auth.Tests.DocScan.Session.Retrieve
{
    [TestClass]
    public class ShareCodeResourceResponseTests
    {
        [TestMethod]
        public void ShouldDeserializeShareCodeResource()
        {
            DateTime now = DateTime.Now;
            
            dynamic shareCodeResource = new
            {
                id = "share-code-id-123",
                source = new
                {
                    type = "END_USER"
                },
                created_at = "2021-01-01T12:00:00Z",
                last_updated = "2021-01-02T12:00:00Z",
                lookup_profile = new
                {
                    media = new
                    {
                        id = "media-id-1",
                        type = "JSON",
                        created = now.AddMinutes(-10),
                        last_updated = now.AddMinutes(-5)
                    }
                },
                returned_profile = new
                {
                    media = new
                    {
                        id = "media-id-2",
                        type = "JSON",
                        created = now.AddMinutes(-10),
                        last_updated = now.AddMinutes(-5)
                    }
                },
                id_photo = new
                {
                    media = new
                    {
                        id = "media-id-3",
                        type = "IMAGE",
                        created = now.AddMinutes(-10),
                        last_updated = now.AddMinutes(-5)
                    }
                },
                file = new
                {
                    media = new
                    {
                        id = "media-id-4",
                        type = "PDF",
                        created = now.AddMinutes(-10),
                        last_updated = now.AddMinutes(-5)
                    }
                },
                tasks = new[]
                {
                    new
                    {
                        type = "VERIFY_SHARE_CODE_TASK",
                        id = "task-id-1",
                        state = "DONE",
                        created = "2021-01-01T12:00:00Z",
                        last_updated = "2021-01-01T13:00:00Z",
                        generated_media = new[]
                        {
                            new
                            {
                                id = "gen-media-1",
                                type = "IMAGE"
                            }
                        }
                    }
                }
            };

            string json = JsonConvert.SerializeObject(shareCodeResource);
            ShareCodeResourceResponse response = JsonConvert.DeserializeObject<ShareCodeResourceResponse>(json);

            Assert.AreEqual("share-code-id-123", response.Id);
            Assert.IsNotNull(response.Source);
            Assert.AreEqual("END_USER", response.Source.Type);
            Assert.AreEqual("2021-01-01T12:00:00Z", response.CreatedAt);
            Assert.AreEqual("2021-01-02T12:00:00Z", response.LastUpdated);
            
            Assert.IsNotNull(response.LookupProfile);
            Assert.AreEqual("media-id-1", response.LookupProfile.Media.Id);
            Assert.AreEqual("JSON", response.LookupProfile.Media.Type);
            
            Assert.IsNotNull(response.ReturnedProfile);
            Assert.AreEqual("media-id-2", response.ReturnedProfile.Media.Id);
            
            Assert.IsNotNull(response.IdPhoto);
            Assert.AreEqual("media-id-3", response.IdPhoto.Media.Id);
            Assert.AreEqual("IMAGE", response.IdPhoto.Media.Type);
            
            Assert.IsNotNull(response.File);
            Assert.AreEqual("media-id-4", response.File.Media.Id);
            
            Assert.IsNotNull(response.Tasks);
            Assert.AreEqual(1, response.Tasks.Count);
            Assert.IsInstanceOfType(response.Tasks[0], typeof(VerifyShareCodeTaskResponse));
            Assert.AreEqual("task-id-1", response.Tasks[0].Id);
            Assert.AreEqual("VERIFY_SHARE_CODE_TASK", response.Tasks[0].Type);
            Assert.AreEqual("DONE", response.Tasks[0].State);
            Assert.AreEqual(1, response.Tasks[0].GeneratedMedia.Count);
            Assert.AreEqual("gen-media-1", response.Tasks[0].GeneratedMedia[0].Id);
        }
    }
}
