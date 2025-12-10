using System;
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
        public DateTime? CreatedAt { get; internal set; }

        [JsonProperty(PropertyName = "last_updated")]
        public DateTime? LastUpdated { get; internal set; }

        [JsonProperty(PropertyName = "lookup_profile")]
        public ProfileResponse LookupProfile { get; internal set; }

        [JsonProperty(PropertyName = "returned_profile")]
        public ProfileResponse ReturnedProfile { get; internal set; }

        [JsonProperty(PropertyName = "id_photo")]
        public IdPhotoResponse IdPhoto { get; internal set; }

        [JsonProperty(PropertyName = "file")]
        public FileResponse File { get; internal set; }

        /// <summary>
        /// Filters the tasks for the share code verification tasks
        /// </summary>
        /// <returns>Returns a list of share code verification tasks</returns>
        public List<VerifyShareCodeTaskResponse> GetVerifyShareCodeTasks()
        {
            if (Tasks == null)
                return new List<VerifyShareCodeTaskResponse>();

            return Tasks.OfType<VerifyShareCodeTaskResponse>().ToList();
        }
    }
}
