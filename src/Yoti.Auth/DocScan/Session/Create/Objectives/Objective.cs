using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Objectives
{
    public class Objective
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; }

        public Objective(string type)
        {
            Type = type;
        }
    }
}