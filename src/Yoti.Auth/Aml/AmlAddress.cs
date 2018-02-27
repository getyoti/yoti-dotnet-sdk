using Newtonsoft.Json;

namespace Yoti.Auth.Aml
{
    public class AmlAddress : IAmlAddress
    {
        [JsonProperty(PropertyName = "post_code")]
        private string _postcode;

        [JsonRequired]
        [JsonProperty(PropertyName = "country")]
        private string _country;

        public AmlAddress(string country, string postcode = null)
        {
            _postcode = postcode;
            _country = country;
        }

        public string GetCountry()
        {
            return _country;
        }

        public string GetPostcode()
        {
            return _postcode;
        }
    }
}