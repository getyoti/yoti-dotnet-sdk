using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Create;
using Yoti.Auth.DocScan.Session.Create.Check;
using Yoti.Auth.DocScan.Session.Create.Filter;
using Yoti.Auth.DocScan.Session.Create.Task;

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

            Assert.AreEqual("ID_DOCUMENT_TEXT_DATA_EXTRACTION", sessionSpec.RequestedTasks.Single().Type);
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
        public void ShouldBuildWithWithRequiredDocument()
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
    }
}