namespace Yoti.Auth.Images
{
    public class JpegImage : Image
    {
        public JpegImage(byte[] content) : base(content)
        {
        }

        public override string GetMIMEType()
        {
            return "image/jpeg";
        }
    }
}