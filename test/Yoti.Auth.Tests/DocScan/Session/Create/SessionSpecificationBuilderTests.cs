using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Create;
using Yoti.Auth.DocScan.Session.Create.Check;
using Yoti.Auth.DocScan.Session.Create.Filter;
using Yoti.Auth.DocScan.Session.Create.Task;
using Yoti.Auth.Tests.TestData;

namespace Yoti.Auth.Tests.DocScan.Session.Create
{
    [TestClass]
    public class SessionSpecificationBuilderTests
    {
        [TestMethod]
        public void ShouldBuildWithClientSessionTokenTtl()
        {
            int clientSessionTokenTtl = 500;

            SessionSpecification sessionSpec =
              new SessionSpecificationBuilder()
              .WithClientSessionTokenTtl(clientSessionTokenTtl)
              .Build();

            Assert.AreEqual(clientSessionTokenTtl, sessionSpec.ClientSessionTokenTtl);
        }

        [TestMethod]
        public void ShouldBuildWithNotifications()
        {
            string topic = "topic";

            NotificationConfig notifications = new NotificationConfigBuilder()
                .WithTopic(topic)
                .Build();

            SessionSpecification sessionSpec =
              new SessionSpecificationBuilder()
              .WithNotifications(notifications)
              .Build();

            Assert.AreEqual(topic, sessionSpec.Notifications.Topics.Single());
        }

        [TestMethod]
        public void ShouldBuildWithRequestedCheck()
        {
            SessionSpecification sessionSpec =
              new SessionSpecificationBuilder()
              .WithRequestedCheck(
                  new RequestedDocumentAuthenticityCheckBuilder()
                  .Build())
              .Build();

            Assert.AreEqual("ID_DOCUMENT_AUTHENTICITY", sessionSpec.RequestedChecks.Single().Type);
        }

        [TestMethod]
        public void ShouldBuildWithRequestedTask()
        {
            SessionSpecification sessionSpec =
              new SessionSpecificationBuilder()
              .WithRequestedTask(
                  new RequestedTextExtractionTaskBuilder()
                  .WithManualCheckFallback()
                  .Build())
              .Build();

            var result = (RequestedTextExtractionTask)sessionSpec.RequestedTasks.Single();

            Assert.AreEqual("ID_DOCUMENT_TEXT_DATA_EXTRACTION", result.Type);
            Assert.AreEqual("FALLBACK", result.Config.ManualCheck);
        }

        [TestMethod]
        public void ShouldBuildWithResourcesTtl()
        {
            int resourcesTtl = 450;

            SessionSpecification sessionSpec =
              new SessionSpecificationBuilder()
              .WithResourcesTtl(resourcesTtl)
              .Build();

            Assert.AreEqual(resourcesTtl, sessionSpec.ResourcesTtl);
        }

        [TestMethod]
        public void ShouldBuildWithSdkConfig()
        {
            SessionSpecification sessionSpec =
              new SessionSpecificationBuilder()
              .WithSdkConfig(
                  new SdkConfigBuilder()
                  .WithAllowsCameraAndUpload()
                  .Build())
              .Build();

            Assert.AreEqual("CAMERA_AND_UPLOAD", sessionSpec.SdkConfig.AllowedCaptureMethods);
        }

        [TestMethod]
        public void ShouldBuildWithUserTrackingId()
        {
            string userTrackingId = "someTrackingId";

            SessionSpecification sessionSpec =
              new SessionSpecificationBuilder()
              .WithUserTrackingId(userTrackingId)
              .Build();

            Assert.AreEqual(userTrackingId, sessionSpec.UserTrackingId);
        }

        [TestMethod]
        public void ShouldBuildWithRequiredDocument()
        {
            SessionSpecification sessionSpec =
              new SessionSpecificationBuilder()
              .WithRequiredDocument(
                  new RequiredIdDocumentBuilder()
                  .WithFilter(
                      new DocumentRestrictionsFilterBuilder()
                      .ForIncludeList()
                      .WithDocumentRestriction(
                          new List<string> { "USA" },
                          new List<string> { "PASSPORT" })
                      .Build())
                  .Build())
              .Build();

            RequiredIdDocument result = (RequiredIdDocument)sessionSpec.RequiredDocuments.Single();
            Assert.AreEqual("ID_DOCUMENT", result.Type);
            Assert.AreEqual("DOCUMENT_RESTRICTIONS", result.Filter.Type);

            DocumentRestrictionsFilter filter = (DocumentRestrictionsFilter)result.Filter;

            Assert.AreEqual("WHITELIST", filter.Inclusion);
            Assert.AreEqual("USA", filter.Documents.Single().CountryCodes.Single());
            Assert.AreEqual("PASSPORT", filter.Documents.Single().DocumentTypes.Single());
        }

