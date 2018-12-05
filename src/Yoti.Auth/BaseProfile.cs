using System.Collections.Generic;

namespace Yoti.Auth
{
    public abstract class BaseProfile
    {
        private Dictionary<string, BaseAttribute> _attributes;

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
        /// Retrieves an attribute based on its name, without specifying the type.
        /// The value of the attribute is of type "object", which must be cast to the desired type.
        /// </summary>
        /// <param name="name">The name of the attribute</param>
        /// <returns>Yoti Attribute</returns>
        public YotiAttribute<object> GetAttributeByName(string name)
        {
            if (_attributes.TryGetValue(name, out BaseAttribute matchingAttribute))
            {
                return (YotiAttribute<object>)matchingAttribute;
            }

            return null;
        }
    }
}