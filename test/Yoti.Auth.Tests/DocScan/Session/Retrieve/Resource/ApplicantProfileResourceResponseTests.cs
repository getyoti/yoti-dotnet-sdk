using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Resource;

namespace Yoti.Auth.Tests.DocScan.Session.Retrieve.Resource
{
    [TestClass]
    public class ApplicantProfileResourceResponseTests
    {
        private const string ApplicantProfileJson = @"{
            ""resources"": {
                ""applicant_profiles"": [
                    {
                        ""id"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                        ""source"": {
                            ""type"": ""END_USER""
                        },
                        ""media"": {
                            ""id"": ""media-id-123"",
                            ""type"": ""IMAGE"",
                            ""created"": ""2021-06-11T11:39:24Z"",
                            ""last_updated"": ""2021-06-11T11:39:24Z""
                        },
                        ""created_at"": ""2021-06-11T11:39:24Z"",
                        ""last_updated"": ""2021-06-11T12:00:00Z"",
                        ""tasks"": []
                    }
                ]
            }
        }";

        [TestMethod]
        public void ShouldDeserializeApplicantProfilesFromGetSessionResult()
        {
            var result = JsonConvert.DeserializeObject<GetSessionResult>(ApplicantProfileJson);

            Assert.IsNotNull(result.Resources);
            Assert.IsNotNull(result.Resources.ApplicantProfiles);
            Assert.AreEqual(1, result.Resources.ApplicantProfiles.Count);
        }

        [TestMethod]
        public void ShouldDeserializeApplicantProfileId()
        {
            var result = JsonConvert.DeserializeObject<GetSessionResult>(ApplicantProfileJson);

            var applicantProfile = result.Resources.ApplicantProfiles[0];
            Assert.AreEqual("3fa85f64-5717-4562-b3fc-2c963f66afa6", applicantProfile.Id);
        }

        [TestMethod]
        public void ShouldDeserializeApplicantProfileSource()
        {
            var result = JsonConvert.DeserializeObject<GetSessionResult>(ApplicantProfileJson);

            var applicantProfile = result.Resources.ApplicantProfiles[0];
            Assert.IsNotNull(applicantProfile.Source);
            Assert.AreEqual("END_USER", applicantProfile.Source.Type);
        }

        [TestMethod]
        public void ShouldDeserializeApplicantProfileMedia()
        {
            var result = JsonConvert.DeserializeObject<GetSessionResult>(ApplicantProfileJson);

            var applicantProfile = result.Resources.ApplicantProfiles[0];
            Assert.IsNotNull(applicantProfile.Media);
            Assert.AreEqual("media-id-123", applicantProfile.Media.Id);
            Assert.AreEqual("IMAGE", applicantProfile.Media.Type);
        }

        [TestMethod]
        public void ShouldDeserializeApplicantProfileCreatedAt()
        {
            var result = JsonConvert.DeserializeObject<GetSessionResult>(ApplicantProfileJson);

            var applicantProfile = result.Resources.ApplicantProfiles[0];
            Assert.IsNotNull(applicantProfile.CreatedAt);
            Assert.AreEqual(new DateTime(2021, 6, 11, 11, 39, 24, DateTimeKind.Utc), applicantProfile.CreatedAt.Value);
        }

        [TestMethod]
        public void ShouldDeserializeApplicantProfileLastUpdated()
        {
            var result = JsonConvert.DeserializeObject<GetSessionResult>(ApplicantProfileJson);

            var applicantProfile = result.Resources.ApplicantProfiles[0];
            Assert.IsNotNull(applicantProfile.LastUpdated);
            Assert.AreEqual(new DateTime(2021, 6, 11, 12, 0, 0, DateTimeKind.Utc), applicantProfile.LastUpdated.Value);
        }

        [TestMethod]
        public void ShouldDeserializeApplicantProfileTasks()
        {
            var result = JsonConvert.DeserializeObject<GetSessionResult>(ApplicantProfileJson);

            var applicantProfile = result.Resources.ApplicantProfiles[0];
            Assert.IsNotNull(applicantProfile.Tasks);
            Assert.AreEqual(0, applicantProfile.Tasks.Count);
        }

        [TestMethod]
        public void ShouldHandleNullApplicantProfiles()
        {
            const string jsonWithoutApplicantProfiles = @"{ ""resources"": {} }";
            var result = JsonConvert.DeserializeObject<GetSessionResult>(jsonWithoutApplicantProfiles);

            Assert.IsNotNull(result.Resources);
            Assert.IsNull(result.Resources.ApplicantProfiles);
        }
    }
}
