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
        public void ChipDataShouldBeNullWhenNotSet()
        {
            RequestedTextExtractionTask task =
              new RequestedTextExtractionTaskBuilder()
              .WithManualCheckNever()
              .Build();

            Assert.IsNull(task.Config.ChipData);
        }

        [TestMethod]
        public void ShouldNotBuildWithOutManualCheckBeingSet()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                new RequestedTextExtractionTaskBuilder().Build();
            });
        }

        [TestMethod]
        public void ShouldBuildWithChipDataDesired()
        {
            RequestedTextExtractionTask task =
              new RequestedTextExtractionTaskBuilder()
              .WithManualCheckNever()
              .WithChipDataDesired()
              .Build();

            Assert.AreEqual("NEVER", task.Config.ManualCheck);
            Assert.AreEqual("DESIRED", task.Config.ChipData);
        }

        [TestMethod]
        public void ShouldBuildWithChipDataIgnore()
        {
            RequestedTextExtractionTask task =
              new RequestedTextExtractionTaskBuilder()
              .WithManualCheckAlways()
              .WithChipDataIgnore()
              .Build();

            Assert.AreEqual("ALWAYS", task.Config.ManualCheck);
            Assert.AreEqual("IGNORE", task.Config.ChipData);
        }

        [TestMethod]
        public void ShouldBuildWithCreateExpandedDocumentFields()
        {
            RequestedTextExtractionTask task =
              new RequestedTextExtractionTaskBuilder()
              .WithManualCheckAlways()
              .WithCreateExpandedDocumentFields()
              .Build();

            Assert.AreEqual(true, task.Config.CreateExpandedDocumentFields);
        }

        [TestMethod]
        public void ShouldBuildWithouthCreateExpandedDocumentFields()
        {
            RequestedTextExtractionTask task =
              new RequestedTextExtractionTaskBuilder()
              .WithManualCheckAlways()
              .Build();

            Assert.AreEqual(false, task.Config.CreateExpandedDocumentFields);
        }
    }
}