namespace Yoti.Auth.Images
{
    public abstract class Image
    {
        private readonly AttrpubapiV1.ContentType _type;
        private readonly byte[] _content;

        public Image(AttrpubapiV1.ContentType type, byte[] content)
        {
            _content = content;
            _type = type;
        }

        public byte[] GetContent()
        {
            return _content;
        }

        public abstract string GetMIMEType();

        public string GetBase64URI()
        {
            return string.Format("data:{0};base64,{1}", GetMIMEType(), Conversion.BytesToBase64(GetContent()));
        }
    }
}