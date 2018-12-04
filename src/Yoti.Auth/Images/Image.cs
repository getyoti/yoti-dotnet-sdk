namespace Yoti.Auth.Images
{
    public abstract class Image
    {
        private readonly byte[] _content;

        public Image(byte[] content)
        {
            _content = content;
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