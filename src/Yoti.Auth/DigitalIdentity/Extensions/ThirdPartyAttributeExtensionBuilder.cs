using System;
using System.Collections.Generic;
using Yoti.Auth.Share.ThirdParty;

namespace Yoti.Auth.DigitalIdentity.Extensions
{
    public class ThirdPartyAttributeExtensionBuilder : ExtensionBuilder<ThirdPartyAttributeContent>
    {
        private DateTime _expiryDate;
        private List<AttributeDefinition> _definitions;

        public ThirdPartyAttributeExtensionBuilder()
        {
            _definitions = new List<AttributeDefinition>();
        }

        /// <summary>
        /// Allows you to specify the expiry date of the third party attribute
        /// </summary>
        /// <param name="expiryDate"></param>
        public ThirdPartyAttributeExtensionBuilder WithExpiryDate(DateTime expiryDate)
        {
            _expiryDate = expiryDate;
            return this;
        }

        /// <summary>
        /// Add a definition to the list of specified third party attribute definitions
        /// </summary>
        /// <param name="definition"></param>
        public ThirdPartyAttributeExtensionBuilder WithDefinition(string definition)
        {
            Validation.NotNullOrEmpty(definition, nameof(definition));

            _definitions.Add(new AttributeDefinition(definition));
            return this;
        }

        /// <summary>
        /// Set the list of third party attribute definitions (will override any previously set definitions)
        /// </summary>
        /// <param name="definitions"></param>
        public ThirdPartyAttributeExtensionBuilder WithDefinitions(List<string> definitions)
        {
            Validation.NotNull(definitions, nameof(definitions));

            var attributeDefinitions = new List<AttributeDefinition>();

            foreach (string definition in definitions)
            {
                attributeDefinitions.Add(new AttributeDefinition(definition));
            }

            _definitions = attributeDefinitions;
            return this;
        }

        public new Extension<ThirdPartyAttributeContent> Build()
        {
            var thirdPartyAttributeContent = new ThirdPartyAttributeContent(_expiryDate, _definitions);

            return new Extension<ThirdPartyAttributeContent>(
                Constants.Extension.ThirdPartyAttribute,
                thirdPartyAttributeContent);
        }
    }
}