using System.Collections.Generic;
using Yoti.Auth.Anchors;

namespace Yoti.Auth.Attribute
{
    /// <summary>
    /// A class to represent a Yoti attribute, extending <see cref="BaseAttribute"/>. A Yoti
    /// attribute consists of the attribute name, an associated <see cref="YotiAttributeValue"/>, and
    /// a list of <see cref="Anchor"/> from Yoti. It may hold one or more anchors, which specify how
    /// an attribute has been provided and how it has been verified within the Yoti platform.
    /// </summary>
    public class YotiAttribute<T> : BaseAttribute
    {
        private readonly T _value;

        public YotiAttribute(string name, T value, List<Anchor> anchors, string id = null) : base(name, anchors, id)
        {
            _value = value;
        }

        /// <summary>
        /// Retrieves the value of an attribute. If this is null, the default value for the type is returned.
        /// </summary>
        /// <returns>Value of the attribute</returns>
        public T GetValue()
        {
            return _value;
        }
    }
}