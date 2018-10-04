using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Yoti.Auth.Anchors;

namespace Yoti.Auth
{
    /// <summary>
    /// A class to represent a Yoti attribute. A Yoti attribute consists of the attribute name, an associated
    /// <see cref="YotiAttributeValue"/>, and a list of <see cref="Anchor"/>s from Yoti.
    /// It may hold one or more anchors, which specify how an attribute has been provided
    /// and how it has been verified within the Yoti platform.
    /// </summary>
    public class YotiAttribute<T>
    {
        protected readonly YotiAttributeValue Value;
        private readonly string _name;
        private readonly List<Anchor> _anchors;

        public YotiAttribute(string name, YotiAttributeValue value)
        {
            _name = name;
            Value = value;
        }

        public YotiAttribute(string name, YotiAttributeValue value, List<Anchor> anchors)
        {
            _name = name;
            Value = value;
            _anchors = anchors;
        }

        /// <summary>
        /// Gets the name of the attribute
        /// </summary>
        /// <returns>Attribute name</returns>
        public string GetName()
        {
            return _name;
        }

        /// <summary>
        /// Retrieves the value of an attribute. If this is null, the default value for the type is returned.
        /// </summary>
        /// <returns>Value of the attribute</returns>
        public T GetValue()
        {
            if (Value == null)
                return default(T);

            return Value.ToBytes().ConvertType<T>();
        }

        /// <summary>
        /// Gets the JSON value of an attribute, in the form of a Dictionary
        /// </summary>
        /// <returns>JSON value of an attribute</returns>
        public Dictionary<string, JToken> GetJsonValue()
        {
            return Value.ToJson();
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

        /// <summary>
        /// Get the anchors for an attribute. If an attribute has only one SOURCE
        /// Anchor with the value set to "USER_PROVIDED" and zero VERIFIER Anchors,
        /// then the attribute is a self-certified one.
        /// </summary>
        /// <returns>A list of all of the anchors associated with an attribute</returns>
        public List<Anchor> GetAnchors()
        {
            return _anchors.ToList();
        }

        /// <summary>
        /// Sources are a subset of the anchors associated with an attribute, where the
        /// anchor type is <see cref="AnchorType.Source"/>".
        /// </summary>
        /// <returns>A list of <see cref="AnchorType.Source"/>" anchors</returns>
        public List<Anchor> GetSources()
        {
            return _anchors.Where(a => a.GetAnchorType() == AnchorType.Source).ToList();
        }

        /// <summary>
        /// Verifiers are a subset of the anchors associated with an attribute, where the
        /// anchor type is <see cref="AnchorType.Verifier"/>".
        /// </summary>
        /// <returns>A list of <see cref="AnchorType.Verifier"/>" anchors</returns>
        public List<Anchor> GetVerifiers()
        {
            return _anchors.Where(a => a.GetAnchorType() == AnchorType.Verifier).ToList();
        }
    }
}