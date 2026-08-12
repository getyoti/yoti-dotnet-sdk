using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Create.Check;

namespace Yoti.Auth.Tests.DocScan.Session.Create.Check
{
    [TestClass]
    public class RequestedIDDocumentComparisonCheckBuilderTests
    {
        [TestMethod]
        public void ShouldBuildWithNoConfiguration()
        {
            RequestedIdDocumentComparisonCheck idDocumentComparisonCheck = new RequestedIdDocumentComparisonCheckBuilder().Build();

            Assert.AreEqual("ID_DOCUMENT_COMPARISON", idDocumentComparisonCheck.Type);
            Assert.IsNotNull(idDocumentComparisonCheck.Config);
        }

        [TestMethod]
        public void ShouldBuildWithHandledCheckLimit()
        {
            RequestedIdDocumentComparisonCheck idDocumentComparisonCheck =
              new RequestedIdDocumentComparisonCheckBuilder()
              .WithHandledCheckLimit(5)
              .Build();

            Assert.AreEqual(5, idDocumentComparisonCheck.Config.HandledCheckLimit);
        }

        [TestMethod]
        public void ShouldBuildWithoutHandledCheckLimit()
        {
            RequestedIdDocumentComparisonCheck idDocumentComparisonCheck =
              new RequestedIdDocumentComparisonCheckBuilder()
              .Build();

            Assert.IsNull(idDocumentComparisonCheck.Config.HandledCheckLimit);
        }
    }
}