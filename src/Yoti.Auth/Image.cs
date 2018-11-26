namespace Yoti.Auth
{
    public class Image
    {
        private readonly byte[] _data;
        private readonly AttrpubapiV1.ContentType _type;

        public Image(AttrpubapiV1.ContentType type, byte[] content)
        {
            _data = content;
            _type = type;
        }

        public AttrpubapiV1.ContentType Type()
        {
            return _type;
        }

        public byte[] GetContent()
        {
            return _data;
        }

        public string GetMIMEType()
        {
            switch (_type)
            {
                case AttrpubapiV1.ContentType.Jpeg:
                    return "image/jpeg";

                case AttrpubapiV1.ContentType.Png:
                    return "image/png";

                default:
                    return "";
            }
        }

        public string Base64URI
        {
            get
            {
                switch (Type())
                {
                    case AttrpubapiV1.ContentType.Jpeg:
                        return "data:image/jpeg;base64," + Conversion.BytesToBase64(GetContent());
                    case AttrpubapiV1.ContentType.Png:
                        return "data:image/png;base64," + Conversion.BytesToBase64(GetContent());
                    default:
                        return null;
                }
            }
        }
    }
}