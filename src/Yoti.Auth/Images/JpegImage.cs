namespace Yoti.Auth.Images
{
    public class JpegImage : Image
    {
        public JpegImage(byte[] content) : base("image/jpeg", content)
        {
        }
    }
}