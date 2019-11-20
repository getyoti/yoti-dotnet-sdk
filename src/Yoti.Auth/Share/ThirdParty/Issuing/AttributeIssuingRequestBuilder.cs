using System.Collections.Generic;

namespace Yoti.Auth.Share.ThirdParty.Issuing
{
    public class AttributeIssuingRequestBuilder
    {
        private string _issuanceToken;
        private List<IssuingAttribute> _issuingAttributes;

        public AttributeIssuingRequestBuilder()
        {
            _issuingAttributes = new List<IssuingAttribute>();
        }

        /// <summary>
        /// Adds a token to the <see cref="AttributeIssuingRequest"/>.
        /// </summary>
        /// <param name="issuanceToken"></param>
        public AttributeIssuingRequestBuilder WithIssuanceToken(string issuanceToken)
        {
            _issuanceToken = issuanceToken;
            return this;
        }

        /// <summary>
        /// Adds an Issuing Attribute to the <see cref="AttributeIssuingRequest"/>.
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="value"></param>
        public AttributeIssuingRequestBuilder WithIssuingAttribute(AttributeDefinition definition, string value)
        {
            var issuingAttribute = new IssuingAttribute(definition, value);
            return WithIssuingAttribute(issuingAttribute);
        }

        /// <summary>
        /// Adds an Issuing Attribute to the <see cref="AttributeIssuingRequest"/>.
        /// </summary>
        /// <param name="issuingAttribute"></param>
        public AttributeIssuingRequestBuilder WithIssuingAttribute(IssuingAttribute issuingAttribute)
        {
            _issuingAttributes.Add(issuingAttribute);
            return this;
        }

        /// <summary>
        /// Sets the list of Issuing Attributes which form the <see cref="AttributeIssuingRequest"/>.
        /// This will replace any issuing attributes set previously.
        /// </summary>
        /// <param name="issuingAttributes"></param>
        public AttributeIssuingRequestBuilder WithIssuingAttributes(List<IssuingAttribute> issuingAttributes)
        {
            _issuingAttributes = issuingAttributes;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="AttributeIssuingRequest"/>
        /// </summary>
        public AttributeIssuingRequest Build()
        {
            return new AttributeIssuingRequest(_issuanceToken, _issuingAttributes);
        }
    }
}