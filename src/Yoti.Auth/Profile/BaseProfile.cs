using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Yoti.Auth.Attribute;

namespace Yoti.Auth.Profile
{
    public abstract class BaseProfile : IBaseProfile
    {
        private readonly List<BaseAttribute> _attributeList = new List<BaseAttribute>();

        /// <summary>
        /// Dictionary of <see cref="BaseAttribute"/>. BaseAttributes do not have an associated
        /// value, and must be cast to a <see cref="YotiAttribute{T}"/> (see <see cref="GetAttributeByName{T}(string)"/>)
        /// </summary>
        [Obsolete("Attributes is deprecated after the introduction of multiple same-named attributes, use AttributeCollection instead")]
        public Dictionary<string, BaseAttribute> Attributes { get; private set; }

        /// <summary>
        /// Collection of <see cref="BaseAttribute"/>. BaseAttributes do not have an associated
        /// value, and must be cast to a <see cref="YotiAttribute{T}"/> (see <see cref="GetAttributeByName{T}(string)"/>)
        /// </summary>
        public ReadOnlyCollection<BaseAttribute> AttributeCollection => _attributeList.AsReadOnly();

        protected BaseProfile()
        {
            Attributes = new Dictionary<string, BaseAttribute>();
        }

        protected BaseProfile(List<BaseAttribute> attributes)
        {
            Validation.NotNull(attributes, nameof(attributes));

            Attributes = new Dictionary<string, BaseAttribute>();
            foreach (var attribute in attributes)
            {
                TryAddAttribute(attribute);
            }

            _attributeList = attributes;
        }

        private void TryAddAttribute(BaseAttribute attribute)
        {
            string attributeName = attribute.GetName();

            if (!Attributes.ContainsKey(attributeName))
            {
                Attributes.Add(attributeName, attribute);
            }
        }

        internal void Add<T>(YotiAttribute<T> value)
        {
            TryAddAttribute(value);
            _attributeList.Add(value);
        }

        /// <summary>
        /// Retrieves an attribute which matches the attribute name specified.
        /// </summary>
        /// <typeparam name="T">The expected type of the attribute</typeparam>
        /// <param name="name">The name of the attribute</param>
        /// <returns><see cref="YotiAttribute{T}"/></returns>
        public YotiAttribute<T> GetAttributeByName<T>(string name)
        {
            BaseAttribute firstMatchingAttribute = AttributeCollection.FirstOrDefault(a => a.GetName() == name);

            if (firstMatchingAttribute != null)
            {
                return (YotiAttribute<T>)firstMatchingAttribute;
            }

            return null;
        }

        /// <summary>
        /// Retrieves a list of attributes which match the attribute name specified.
        /// </summary>
        /// <typeparam name="T">The expected type of the attribute</typeparam>
        /// <param name="name">The name to match</param>
        /// <returns>List of <see cref="YotiAttribute{T}"/></returns>
        public ReadOnlyCollection<YotiAttribute<T>> GetAttributesByName<T>(string name)
        {
            List<YotiAttribute<T>> attributes = new List<YotiAttribute<T>>();

            foreach (var attribute in AttributeCollection)
            {
                if (attribute.GetName() == name)
                {
                    attributes.Add((YotiAttribute<T>)attribute);
                }
            }

            return attributes.AsReadOnly();
        }

        /// <summary>
        /// Returns all of the <see cref="YotiAttribute"/> where the name starts with the given
        /// string, and the type can be cast to the given generic type Returns null if there were no matches.
        /// </summary>
        /// <typeparam name="T">The type parameter indicating the type of the desired attribute</typeparam>
        /// <param name="prefix">Attribute name to search for</param>
        /// <returns>All matching attributes, null if there was no match</returns>
        public List<YotiAttribute<T>> FindAttributesStartingWith<T>(string prefix)
        {
            if (prefix == null)
                throw new ArgumentNullException(nameof(prefix));

            List<YotiAttribute<T>> matches = new List<YotiAttribute<T>>();

            foreach (KeyValuePair<string, BaseAttribute> attribute in Attributes)
            {
                if (attribute.Key.StartsWith(prefix, System.StringComparison.Ordinal)
                    && attribute.Value is YotiAttribute<T> castableAttribute)
                {
                    matches.Add(castableAttribute);
                }
            }

            return matches;
        }
    }
}