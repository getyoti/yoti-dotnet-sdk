using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public abstract class DocumentFilter
    {
        protected DocumentFilter(string type)
        {
            Validation.NotNull(type, nameof(type));
            Type = type;
        }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; internal set; }
    }
}