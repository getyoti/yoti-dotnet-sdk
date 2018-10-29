using System.Collections.Generic;

namespace Yoti.Auth
{
    public class Profile
    {
        private Dictionary<string, BaseAttribute> _attributes;

        internal Profile()
        {
            _attributes = new Dictionary<string, BaseAttribute>();
        }

        internal Profile(Dictionary<string, BaseAttribute> attributes)
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
        /// <returns></returns>
        public YotiAttribute<T> GetAttributeByName<T>(string name)
        {
            if (_attributes.TryGetValue(name, out BaseAttribute matchingAttribute))
            {
                return (YotiAttribute<T>)matchingAttribute;
            }

            return null;
        }
    }
}