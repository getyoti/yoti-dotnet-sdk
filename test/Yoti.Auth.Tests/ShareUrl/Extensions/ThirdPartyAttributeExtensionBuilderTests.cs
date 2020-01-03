using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Share.ThirdParty;
using Yoti.Auth.ShareUrl.Extensions;

namespace Yoti.Auth.Tests.ShareUrl.Extensions
{
    [TestClass]
    public class ThirdPartyAttributeExtensionBuilderTests
    {
        private readonly DateTime _someDate = DateTime.Today.AddDays(1);
        private const string _someDefinition = "com.thirdparty.id";

        [TestMethod]
        public void ShouldFailForNullDefinition()
        {
            var exception = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                new ThirdPartyAttributeExtensionBuilder()
                               .WithDefinition(null)
                               .Build();
            });

            Assert.IsTrue(exception.Message.Contains("definition"));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        public void ShouldFailForInvalidDefinitions(string definition)
        {
            var exception = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                new ThirdPartyAttributeExtensionBuilder()
                .WithDefinition(definition)
                .Build();
            });

            Assert.IsTrue(exception.Message.Contains("definition"));
        }

        [TestMethod]
        public void ShouldBuildThirdPartyAttributeExtensionWithGivenValues()
        {
            Extension<ThirdPartyAttributeContent> extension =
                new ThirdPartyAttributeExtensionBuilder()
                .WithDefinition(_someDefinition)
                .WithExpiryDate(_someDate)
                .Build();

            Assert.AreEqual(Constants.Extension.ThirdPartyAttribute, extension.ExtensionType);

            string expectedDate = _someDate.ToString(Constants.Format.RFC3339PatternMilli, CultureInfo.InvariantCulture);
            Assert.AreEqual(expectedDate, extension.Content.ExpiryDate);

            List<AttributeDefinition> definitions = extension.Content.Definitions;
            Assert.AreEqual(1, definitions.Count);
            Assert.AreEqual(_someDefinition, definitions[0].Name);
        }

        [TestMethod]
        public void ShouldBuildThirdPartyAttributeExtensionWithMultipleDefinitions()
        {
            var definitions = new List<string> { "firstDefinition", "secondDefinition" };

            Extension<ThirdPartyAttributeContent> extension =
                new ThirdPartyAttributeExtensionBuilder()
                .WithDefinitions(definitions)
                .WithExpiryDate(_someDate)
                .Build();

            Assert.AreEqual(Constants.Extension.ThirdPartyAttribute, extension.ExtensionType);

            List<AttributeDefinition> result = extension.Content.Definitions;
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("firstDefinition", result[0].Name);
            Assert.AreEqual("secondDefinition", result[1].Name);
        }

        [TestMethod]
        public void ShouldOverwriteSingularlyAddedDefinition()
        {
            var definitions = new List<string> { "firstDefinition", "secondDefinition" };

            Extension<ThirdPartyAttributeContent> extension =
                new ThirdPartyAttributeExtensionBuilder()
                .WithExpiryDate(_someDate)
                .WithDefinition(_someDefinition)
                .WithDefinitions(definitions)
                .Build();

            Assert.AreEqual(Constants.Extension.ThirdPartyAttribute, extension.ExtensionType);

            List<AttributeDefinition> result = extension.Content.Definitions;
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("firstDefinition", result[0].Name);
            Assert.AreEqual("secondDefinition", result[1].Name);
        }
    }
}