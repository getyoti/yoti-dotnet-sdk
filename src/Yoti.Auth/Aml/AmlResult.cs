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

        public void SetOnFraudList(bool onFraudList)
        {
            _onFraudList = onFraudList;
        }

        public bool IsOnPepList()
        {
            return _onPepList;
        }

        public void SetOnPepList(bool onPepList)
        {
            _onPepList = onPepList;
        }

        public bool IsOnWatchList()
        {
            return _onWatchList;
        }

        public void SetOnWatchList(bool onWatchList)
        {
            _onWatchList = onWatchList;
        }

        public override string ToString()
        {
            return string.Format(
                "SimpleAmlResult{{onFraudList={0}, onPepList={1}, onWatchList={2}}",
                _onFraudList,
                _onPepList,
                _onWatchList);
        }
    }
}