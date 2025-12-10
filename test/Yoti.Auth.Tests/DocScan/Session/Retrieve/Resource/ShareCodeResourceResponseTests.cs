using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Resource;

namespace Yoti.Auth.Tests.DocScan.Session.Retrieve.Resource
{
    [TestClass]
    public class ShareCodeResourceResponseTests
    {
        [TestMethod]
        public void ShouldDeserializeShareCodeResourceResponse()
        {
            string json = @"{
                ""id"": ""share-code-123"",
                ""source"": {
                    ""type"": ""END_USER""
                },
                ""created_at"": ""2024-01-01T10:00:00Z"",
                ""last_updated"": ""2024-01-01T11:00:00Z"",
                ""lookup_profile"": {
                    ""id"": ""lookup-profile-id"",
                    ""type"": ""JSON"",
                    ""created"": ""2024-01-01T10:00:00Z"",
                    ""last_updated"": ""2024-01-01T11:00:00Z""
                },
                ""returned_profile"": {
                    ""id"": ""returned-profile-id"",
                    ""type"": ""JSON"",
                    ""created"": ""2024-01-01T10:00:00Z"",
                    ""last_updated"": ""2024-01-01T11:00:00Z""
                },
                ""id_photo"": {
                    ""id"": ""id-photo-id"",
                    ""type"": ""IMAGE"",
                    ""created"": ""2024-01-01T10:00:00Z"",
                    ""last_updated"": ""2024-01-01T11:00:00Z""
                },
                ""file"": {
                    ""id"": ""file-id"",
                    ""type"": ""PDF"",
                    ""created"": ""2024-01-01T10:00:00Z"",
                    ""last_updated"": ""2024-01-01T11:00:00Z""
                },
                ""tasks"": []
            }";

            ShareCodeResourceResponse response = JsonConvert.DeserializeObject<ShareCodeResourceResponse>(json);

            Assert.IsNotNull(response);
            Assert.AreEqual("share-code-123", response.Id);
            Assert.AreEqual("2024-01-01T10:00:00Z", response.CreatedAt);
            Assert.AreEqual("2024-01-01T11:00:00Z", response.LastUpdated);
            Assert.IsNotNull(response.LookupProfile);
            Assert.AreEqual("lookup-profile-id", response.LookupProfile.Id);
            Assert.IsNotNull(response.ReturnedProfile);
            Assert.AreEqual("returned-profile-id", response.ReturnedProfile.Id);
            Assert.IsNotNull(response.IdPhoto);
            Assert.AreEqual("id-photo-id", response.IdPhoto.Id);
            Assert.IsNotNull(response.File);
            Assert.AreEqual("file-id", response.File.Id);
        }

        [TestMethod]
        public void ShouldDeserializeResourceContainerWithShareCodes()
        {
            string json = @"{
                ""share_codes"": [
                    {
                        ""id"": ""share-code-1"",
                        ""source"": {
                            ""type"": ""END_USER""
                        },
                        ""created_at"": ""2024-01-01T10:00:00Z"",
                        ""last_updated"": ""2024-01-01T11:00:00Z"",
                        ""tasks"": []
                    },
                    {
                        ""id"": ""share-code-2"",
                        ""source"": {
                            ""type"": ""END_USER""
                        },
                        ""created_at"": ""2024-01-02T10:00:00Z"",
                        ""last_updated"": ""2024-01-02T11:00:00Z"",
                        ""tasks"": []
                    }
                ]
            }";

            ResourceContainer container = JsonConvert.DeserializeObject<ResourceContainer>(json);

            Assert.IsNotNull(container);
            Assert.IsNotNull(container.ShareCodes);
            Assert.AreEqual(2, container.ShareCodes.Count);
            Assert.AreEqual("share-code-1", container.ShareCodes[0].Id);
            Assert.AreEqual("share-code-2", container.ShareCodes[1].Id);
        }
    }
}
