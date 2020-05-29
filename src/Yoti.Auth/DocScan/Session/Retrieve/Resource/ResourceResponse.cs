using System.Collections.Generic;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve.Task;

namespace Yoti.Auth.DocScan.Session.Retrieve.Resource
{
    public class ResourceResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; internal set; }

        [JsonProperty(PropertyName = "tasks")]
        public List<TaskResponse> Tasks { get; internal set; }
    }
}