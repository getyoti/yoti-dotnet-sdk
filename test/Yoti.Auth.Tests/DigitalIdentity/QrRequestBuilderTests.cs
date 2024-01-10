using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DigitalIdentity;
using Yoti.Auth.Tests.TestData;

namespace Yoti.Auth.Tests.DigitalIdentity
{
    [TestClass]
    public class QrRequestBuilderTests
    {

        private const string _someTransportString = "someTransport";
        private const string _someDisplayMode = "someDisplay";



        [TestMethod]
        public void ShouldBuildADynamicScenario()
        {
            QrRequest result = new QrRequestBuilder()
                .WithDisplayMode(_someDisplayMode)
                .WithTransport(_someTransportString)
               .Build();

            
            Assert.AreEqual(_someDisplayMode, result.DisplayMode);
            Assert.AreEqual(_someTransportString, result.Transport);
        }
        
    }
}