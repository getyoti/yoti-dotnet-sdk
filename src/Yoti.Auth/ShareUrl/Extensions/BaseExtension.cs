using Newtonsoft.Json;

namespace Yoti.Auth.ShareUrl.Extensions
{
    public abstract class BaseExtension
    {
        [JsonProperty(PropertyName = "type")]
        private readonly string _type;

        private protected BaseExtension(string type)
        {
            _type = type;
        }

        /// <summary>
        /// Get the feature's type
        /// </summary>
        [JsonIgnore]
        public string ExtensionType
        {
            get
            {
                return _type;
            }
        }
    }
}