namespace Yoti.Auth.Images
{
    public abstract class Image
    {
        private readonly byte[] _content;

        protected Image(byte[] content)
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
            return $"data:{GetMIMEType()};base64,{Conversion.BytesToBase64(GetContent())}";
        }
    }
}