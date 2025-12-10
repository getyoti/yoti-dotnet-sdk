using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Resource;
using Yoti.Auth.DocScan.Session.Retrieve.Task;

namespace Yoti.Auth.Tests.DocScan.Session.Retrieve.Resource
{
    [TestClass]
    public class ShareCodeResourceResponseTests
    {
        [TestMethod]
        public void ShouldParseShareCodeResourceResponse()
        {
            string json = @"
            {
                ""resources"": {
                    ""share_codes"": [
                        {
                            ""id"": ""share-code-id-1"",
                            ""created_at"": ""2024-01-01T10:00:00Z"",
                            ""last_updated"": ""2024-01-01T11:00:00Z"",
                            ""source"": {
                                ""type"": ""END_USER""
                            },
                            ""tasks"": [],
                            ""lookup_profile"": {
                                ""media"": {
                                    ""id"": ""media-id-1"",
                                    ""type"": ""JSON"",
                                    ""created"": ""2024-01-01T10:00:00Z"",
                                    ""last_updated"": ""2024-01-01T10:00:00Z""
                                }
                            },
                            ""returned_profile"": {
                                ""media"": {
                                    ""id"": ""media-id-2"",
                                    ""type"": ""JSON"",
                                    ""created"": ""2024-01-01T10:00:00Z"",
                                    ""last_updated"": ""2024-01-01T10:00:00Z""
                                }
                            },
                            ""id_photo"": {
                                ""media"": {
                                    ""id"": ""media-id-3"",
                                    ""type"": ""IMAGE/JPEG"",
                                    ""created"": ""2024-01-01T10:00:00Z"",
                                    ""last_updated"": ""2024-01-01T10:00:00Z""
                                }
                            },
                            ""file"": {
                                ""media"": {
                                    ""id"": ""media-id-4"",
                                    ""type"": ""JSON"",
                                    ""created"": ""2024-01-01T10:00:00Z"",
                                    ""last_updated"": ""2024-01-01T10:00:00Z""
                                }
                            }
                        }
                    ]
                }
            }";

            GetSessionResult result = JsonConvert.DeserializeObject<GetSessionResult>(json);

            Assert.IsNotNull(result.Resources.ShareCodes);
            Assert.AreEqual(1, result.Resources.ShareCodes.Count);
            
            ShareCodeResourceResponse shareCode = result.Resources.ShareCodes[0];
            Assert.AreEqual("share-code-id-1", shareCode.Id);
            Assert.IsNotNull(shareCode.CreatedAt);
            Assert.IsNotNull(shareCode.LastUpdated);
            Assert.IsNotNull(shareCode.Source);
            Assert.IsNotNull(shareCode.LookupProfile);
            Assert.IsNotNull(shareCode.LookupProfile.Media);
            Assert.AreEqual("media-id-1", shareCode.LookupProfile.Media.Id);
            Assert.IsNotNull(shareCode.ReturnedProfile);
            Assert.IsNotNull(shareCode.ReturnedProfile.Media);
            Assert.AreEqual("media-id-2", shareCode.ReturnedProfile.Media.Id);
            Assert.IsNotNull(shareCode.IdPhoto);
            Assert.IsNotNull(shareCode.IdPhoto.Media);
            Assert.AreEqual("media-id-3", shareCode.IdPhoto.Media.Id);
            Assert.IsNotNull(shareCode.File);
            Assert.IsNotNull(shareCode.File.Media);
            Assert.AreEqual("media-id-4", shareCode.File.Media.Id);
        }

        [TestMethod]
        public void ShouldParseShareCodeWithVerifyShareCodeTask()
        {
            string json = @"
            {
                ""resources"": {
                    ""share_codes"": [
                        {
                            ""id"": ""share-code-id-1"",
                            ""tasks"": [
                                {
                                    ""id"": ""task-id-1"",
                                    ""type"": ""VERIFY_SHARE_CODE_TASK"",
                                    ""state"": ""DONE"",
                                    ""created"": ""2024-01-01T10:00:00Z"",
                                    ""last_updated"": ""2024-01-01T11:00:00Z""
                                }
                            ]
                        }
                    ]
                }
            }";

            GetSessionResult result = JsonConvert.DeserializeObject<GetSessionResult>(json);

            Assert.IsNotNull(result.Resources.ShareCodes);
            ShareCodeResourceResponse shareCode = result.Resources.ShareCodes[0];
            Assert.IsNotNull(shareCode.Tasks);
            Assert.AreEqual(1, shareCode.Tasks.Count);
            Assert.IsInstanceOfType(shareCode.Tasks[0], typeof(VerifyShareCodeTaskResponse));
            
            var verifyTasks = shareCode.GetVerifyShareCodeTasks();
            Assert.AreEqual(1, verifyTasks.Count);
            Assert.AreEqual("task-id-1", verifyTasks[0].Id);
            Assert.AreEqual("VERIFY_SHARE_CODE_TASK", verifyTasks[0].Type);
            Assert.AreEqual("DONE", verifyTasks[0].State);
        }

        [TestMethod]
        public void ShouldReturnEmptyListWhenNoVerifyShareCodeTasks()
        {
            var shareCode = new ShareCodeResourceResponse
            {
                Tasks = new List<TaskResponse>()
            };

            var verifyTasks = shareCode.GetVerifyShareCodeTasks();
            Assert.IsNotNull(verifyTasks);
            Assert.AreEqual(0, verifyTasks.Count);
        }

        [TestMethod]
        public void ShouldReturnEmptyListWhenTasksIsNull()
        {
            var shareCode = new ShareCodeResourceResponse
            {
                Tasks = null
            };

            var verifyTasks = shareCode.GetVerifyShareCodeTasks();
            Assert.IsNotNull(verifyTasks);
            Assert.AreEqual(0, verifyTasks.Count);
        }
    }
}
