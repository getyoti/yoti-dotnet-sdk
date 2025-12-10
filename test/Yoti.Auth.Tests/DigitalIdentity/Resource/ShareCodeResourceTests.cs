using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DigitalIdentity.Resource;
using Yoti.Auth.DocScan.Session.Retrieve;

namespace Yoti.Auth.Tests.DigitalIdentity.Resource
{
    [TestClass]
    public class ShareCodeResourceTests
    {
        [TestMethod]
        public void ShouldDeserializeShareCodeResource()
        {
            const string json = @"{
                ""id"": ""resource-123"",
                ""source"": ""USER_PROVIDED"",
                ""created_at"": ""2023-01-01T12:00:00Z"",
                ""last_updated"": ""2023-01-01T13:00:00Z"",
                ""lookup_profile"": {
                    ""id"": ""media-1"",
                    ""type"": ""JSON"",
                    ""created"": ""2023-01-01T12:00:00Z"",
                    ""last_updated"": ""2023-01-01T13:00:00Z""
                },
                ""returned_profile"": {
                    ""id"": ""media-2"",
                    ""type"": ""JSON"",
                    ""created"": ""2023-01-01T12:00:00Z"",
                    ""last_updated"": ""2023-01-01T13:00:00Z""
                },
                ""id_photo"": {
                    ""id"": ""media-3"",
                    ""type"": ""IMAGE"",
                    ""created"": ""2023-01-01T12:00:00Z"",
                    ""last_updated"": ""2023-01-01T13:00:00Z""
                },
                ""file"": {
                    ""id"": ""media-4"",
                    ""type"": ""PDF"",
                    ""created"": ""2023-01-01T12:00:00Z"",
                    ""last_updated"": ""2023-01-01T13:00:00Z""
                },
                ""tasks"": [
                    {
                        ""type"": ""SUPPLEMENTARY_DOC_TEXT_DATA_EXTRACTION"",
                        ""id"": ""task-1"",
                        ""state"": ""DONE"",
                        ""created"": ""2023-01-01T12:00:00Z"",
                        ""last_updated"": ""2023-01-01T13:00:00Z"",
                        ""generated_media"": [
                            {
                                ""id"": ""gen-media-1"",
                                ""type"": ""JSON""
                            }
                        ]
                    }
                ]
            }";

            ShareCodeResource result = JsonConvert.DeserializeObject<ShareCodeResource>(json);

            Assert.IsNotNull(result);
            Assert.AreEqual("resource-123", result.Id);
            Assert.AreEqual("USER_PROVIDED", result.Source);
            Assert.AreEqual("2023-01-01T12:00:00Z", result.CreatedAt);
            Assert.AreEqual("2023-01-01T13:00:00Z", result.LastUpdated);
            
            Assert.IsNotNull(result.LookupProfile);
            Assert.AreEqual("media-1", result.LookupProfile.Id);
            
            Assert.IsNotNull(result.ReturnedProfile);
            Assert.AreEqual("media-2", result.ReturnedProfile.Id);
            
            Assert.IsNotNull(result.IdPhoto);
            Assert.AreEqual("media-3", result.IdPhoto.Id);
            
            Assert.IsNotNull(result.File);
            Assert.AreEqual("media-4", result.File.Id);
            
            Assert.IsNotNull(result.Tasks);
            Assert.AreEqual(1, result.Tasks.Count);
            Assert.AreEqual("task-1", result.Tasks[0].Id);
            Assert.AreEqual("DONE", result.Tasks[0].State);
            Assert.AreEqual(1, result.Tasks[0].GeneratedMedia.Count);
        }

        [TestMethod]
        public void ShouldHandleNullMediaFields()
        {
            const string json = @"{
                ""id"": ""resource-123"",
                ""source"": ""USER_PROVIDED"",
                ""created_at"": ""2023-01-01T12:00:00Z"",
                ""last_updated"": ""2023-01-01T13:00:00Z"",
                ""tasks"": []
            }";

            ShareCodeResource result = JsonConvert.DeserializeObject<ShareCodeResource>(json);

            Assert.IsNotNull(result);
            Assert.AreEqual("resource-123", result.Id);
            Assert.IsNull(result.LookupProfile);
            Assert.IsNull(result.ReturnedProfile);
            Assert.IsNull(result.IdPhoto);
            Assert.IsNull(result.File);
        }
    }
}
