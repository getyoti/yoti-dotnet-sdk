namespace Yoti.Auth
{
    internal class Response
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Content { get; set; }
        public string ReasonPhrase { get; set; }
    }
}