using System;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth
{
    public class Image
    {
        public TypeEnum Type { get; set; }

        public byte[] Data { get; set; }

        [Obsolete("Will be removed in version 3.0.0. Please use Selfie.GetBase64URI() instead")]
        public string Base64URI { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}