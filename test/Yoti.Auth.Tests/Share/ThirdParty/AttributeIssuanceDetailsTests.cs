using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Share.ThirdParty;

namespace Yoti.Auth.Tests.Share.ThirdParty
{
    [TestClass]
    public class AttributeIssuanceDetailsTests
    {
        [TestMethod]
        public void NullTokenShouldBeSetToEmptyString()
        {
            var attributeIssuanceDetails = new AttributeIssuanceDetails(null, null, null);

            Assert.AreEqual(string.Empty, attributeIssuanceDetails.Token);
        }
    }
}