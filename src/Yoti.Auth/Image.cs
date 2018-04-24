using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth
{
    public class Image
    {
        public TypeEnum Type { get; set; }

        public byte[] Data { get; set; }
        public string Base64URI { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}