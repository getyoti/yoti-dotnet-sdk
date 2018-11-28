using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Yoti.Auth.Anchors;
using Yoti.Auth.Images;

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
        private readonly byte[] _data;
        private readonly AttrpubapiV1.ContentType _type;

        public YotiAttribute(string name, AttrpubapiV1.ContentType type, byte[] data) : base(name)
        {
            _type = type;
            _data = data;
        }

        public YotiAttribute(string name, AttrpubapiV1.ContentType type, byte[] data, List<Anchor> anchors) : base(name, anchors)
        {
            _type = type;
            _data = data;
        }

        public byte[] Data()
        {
            return _data;
        }

        /// <summary>
        /// Gets the JSON value of an attribute, in the form of a <see cref="Dictionary{string, JToken}"/>
        /// </summary>
        /// <returns>JSON value of an attribute</returns>
        public Dictionary<string, JToken> GetJsonValue()
        {
            string utf8JSON = Conversion.BytesToUtf8(_data);
            Dictionary<string, JToken> deserializedJson = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, JToken>>(utf8JSON);
            return deserializedJson;
        }

        /// <summary>
        /// Retrieves the value of an attribute. If this is null, the default value for the type is returned.
        /// </summary>
        /// <returns>Value of the attribute</returns>
        public T GetValue()
        {
            if (_data == null)
                return default;

            if (typeof(T) == typeof(Image))
            {
                return (T)(object)ToImage(_type);
            };

            return _data.ConvertType<T>();
        }

        private Image ToImage(AttrpubapiV1.ContentType contentType)
        {
            switch (contentType)
            {
                case AttrpubapiV1.ContentType.Jpeg:
                    return new JpegImage(_data);

                case AttrpubapiV1.ContentType.Png:
                    return new PngImage(_data);

                default:
                    throw new InvalidOperationException(
                        string.Format(
                            "Unable to create image from unsupported content type: {0}",
                            _type));
            }
        }
    }
}