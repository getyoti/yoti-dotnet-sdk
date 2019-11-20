using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Exceptions;
using Yoti.Auth.Tests.Common;
using Yoti.Auth.Web;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class ActivityDetailsParserTests
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
                ActivityDetailsParser.HandleResponse(KeyPair.Get(), response.Content);
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
                ActivityDetailsParser.HandleResponse(KeyPair.Get(), response.Content);
            });
        }
    }
}