        [TestMethod]
        public void ShoudBuildWithIdentityProfileRequirements()
        {
            var sessionSpec = new SessionSpecificationBuilder()
                 .WithIdentityProfileRequirements(new
                 {
                     trust_framework = "UK_TFIDA",
                     scheme = new
                     {
                         type = "DBS",
                         objective = "BASIC"
                     }
                 })
            .Build();

            string sessionSpecJson = JsonConvert.SerializeObject(sessionSpec);
            Assert.IsTrue(sessionSpecJson.Contains("UK_TFIDA"));
            Assert.IsTrue(sessionSpecJson.Contains("DBS"));
            Assert.IsTrue(sessionSpecJson.Contains("BASIC"));
        }

        [TestMethod]
        public void ShoudBuildWithAdvancedIdentityProfileRequirements()
        {
            string advancedIdentityProfileJson = @"
            {
                ""profiles"": [
                    {
                        ""trust_framework"": ""UK_TFIDA"",
                        ""schemes"": [
                            {
                                ""label"": ""LB912"",
                                ""type"": ""RTW""
                            },
                            {
                                ""label"": ""LB777"",
                                ""type"": ""DBS"",
                                ""objective"": ""BASIC""
                            }
                        ]
                    },
                    {
                        ""trust_framework"": ""YOTI_GLOBAL"",
                        ""schemes"": [
                            {
                                ""label"": ""LB321"",
                                ""type"": ""IDENTITY"",
                                ""objective"": ""AL_L1"",
                                ""config"": {}
                            }
                        ]
                    }
                ]
            }";
            
            var sessionSpec = new SessionSpecificationBuilder()
                .WithIdentityProfileRequirements(advancedIdentityProfileJson)
                .Build();

            string sessionSpecJson = JsonConvert.SerializeObject(sessionSpec);
            Assert.IsTrue(sessionSpecJson.Contains("UK_TFIDA"));
            Assert.IsTrue(sessionSpecJson.Contains("YOTI_GLOBAL"));
            Assert.IsTrue(sessionSpecJson.Contains("IDENTITY"));
            
        }
        
        [TestMethod]
        public void ShouldNotImplicitlySetAValueForAdvancedIdentityProfileRequirements()
        {
            SessionSpecification sessionSpec =
                new SessionSpecificationBuilder()
                    .Build();

            Assert.IsNull(sessionSpec.AdvancedIdentityProfileRequirements);
        }
        
        [TestMethod]
        public void ShoudBuildWithSubject()
        {
            var sessionSpec = new SessionSpecificationBuilder()
                .WithSubject(new
                {
                    subject_id = "some_subject_id_string"
                })
            .Build();

            string sessionSpecJson = JsonConvert.SerializeObject(sessionSpec);
            Assert.IsTrue(sessionSpecJson.Contains("some_subject_id_string"));
        }

        [TestMethod]
        public void ShoudBuildWithCreateIdentityProfilePreview()
        {
            var sessionSpec = new SessionSpecificationBuilder()
                .WithCreateIdentityProfilePreview(true)
            .Build();

            Assert.IsTrue(sessionSpec.CreateIdentityProfilePreview);
        }

        [TestMethod]
        public void ShouldBuildWithBlockBiometricConsentTrue()
        {
            SessionSpecification sessionSpec =
              new SessionSpecificationBuilder()
              .WithBlockBiometricConsent(true)
              .Build();

            Assert.IsTrue((bool)sessionSpec.BlockBiometricConsent);
        }

        [TestMethod]
        public void ShouldBuildWithBlockBiometricConsentFalse()
        {
            SessionSpecification sessionSpec =
              new SessionSpecificationBuilder()
              .WithBlockBiometricConsent(false)
              .Build();

            Assert.IsFalse((bool)sessionSpec.BlockBiometricConsent);
        }


        [TestMethod]
        public void ShouldBuildWithSessionDeadline()
        {
            var correctFormat = "2021-09-14T17:48:26.902+01:00";
            DateTimeOffset dateTimeOffset = DateTimeOffset.Parse(correctFormat);

            SessionSpecification sessionSpec =
                new SessionSpecificationBuilder()
                .WithSessionDeadline(dateTimeOffset)
                .Build();

            Assert.AreEqual(dateTimeOffset, sessionSpec.SessionDeadline);
        }

