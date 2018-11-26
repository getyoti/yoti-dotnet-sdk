namespace Yoti.Auth
{
    public class Image
    {
        public Image(AttrpubapiV1.ContentType type, byte[] content)
        {
            Content = content;
            Type = type;
        }

        public AttrpubapiV1.ContentType Type { get; private set; }

        public byte[] Content { get; private set; }

        public string MIMEType
        {
            get
            {
                switch (Type)
                {
                    case AttrpubapiV1.ContentType.Jpeg:
                        return "image/jpeg";

                    case AttrpubapiV1.ContentType.Png:
                        return "image/png";

                    default:
                        return "";
                }
            }
        }

        public string Base64URI
        {
            get
            {
                switch (Type)
                {
                    case AttrpubapiV1.ContentType.Jpeg:
                        return "data:image/jpeg;base64," + Conversion.BytesToBase64(Content);
                    case AttrpubapiV1.ContentType.Png:
                        return "data:image/png;base64," + Conversion.BytesToBase64(Content);
                    default:
                        return null;
                }
            }
        }
    }
}