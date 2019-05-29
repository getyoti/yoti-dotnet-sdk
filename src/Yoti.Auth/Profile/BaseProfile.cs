using System;
using System.Collections.Generic;

namespace Yoti.Auth.Profile
{
    public abstract class BaseProfile : IBaseProfile
    {
        public Dictionary<string, BaseAttribute> Attributes { get; private set; }

        protected BaseProfile()
        {
            Attributes = new Dictionary<string, BaseAttribute>();
        }

        protected BaseProfile(Dictionary<string, BaseAttribute> attributes)
        {
            Attributes = attributes;
        }

        internal void Add<T>(YotiAttribute<T> value)
        {
            Attributes.Add(value.GetName(), value);
        }

        /// <summary>
        /// Retrieves an attribute based on its name
        /// </summary>
        /// <typeparam name="T">The expected type of the attribute</typeparam>
        /// <param name="name">The name of the attribute</param>
        /// <returns>Yoti Attribute</returns>
        public YotiAttribute<T> GetAttributeByName<T>(string name)
        {
            if (Attributes.TryGetValue(name, out BaseAttribute matchingAttribute))
            {
                return (YotiAttribute<T>)matchingAttribute;
            }

            return null;
        }

        /// <summary>
        /// Returns all of the <see cref="YotiAttribute"/>s  where the name starts with the
        /// given string, and the type can be cast to the given generic type
        /// Returns null if there were no matches.
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