        [TestMethod]
        public void ShouldCorrectlySerialiseSessionDeadlineFormat()
        {
            var correctFormat = "2021-09-14T17:48:26.902+01:00";
            DateTimeOffset dateTimeOffset = DateTimeOffset.Parse(correctFormat);
            var correctKvp = $"\"session_deadline\":\"{correctFormat}\"";

            SessionSpecification sessionSpec =
                new SessionSpecificationBuilder()
                .WithSessionDeadline(dateTimeOffset)
                .Build();
            string sessionSpecJson = JsonConvert.SerializeObject(sessionSpec);

            Assert.AreEqual(dateTimeOffset, sessionSpec.SessionDeadline);
            Assert.IsTrue(sessionSpecJson.Contains(correctKvp));
        }

        [TestMethod]
        public void ShouldNotImplicitlySetAValueForSessionDeadline()
        {
            SessionSpecification sessionSpec =
                new SessionSpecificationBuilder()
                .Build();

            Assert.IsNull(sessionSpec.SessionDeadline);
        }

        [TestMethod]
        public void ShouldNotImplicitlySetAValueForClientSessionTokenTtl()
        {
            SessionSpecification sessionSpec =
                new SessionSpecificationBuilder()
                .Build();

            Assert.IsNull(sessionSpec.ClientSessionTokenTtl);
        }

        [TestMethod]
        public void ShouldBuildWithIdentityProfileRequirements()
        {
            object identityProfileRequirements = IdentityProfiles.CreateStandardIdentityProfileRequirements();

            SessionSpecification sessionSpec =
                new SessionSpecificationBuilder()
                .WithIdentityProfileRequirements(identityProfileRequirements)
                .Build();

            Assert.AreEqual(identityProfileRequirements, sessionSpec.IdentityProfileRequirements);
        }

        [TestMethod]
        public void ShouldBuildWithIdentityProfilePreview()
        {
            object identityProfileRequirements = IdentityProfiles.CreateStandardIdentityProfileRequirements();

            SessionSpecification sessionSpec =
                new SessionSpecificationBuilder()
                .WithIdentityProfileRequirements(identityProfileRequirements)
                .WithCreateIdentityProfilePreview(true)
                .Build();

            Assert.AreEqual(identityProfileRequirements, sessionSpec.IdentityProfileRequirements);
        }
        
        [TestMethod]
        public void ShouldBuildWithAdvancedIdentityProfilePreview()
        {
            AdvancedIdentityProfile advancedIdentityProfileRequirements = IdentityProfiles.CreateStandardAdvancedIdentityProfileRequirements();

            SessionSpecification sessionSpec =
                new SessionSpecificationBuilder()
                    .WithAdvancedIdentityProfileRequirements(advancedIdentityProfileRequirements)
                    .WithCreateIdentityProfilePreview(true)
                    .Build();

            Assert.AreEqual(advancedIdentityProfileRequirements, sessionSpec.AdvancedIdentityProfileRequirements);
        }
        
        [TestMethod]
        public void ShouldBuildWithAdvancedIdentityProfileRequirements()
        {
            AdvancedIdentityProfile advancedIdentityProfileRequirements = IdentityProfiles.CreateStandardAdvancedIdentityProfileRequirements();

            SessionSpecification sessionSpec =
                new SessionSpecificationBuilder()
                    .WithAdvancedIdentityProfileRequirements(advancedIdentityProfileRequirements)
                    .Build();

            Assert.AreEqual(advancedIdentityProfileRequirements, sessionSpec.AdvancedIdentityProfileRequirements);
        }

        [TestMethod]    
        public void ShouldNotImplicitlySetAValueForIdentityProfileRequirements()
        {
            SessionSpecification sessionSpec =
                new SessionSpecificationBuilder()
                .Build();

            Assert.IsNull(sessionSpec.IdentityProfileRequirements);
        }

        [TestMethod]
        public void ShouldBuildWithSubject()
        {
            object subject = IdentityProfiles.CreateStandardSubject();

            SessionSpecification sessionSpec =
                new SessionSpecificationBuilder()
                .WithSubject(subject)
                .Build();

            Assert.AreEqual(subject, sessionSpec.Subject);
        }

        [TestMethod]
        public void ShouldNotImplicitlySetAValueForSubject()
        {
            SessionSpecification sessionSpec =
                new SessionSpecificationBuilder()
                .Build();

            Assert.IsNull(sessionSpec.Subject);
        }
    }
}
