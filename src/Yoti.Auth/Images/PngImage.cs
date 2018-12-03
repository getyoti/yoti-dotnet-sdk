namespace Yoti.Auth.Images
{
    public class PngImage : Image
    {
        public PngImage(byte[] content) : base(content)
        {
        }

        public override string GetMIMEType()
        {
            return "image/png";
        }
    }
}