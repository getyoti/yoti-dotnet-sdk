using AttrpubapiV1;

namespace Yoti.Auth.Images
{
    public class JpegImage : Image
    {
        public JpegImage(byte[] content) : base(ContentType.Jpeg, content)
        {
        }

        public override string GetMIMEType()
        {
            return "image/jpeg";
        }
    }
}