using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Exceptions;
using Yoti.Auth.Tests.Common;
using Yoti.Auth.Web;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class ProfileParserTests
    {
        [TestMethod]
        public void UnsuccessfulResponseThrowsYotiProfileException()
        {
            var response = new Response
            {
                Success = false
            };

            Assert.ThrowsException<YotiProfileException>(() =>
            {
                ProfileParser.HandleResponse(KeyPair.Get(), response);
            });
        }

        [TestMethod]
        public void NullOrEmptyContentThrowsProfileException()
        {
            var response = new Response
            {
                Success = true,
                Content = ""
            };

            Assert.ThrowsException<YotiProfileException>(() =>
            {
                ProfileParser.HandleResponse(KeyPair.Get(), response);
            });
        }
    }
}