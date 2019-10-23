using Yoti.Auth.ProtoBuf.Attribute;

namespace Yoti.Auth.Attribute
{
    public class MultiValueItem
    {
        public MultiValueItem(object value, ContentType contentType)
        {
            Value = value;
            ContentType = contentType;
        }

        public object Value { get; private set; }
        public ContentType ContentType { get; private set; }
    }
}