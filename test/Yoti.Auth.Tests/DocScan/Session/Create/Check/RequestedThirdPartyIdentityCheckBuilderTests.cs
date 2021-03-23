using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DocScan.Session.Create.Check;

namespace Yoti.Auth.Tests.DocScan.Session.Create.Check
{
    [TestClass]
    public class RequestedThirdPartyIdentityCheckBuilderTests
    {
        [TestMethod]
        public void ShouldBuild()
        {
            RequestedThirdPartyIdentityCheck check =
              new RequestedThirdPartyIdentityCheckBuilder()
              .Build();

            Assert.AreEqual("THIRD_PARTY_IDENTITY", check.Type);
            Assert.IsNotNull(check.Config);
        }
    }
}