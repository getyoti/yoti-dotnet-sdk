using System;
using System.Collections.Generic;

namespace Yoti.Auth
{
    internal class Profile : YotiUserProfile
    {
        private Dictionary<string, YotiAttribute<object>> _attributes;

        /// <summary>
        /// Returns the attribute object for the key, null if it is not present
        /// </summary>
        public YotiAttribute<Object> GetAttributeObject(string name)
        {
            EnsureName(name);
            bool success = _attributes.TryGetValue(name, out YotiAttribute<Object> yotiAttribute);

            if (!success)
                throw new InvalidOperationException(
                    string.Format(
                        "Unable to get value of attribute '{0}'",
                        name));

            return yotiAttribute;
        }

        /// <summary>
        /// Returns the sources for the given key, returns null if it is not present
        /// </summary>
        public HashSet<string> GetAttributeSources(string name)
        {
            return GetAttributeObject(name).GetSources();
        }

        /// <summary>
        /// Returns the verifiers for the given key, returns null if it is not present
        /// </summary>
        public HashSet<string> GetAttributeVerifiers(string name)
        {
            return GetAttributeObject(name).GetVerifiers();
        }

        private void EnsureName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}