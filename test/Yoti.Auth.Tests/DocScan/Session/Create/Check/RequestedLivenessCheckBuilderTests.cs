using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Create.Check;

namespace Yoti.Auth.Tests.DocScan.Session.Create.Check
{
    [TestClass]
    public class RequestedLivenessCheckBuilderTests
    {
        [TestMethod]
        public void ShouldBuildForLivenessType()
        {
            RequestedLivenessCheck check =
              new RequestedLivenessCheckBuilder()
              .ForLivenessType("SomeLivenessType")
              .Build();

            Assert.AreEqual("LIVENESS", check.Type);
            Assert.AreEqual("SomeLivenessType", check.Config.LivenessType);
        }

        [TestMethod]
        public void ShouldBuildForZoomLiveness()
        {
            RequestedLivenessCheck check =
              new RequestedLivenessCheckBuilder()
              .ForZoomLiveness()
              .Build();

            Assert.AreEqual("ZOOM", check.Config.LivenessType);
        }

        [TestMethod]
        public void ShouldBuildWithMaxRetries()
        {
            RequestedLivenessCheck check =
              new RequestedLivenessCheckBuilder()
              .ForZoomLiveness()
              .WithMaxRetries(5)
              .Build();

            Assert.AreEqual(5, check.Config.MaxRetries);
        }

        [TestMethod]
        public void ShouldDefaultTo1MaxRetryIfNotSet()
        {
            RequestedLivenessCheck check =
              new RequestedLivenessCheckBuilder()
              .ForZoomLiveness()
              .Build();

            Assert.AreEqual(1, check.Config.MaxRetries);
        }

        [TestMethod]
        public void ShouldNotBuildWithLivenessTypeNotBeingSet()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                new RequestedLivenessCheckBuilder().Build();
            });
        }
    }
}