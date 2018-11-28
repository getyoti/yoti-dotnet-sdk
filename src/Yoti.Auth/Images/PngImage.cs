using AttrpubapiV1;

namespace Yoti.Auth.Images
{
    public class PngImage : Image
    {
        public PngImage(byte[] content) : base(ContentType.Png, content)
        {
        }

        public override string GetMIMEType()
        {
            return "image/png";
        }
    }
}