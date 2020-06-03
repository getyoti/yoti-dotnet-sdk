using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Create.Task;

namespace Yoti.Auth.Tests.DocScan.Session.Create.Check
{
    [TestClass]
    public class RequestedTextExtractionTaskBuilderTests
    {
        [TestMethod]
        public void ShouldBuildWithManualCheckAlways()
        {
            RequestedTextExtractionTask task =
              new RequestedTextExtractionTaskBuilder()
              .WithManualCheckAlways()
              .Build();

            Assert.AreEqual("ALWAYS", task.Config.ManualCheck);
        }

        [TestMethod]
        public void ShouldBuildWithManualCheckFallback()
        {
            RequestedTextExtractionTask task =
              new RequestedTextExtractionTaskBuilder()
              .WithManualCheckFallback()
              .Build();

            Assert.AreEqual("FALLBACK", task.Config.ManualCheck);
        }

        [TestMethod]
        public void ShouldBuildWithManualCheckNever()
        {
            RequestedTextExtractionTask task =
              new RequestedTextExtractionTaskBuilder()
              .WithManualCheckNever()
              .Build();

            Assert.AreEqual("NEVER", task.Config.ManualCheck);
        }

        [TestMethod]
        public void ShouldNotBuildWithOutManualCheckBeingSet()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                new RequestedTextExtractionTaskBuilder().Build();
            });
        }
    }
}