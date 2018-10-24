using System.Collections.Generic;

namespace Yoti.Auth
{
    public class Profile
    {
        private Dictionary<string, YotiAttribute<object>> _attributes;

        internal Profile()
        {
            _attributes = new Dictionary<string, YotiAttribute<object>>();
        }

        internal Profile(Dictionary<string, YotiAttribute<object>> attributes)
        {
            _attributes = attributes;
        }

        internal void Add(string key, YotiAttribute<object> value)
        {
            _attributes.Add(key, value);
        }

        /// <summary>
        /// Retrieves an attribute based on its name
        /// </summary>
        /// <typeparam name="T">The expected type of the attribute</typeparam>
        /// <param name="name">The name of the attribute</param>
        /// <returns></returns>
        public YotiAttribute<T> GetAttributeByName<T>(string name)
        {
            if (_attributes.TryGetValue(name, out YotiAttribute<object> matchingAttribute))
            {
                return new YotiAttribute<T>(matchingAttribute);
            }

            return null;
        }
    }
}