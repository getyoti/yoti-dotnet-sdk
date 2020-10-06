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
    }
}