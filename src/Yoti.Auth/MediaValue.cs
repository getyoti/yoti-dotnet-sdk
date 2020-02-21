namespace Yoti.Auth
{
    public class MediaValue

    {
        private protected readonly string _mimeType;
        private readonly byte[] _content;

        public MediaValue(string mimeType, byte[] content)
        {
            _mimeType = mimeType;
            _content = content;
        }

        public string GetMIMEType()
        {
            return _mimeType;
        }

        public byte[] GetContent()
        {
            return _content;
        }

        public string GetBase64URI()
        {
            return $"data:{GetMIMEType()};base64,{Conversion.BytesToBase64(GetContent())}";
        }
    }
}