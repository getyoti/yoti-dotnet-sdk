namespace Yoti.Auth
{
    public enum ImageType { Jpeg, Png }

    public class Image
    {
        public ImageType Type { get; set; }
        public byte[] Data { get; set; }
        public string Base64Data { get; set; }
    }
}