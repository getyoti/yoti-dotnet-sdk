using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Share.ThirdParty;
using Yoti.Auth.DigitalIdentity.Extensions;

namespace Yoti.Auth.Tests.DigitalIdentity.Extensions
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

        [DataTestMethod]
        [DataRow("2006-01-02T22:04:05Z", "2006-01-02T22:04:05.000Z")]
        [DataRow("2006-01-02T22:04:05.1Z", "2006-01-02T22:04:05.100Z")]
        [DataRow("2006-01-02T22:04:05.12Z", "2006-01-02T22:04:05.120Z")]
        [DataRow("2006-01-02T22:04:05.123Z", "2006-01-02T22:04:05.123Z")]
        [DataRow("2006-01-02T22:04:05.1234Z", "2006-01-02T22:04:05.123Z")]
        [DataRow("2006-01-02T22:04:05.999999Z", "2006-01-02T22:04:05.999Z")]
        [DataRow("2006-01-02T22:04:05.123456Z", "2006-01-02T22:04:05.123Z")]
        [DataRow("2002-10-02T10:00:00.1-05:00", "2002-10-02T15:00:00.100Z")]
        [DataRow("2002-10-02T10:00:00.12345+11:00", "2002-10-01T23:00:00.123Z")]
        [TestMethod]
        public void ShouldBuildThirdPartyAttributeExtensionWithExpiryDates(string expiryDateInputString, string expectedExpiryDate)
        {
            bool parseSuccess = DateTime.TryParse(
                expiryDateInputString,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal,
                out DateTime expiryDate);

            Assert.IsTrue(parseSuccess);

            Extension<ThirdPartyAttributeContent> extension =
                new ThirdPartyAttributeExtensionBuilder()
                .WithDefinition(_someDefinition)
                .WithExpiryDate(expiryDate)
                .Build();

            Assert.AreEqual(expectedExpiryDate, extension.Content.ExpiryDate);
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