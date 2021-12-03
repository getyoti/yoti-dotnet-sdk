using Newtonsoft.Json;
using System.Collections.Generic;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Task;

namespace Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Document
{
    public abstract class RequiredDocumentResourceResponse : RequiredResourceResponse
    {
        /// <summary>
        /// Tasks that need to be completed as part of the document requirement
        /// </summary>
        [JsonProperty(PropertyName = "requested_tasks")]
        public List<RequestedTaskResponse> RequestedTasks { get; private set; }
    }
}