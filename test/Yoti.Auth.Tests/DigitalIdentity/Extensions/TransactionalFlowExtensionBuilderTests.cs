using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.ShareUrl.Extensions;

namespace Yoti.Auth.Tests.DigitalIdentity.Extensions
{
    [TestClass]
    public class TransactionalFlowExtensionBuilderTests
    {
        private readonly object _objectContent = new object();
        private readonly DateTime _dateTimeContent = new DateTime(1980, 1, 1);

        [TestMethod]
        public void ShouldFailForNullContent()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new TransactionalFlowExtensionBuilder<object>()
                .WithContent(null)
                .Build();
            });
        }

        [TestMethod]
        public void ShouldBuildWithObjectContent()
        {
            Extension<object> extension = new TransactionalFlowExtensionBuilder<object>()
                .WithContent(_objectContent)
                .Build();

            Assert.AreEqual(_objectContent, extension.Content);
        }

        [TestMethod]
        public void ShouldBuildWithDateTimeContent()
        {
            Extension<DateTime> extension = new TransactionalFlowExtensionBuilder<DateTime>()
                .WithContent(_dateTimeContent)
                .Build();

            Assert.AreEqual(_dateTimeContent, extension.Content);
        }
    }
}