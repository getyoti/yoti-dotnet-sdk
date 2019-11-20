using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Share.ThirdParty;
using Yoti.Auth.Share.ThirdParty.Issuing;

namespace Yoti.Auth.Tests.Share.ThirdParty.Issuing
{
    [TestClass]
    public class AttributeIssuingRequestBuilderTests
    {
        private const string _someIssuanceToken = "someIssuanceToken";
        private const string _someDefinitionName = "some.third.party.definition";
        private const string _someIssuingAttributeValue = "someAttributeValue";

        private static readonly AttributeDefinition _someAttributeDefinition = new AttributeDefinition(_someDefinitionName);
        private static readonly IssuingAttribute _someIssuingAttribute = new IssuingAttribute(_someAttributeDefinition, _someIssuingAttributeValue);

        [TestMethod]
        public void ShouldBuildThirdPartyAttributeIssuingRequest()
        {
            var attributeIssuingRequest = new AttributeIssuingRequestBuilder()
                .WithIssuanceToken(_someIssuanceToken)
                .WithIssuingAttribute(_someIssuingAttribute)
                .Build();

            Assert.AreEqual(1, attributeIssuingRequest.IssuingAttributes.Count);
            Assert.AreEqual(_someIssuanceToken, attributeIssuingRequest.IssuanceToken);

            Assert.AreEqual(_someIssuingAttribute, attributeIssuingRequest.IssuingAttributes[0]);
            Assert.AreEqual(_someDefinitionName, attributeIssuingRequest.IssuingAttributes[0].Name);
            Assert.AreEqual(_someIssuingAttributeValue, attributeIssuingRequest.IssuingAttributes[0].Value);
        }

        [TestMethod]
        public void ShouldAcceptDefinitionAndValue()
        {
            var attributeIssuingRequest = new AttributeIssuingRequestBuilder()
                .WithIssuanceToken(_someIssuanceToken)
                .WithIssuingAttribute(_someAttributeDefinition, _someIssuingAttributeValue)
                .Build();

            Assert.AreEqual(1, attributeIssuingRequest.IssuingAttributes.Count);
            Assert.AreEqual(_someIssuanceToken, attributeIssuingRequest.IssuanceToken);

            Assert.IsTrue(new IssuingAttributeComparer().Equals(_someIssuingAttribute, attributeIssuingRequest.IssuingAttributes[0]));
            Assert.AreEqual(_someDefinitionName, attributeIssuingRequest.IssuingAttributes[0].Name);
            Assert.AreEqual(_someIssuingAttributeValue, attributeIssuingRequest.IssuingAttributes[0].Value);
        }

        [TestMethod]
        public void ShouldAcceptListOfIssuingAttributes()
        {
            List<IssuingAttribute> issuingAttributes = new List<IssuingAttribute>
            {
                CreateIssuingAttribute("some.attribute", "someValue"),
                CreateIssuingAttribute("some.other.attribute", "someOtherValue"),
            };

            var attributeIssuingRequest = new AttributeIssuingRequestBuilder()
                .WithIssuanceToken(_someIssuanceToken)
                .WithIssuingAttributes(issuingAttributes)
                .Build();

            Assert.AreEqual(2, attributeIssuingRequest.IssuingAttributes.Count);

            Assert.IsTrue(new IssuingAttributeComparer().Equals(issuingAttributes[0], attributeIssuingRequest.IssuingAttributes[0]));
            Assert.IsTrue(new IssuingAttributeComparer().Equals(issuingAttributes[1], attributeIssuingRequest.IssuingAttributes[1]));
        }

        private static IssuingAttribute CreateIssuingAttribute(string name, string value)
        {
            AttributeDefinition definition = new AttributeDefinition(name);
            return new IssuingAttribute(definition, value);
        }
    }
}