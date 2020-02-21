using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Create;

namespace Yoti.Auth.Tests.DocScan.Session.Create
{
    [TestClass]
    public class NotificationConfigBuilderTests
    {
        [TestMethod]
        public void ShouldBuildWithAuthToken()
        {
            string authToken = "someAuthToken";

            NotificationConfig notificationConfig =
              new NotificationConfigBuilder()
              .WithAuthToken(authToken)
              .Build();

            Assert.AreEqual(authToken, notificationConfig.AuthToken);
        }

        [TestMethod]
        public void ShouldBuildWithEndpoint()
        {
            string endpoint = "someEndpoint";

            NotificationConfig notificationConfig =
              new NotificationConfigBuilder()
              .WithEndpoint(endpoint)
              .Build();

            Assert.AreEqual(endpoint, notificationConfig.Endpoint);
        }

        [TestMethod]
        public void ShouldBuildForResourceUpdate()
        {
            NotificationConfig notificationConfig =
              new NotificationConfigBuilder()
              .ForResourceUpdate()
              .Build();

            Assert.AreEqual("RESOURCE_UPDATE", notificationConfig.Topics.Single());
        }

        [TestMethod]
        public void ShouldBuildForCheckCompletion()
        {
            NotificationConfig notificationConfig =
              new NotificationConfigBuilder()
              .ForCheckCompletion()
              .Build();

            Assert.AreEqual("CHECK_COMPLETION", notificationConfig.Topics.Single());
        }

        [TestMethod]
        public void ShouldBuildForSessionCompletion()
        {
            NotificationConfig notificationConfig =
              new NotificationConfigBuilder()
              .ForSessionCompletion()
              .Build();

            Assert.AreEqual("SESSION_COMPLETION", notificationConfig.Topics.Single());
        }

        [TestMethod]
        public void ShouldBuildForTaskCompletion()
        {
            NotificationConfig notificationConfig =
              new NotificationConfigBuilder()
              .ForTaskCompletion()
              .Build();

            Assert.AreEqual("TASK_COMPLETION", notificationConfig.Topics.Single());
        }

        [TestMethod]
        public void ShouldBuildWithTopic()
        {
            string topicName = "someTopic";

            NotificationConfig notificationConfig =
              new NotificationConfigBuilder()
              .WithTopic(topicName)
              .Build();

            Assert.AreEqual(topicName, notificationConfig.Topics.Single());
        }

        [TestMethod]
        public void ShouldNotAddDuplicateTopic()
        {
            string topicName = "someTopic";

            NotificationConfig notificationConfig =
              new NotificationConfigBuilder()
              .WithTopic(topicName)
              .WithTopic(topicName)
              .Build();

            Assert.AreEqual(1, notificationConfig.Topics.Count);
            Assert.AreEqual(topicName, notificationConfig.Topics.Single());
        }
    }
}