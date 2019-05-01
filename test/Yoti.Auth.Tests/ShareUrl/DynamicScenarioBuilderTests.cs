using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.ShareUrl;
using Yoti.Auth.ShareUrl.Extensions;
using Yoti.Auth.ShareUrl.Policy;

namespace Yoti.Auth.Tests.ShareUrl
{
    [TestClass]
    public class DynamicScenarioBuilderTests
    {
        private const string _someEndpoint = "someEndpoint";

        private readonly BaseExtension extension1 = new ExtensionBuilder<string>()
            .WithContent("content")
            .WithType("string type")
            .Build();

        private readonly BaseExtension extension2 = new ExtensionBuilder<DateTime>()
            .WithContent(new DateTime(1990, 01, 01))
            .WithType("datetime type")
            .Build();

        [TestMethod]
        public void ShouldBuildADynamicScenario()
        {
            DynamicPolicy somePolicy = TestTools.ShareUrl.CreateStandardPolicy();
            DynamicScenario result = new DynamicScenarioBuilder()
                .WithCallbackEndpoint(_someEndpoint)
                .WithPolicy(somePolicy)
                .WithExtension(extension1)
                .WithExtension(extension2)
                .Build();

            var expectedExtensions = new List<BaseExtension> { extension1, extension2 };

            Assert.AreEqual(_someEndpoint, result.CallbackEndpoint);
            Assert.AreEqual(somePolicy, result.DynamicPolicy);
            CollectionAssert.AreEqual(expectedExtensions, result.Extensions);
        }
    }
}