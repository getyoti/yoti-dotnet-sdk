using System;
using System.Collections.Generic;

namespace Yoti.Auth
{
    public abstract class BaseProfile
    {
        private readonly Dictionary<string, BaseAttribute> _attributes;

        internal BaseProfile()
        {
            _attributes = new Dictionary<string, BaseAttribute>();
        }

        internal BaseProfile(Dictionary<string, BaseAttribute> attributes)
        {
            _attributes = attributes;
        }

        internal void Add<T>(YotiAttribute<T> value)
        {
            _attributes.Add(value.GetName(), value);
        }

        /// <summary>
        /// Retrieves an attribute based on its name
        /// </summary>
        /// <typeparam name="T">The expected type of the attribute</typeparam>
        /// <param name="name">The name of the attribute</param>
        /// <returns>Yoti Attribute</returns>
        public YotiAttribute<T> GetAttributeByName<T>(string name)
        {
            if (_attributes.TryGetValue(name, out BaseAttribute matchingAttribute))
            {
                return (YotiAttribute<T>)matchingAttribute;
            }

            return null;
        }

        /// <summary>
        /// Retrieves an attribute based on its name, without specifying the type

        /// <summary>
        /// Returns all of the <see cref="YotiAttribute"/>s which have a name starting with
        /// the specified string, which can be cast to the specified type.
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

            foreach (KeyValuePair<string, BaseAttribute> attribute in _attributes)
            {
                if (attribute.Key.StartsWith(prefix))
                {
                    if (attribute.Value is YotiAttribute<T> castableAttribute)
                        matches.Add(castableAttribute);
                }
            }

            return matches;
        }
    }
}