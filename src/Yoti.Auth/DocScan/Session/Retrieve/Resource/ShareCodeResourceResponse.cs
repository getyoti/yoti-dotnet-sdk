using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve.Task;

namespace Yoti.Auth.DocScan.Session.Retrieve.Resource
{
    /// <summary>
    /// Represents a Share Code resource for a given session
    /// </summary>
    public class ShareCodeResourceResponse : ResourceResponse
    {
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; internal set; }

        [JsonProperty(PropertyName = "last_updated")]
        public string LastUpdated { get; internal set; }

        [JsonProperty(PropertyName = "lookup_profile")]
        public ShareCodeMediaResponse LookupProfile { get; internal set; }

        [JsonProperty(PropertyName = "returned_profile")]
        public ShareCodeMediaResponse ReturnedProfile { get; internal set; }

        [JsonProperty(PropertyName = "id_photo")]
        public ShareCodeMediaResponse IdPhoto { get; internal set; }

        [JsonProperty(PropertyName = "file")]
        public ShareCodeMediaResponse File { get; internal set; }

        /// <summary>
        /// Filters the tasks for the verify share code tasks associated with this resource
        /// </summary>
        /// <returns>Returns a list of verify share code tasks</returns>
        public List<VerifyShareCodeTaskResponse> GetVerifyShareCodeTasks()
        {
            if (Tasks == null)
                return new List<VerifyShareCodeTaskResponse>();

            return Tasks.OfType<VerifyShareCodeTaskResponse>().ToList();
        }
    }
}
