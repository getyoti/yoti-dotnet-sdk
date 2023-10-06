using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.DigitalIdentity.Extensions;

namespace Yoti.Auth.Tests.DigitalIdentity.Extensions
{
    [TestClass]
    public class ExtensionBuilderTests
    {
        private const string _someType = "Some Type";
        private static readonly Dictionary<string, string> _someContent = new Dictionary<string, string>();

        [TestMethod]
        public void ShouldBuildWithTypeAndContent()
        {
            var extension = new ExtensionBuilder<Dictionary<string, string>>()
                .WithType(_someType)
                .WithContent(_someContent)
                .Build();

            Assert.AreEqual(_someType, extension.ExtensionType);
            Assert.AreEqual(_someContent, extension.Content);
        }
    }
}