using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yoti.Auth.Aml
{
    public class AmlAddress : IAmlAddress
    {
        [JsonProperty(PropertyName = "post_code")]
        private string _postcode;

        [JsonRequired]
        [JsonProperty(PropertyName = "country")]
        private string _country;

        public AmlAddress(string postcode, string country)
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

        public override string ToString()
        {
            return "SimpleAmlAddress{" +
                "postcode='" + _postcode + '\'' +
                ", country='" + _country + '\'' +
                '}';
        }
    }
}