namespace Yoti.Auth.Images
{
    public abstract class Image : MediaValue
    {
        protected Image(string mimeType, byte[] content) : base(mimeType, content)
        {
        }
    }
}