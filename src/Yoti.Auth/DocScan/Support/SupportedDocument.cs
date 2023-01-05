using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Support
{
    public class SupportedDocument
    {
        public SupportedDocument(string type, bool isStrictlyLatin = false)
        {
            Type = type;
            IsStrictlyLatin = isStrictlyLatin;
        }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; }

        [JsonProperty(PropertyName = "is_strictly_latin")]
<<<<<<< HEAD
        public bool IsStrictlyLatin { get; private set; }
=======
        public string IsStrictlyLatin { get; }


>>>>>>> origin/development
    }
}