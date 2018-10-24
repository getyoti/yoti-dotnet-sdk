using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth
{
    public class Image
    {
        public TypeEnum Type { get; set; }

        public byte[] Data { get; set; }

        public string Base64URI
        {
            get
            {
                switch (Type)
                {
                    case TypeEnum.Jpeg:
                        return "data:image/jpeg;base64," + Conversion.BytesToBase64(Data);
                    case TypeEnum.Png:
                        return "data:image/png;base64," + Conversion.BytesToBase64(Data);
                    default:
                        return null;
                }
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}