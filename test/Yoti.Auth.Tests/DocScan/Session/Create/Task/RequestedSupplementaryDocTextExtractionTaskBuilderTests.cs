using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Create.Task;

namespace Yoti.Auth.Tests.DocScan.Session.Create.Check
{
    [TestClass]
    public class RequestedSupplementaryDocTextExtractionTaskBuilderTests
    {
        [TestMethod]
        public void ShouldReturnCorrectType()
        {
            RequestedSupplementaryDocTextExtractionTask task =
              new RequestedSupplementaryDocTextExtractionTaskBuilder()
              .Build();

            Assert.AreEqual("SUPPLEMENTARY_DOCUMENT_TEXT_DATA_EXTRACTION", task.Type);
        }

        [TestMethod]
        public void ShouldBuildWithManualCheckAlways()
        {
            RequestedSupplementaryDocTextExtractionTask task =
              new RequestedSupplementaryDocTextExtractionTaskBuilder()
              .WithManualCheckAlways()
              .Build();

            Assert.AreEqual("ALWAYS", task.Config.ManualCheck);
        }

        [TestMethod]
        public void ShouldBuildWithManualCheckFallback()
        {
            RequestedSupplementaryDocTextExtractionTask task =
              new RequestedSupplementaryDocTextExtractionTaskBuilder()
              .WithManualCheckFallback()
              .Build();

            Assert.AreEqual("FALLBACK", task.Config.ManualCheck);
        }

        [TestMethod]
        public void ShouldBuildWithManualCheckNever()
        {
            RequestedSupplementaryDocTextExtractionTask task =
              new RequestedSupplementaryDocTextExtractionTaskBuilder()
              .WithManualCheckNever()
              .Build();

            Assert.AreEqual("NEVER", task.Config.ManualCheck);
        }
    }
}