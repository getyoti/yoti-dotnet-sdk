using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Yoti.Auth.Attribute;

namespace Yoti.Auth.Profile
{
    public abstract class BaseProfile : IBaseProfile
    {
        private readonly Dictionary<string, List<BaseAttribute>> _attributes = new Dictionary<string, List<BaseAttribute>>();

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
        public ReadOnlyCollection<BaseAttribute> AttributeCollection
        {
            get
            {
                return _attributes
                    .SelectMany(x => x.Value)
                    .ToList()
                    .AsReadOnly();
            }
        }

        protected BaseProfile()
        {
            Attributes = new Dictionary<string, BaseAttribute>();
        }

        protected BaseProfile(Dictionary<string, List<BaseAttribute>> attributes)
        {
            Validation.NotNull(attributes, nameof(attributes));

            Attributes = new Dictionary<string, BaseAttribute>();
            foreach (var attributeList in attributes.Values)
            {
                TryAddAttribute(attributeList.FirstOrDefault());
            }

            _attributes = attributes;
        }

        private void TryAddAttribute(BaseAttribute attribute)
        {
            string attributeName = attribute.GetName();

            if (!Attributes.ContainsKey(attributeName))
            {
                Attributes.Add(attributeName, attribute);
            }
        }

        internal void Add<T>(YotiAttribute<T> newValue)
        {
            TryAddAttribute(newValue);

            string attributeName = newValue.GetName();

            if (_attributes.ContainsKey(attributeName))
            {
                List<BaseAttribute> attributeList = _attributes[attributeName];
                attributeList.Add(newValue);
                _attributes[attributeName] = attributeList;
            }
            else
            {
                _attributes.Add(attributeName, new List<BaseAttribute> { newValue });
            }
        }

        /// <summary>
        /// Retrieves an attribute which matches the attribute name specified.
        /// </summary>
        /// <typeparam name="T">The expected type of the attribute</typeparam>
        /// <param name="name">The name of the attribute</param>
        /// <returns><see cref="YotiAttribute{T}"/></returns>
        public YotiAttribute<T> GetAttributeByName<T>(string name)
        {
            bool found = _attributes.TryGetValue(name, out List<BaseAttribute> matchingAttributes);

            if (found)
            {
                return (YotiAttribute<T>)matchingAttributes.First();
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
            List<YotiAttribute<T>> matchingAttributes = new List<YotiAttribute<T>>();

            if (_attributes.ContainsKey(name))
            {
                foreach (var attribute in _attributes[name])
                {
                    matchingAttributes.Add((YotiAttribute<T>)attribute);
                }
            }

            return matchingAttributes.AsReadOnly();
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

            foreach (KeyValuePair<string, List<BaseAttribute>> attributesByName in _attributes)
            {
                if (attributesByName.Key.StartsWith(prefix, StringComparison.Ordinal))
                {
                    foreach (var attribute in attributesByName.Value)
                    {
                        if (attribute is YotiAttribute<T> castableAttribute)
                        {
                            matches.Add(castableAttribute);
                        }
                    }
                }
            }

            return matches;
        }
    }
}