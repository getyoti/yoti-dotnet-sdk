using Newtonsoft.Json;

namespace Yoti.Auth.Aml
{
    public class AmlResult : IAmlResult
    {
        [JsonProperty(PropertyName = "on_fraud_list")]
        private bool _onFraudList;

        [JsonProperty(PropertyName = "on_pep_list")]
        private bool _onPepList;

        [JsonProperty(PropertyName = "on_watch_list")]
        private bool _onWatchList;

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