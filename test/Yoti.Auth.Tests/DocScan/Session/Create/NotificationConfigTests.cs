using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Create;

namespace Yoti.Auth.Tests.DocScan.Session.Create
{
    [TestClass]
    public class NotificationConfigTests
    {
        [TestMethod]
        public void ShouldBeConstructableWithoutAuthType()
        {
            string authToken = "someAuthToken"; 
            string endpoint = "someEndpoint";
            string topicName = "someTopic";
            List<string> topics = new List<string> { topicName };

            NotificationConfig notificationConfig =
              new NotificationConfig(authToken, endpoint, topics);

            Assert.AreEqual(authToken, notificationConfig.AuthToken);
            Assert.AreEqual(endpoint, notificationConfig.Endpoint);
            Assert.AreEqual(topicName, notificationConfig.Topics.Single());
        }

        [TestMethod]
        public void ShouldBeConstructableWithAuthType()
        {
            string authToken = "someAuthToken";
            string endpoint = "someEndpoint";
            string topicName = "someTopic";
            List<string> topics = new List<string>{ topicName };
            string authType = "BASIC";

            NotificationConfig notificationConfig =
              new NotificationConfig(authToken, endpoint, topics, authType);

            Assert.AreEqual(authToken, notificationConfig.AuthToken);
            Assert.AreEqual(endpoint, notificationConfig.Endpoint);
            Assert.AreEqual(topicName, notificationConfig.Topics.Single());
            Assert.AreEqual(authType, notificationConfig.AuthType);
        }
    }
}