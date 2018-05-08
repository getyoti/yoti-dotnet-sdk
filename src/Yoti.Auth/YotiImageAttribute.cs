using System.Collections.Generic;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth
{
    public class YotiImageAttribute<T> : YotiAttribute<T> where T : Image
    {
        public YotiImageAttribute(string name, YotiAttributeValue value) : base(name, value)
        {
        }

        public YotiImageAttribute(string name, YotiAttributeValue value, HashSet<string> sources) : base(name, value, sources)
        {
        }

        public YotiImageAttribute(string name, YotiAttributeValue value, HashSet<string> sources, HashSet<string> verifiers) : base(name, value, sources, verifiers)
        {
        }

        public Image GetImage()
        {
            return new Image
            {
                Base64URI = Base64URI,
                Data = Value.ToBytes(),
                Type = Value.Type
            };
        }

        public string Base64URI
        {
            get
            {
                switch (Value.Type)

                {
                    case TypeEnum.Jpeg:
                        return "data:image/jpeg;base64," + Conversion.BytesToBase64(Value.ToBytes());
                    case TypeEnum.Png:
                        return "data:image/png;base64," + Conversion.BytesToBase64(Value.ToBytes());
                    default:
                        return null;
                }
            }
        }
    }
}