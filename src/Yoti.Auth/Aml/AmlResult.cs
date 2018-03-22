using Newtonsoft.Json;

namespace Yoti.Auth.Aml
{
    public class AmlResult : IAmlResult
    {
#pragma warning disable 0649

        // These fields are assigned to by JSON deserialization
        [JsonProperty(PropertyName = "on_fraud_list")]
        private bool _onFraudList;

        [JsonProperty(PropertyName = "on_pep_list")]
        private bool _onPepList;

        [JsonProperty(PropertyName = "on_watch_list")]
        private bool _onWatchList;

#pragma warning restore 0649

        public bool IsOnFraudList()
        {
            return _onFraudList;
        }

        public bool IsOnPepList()
        {
            return _onPepList;
        }

        public bool IsOnWatchList()
        {
            return _onWatchList;
        }
    }
}