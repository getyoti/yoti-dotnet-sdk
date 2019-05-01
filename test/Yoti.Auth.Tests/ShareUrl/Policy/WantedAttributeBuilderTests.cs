using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.ShareUrl.Policy;

namespace Yoti.Auth.Tests.ShareUrl.Policy
{
    [TestClass]
    public class WantedAttributeBuilderTests
    {
        private const string _someName = "some name";
        private const string _someDerivation = "some derivation";

        [TestMethod]
        public void BuildsAnAttribute()
        {
            WantedAttribute result = new WantedAttributeBuilder()
                .WithName(_someName)
                .WithDerivation(_someDerivation)
                .WithOptional(true)
                .Build();

            Assert.AreEqual(_someName, result.Name);
            Assert.AreEqual(_someDerivation, result.Derivation);
            Assert.AreEqual(true, result.IsOptional);
        }
    }
}