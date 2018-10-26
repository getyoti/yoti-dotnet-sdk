using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Yoti.Auth.Anchors;

namespace Yoti.Auth
{
    /// <summary>
    /// A class to represent a Yoti attribute, extending <see cref="BaseAttribute"/>.
    /// A Yoti attribute consists of the attribute name, an associated
    /// <see cref="YotiAttributeValue"/>, and a list of <see cref="Anchor"/>s from Yoti.
    /// It may hold one or more anchors, which specify how an attribute has been provided
    /// and how it has been verified within the Yoti platform.
    /// </summary>
    public class YotiAttribute<T> : BaseAttribute
    {
        internal readonly YotiAttributeValue Value;

        public YotiAttribute(string name, YotiAttributeValue value) : base(name, value)
        {
            Value = value;
        }

        public YotiAttribute(string name, YotiAttributeValue value, List<Anchor> anchors) : base(name, value, anchors)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the json value of an attribute, in the form of a <see cref="Dictionary{string, JToken}"/>
        /// </summary>
        /// <returns>JSON value of an attribute</returns>
        public Dictionary<string, JToken> GetJsonValue()
        {
            return Value.ToJson();
        }

        /// <summary>
        /// Retrieves the value of an attribute. If this is null, the default value for the type is returned.
        /// </summary>
        /// <returns>Value of the attribute</returns>
        public T GetValue()
        {
            if (Value == null)
                return default(T);

            if (typeof(T) == typeof(Image))
            {
                return (T)(object)Value.ToImage();
            };

            return Value.ToBytes().ConvertType<T>();
        }

        /// <summary>
        /// Attempts to get the value of the attribute, and if this is null, then returns the specified default value
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns>The value of the attribute, or if this is null, the default value</returns>
        public T GetValueOrDefault(T defaultValue)
        {
            T value = GetValue();

            if (value != null)
                return value;

            return defaultValue;
        }
    }
}