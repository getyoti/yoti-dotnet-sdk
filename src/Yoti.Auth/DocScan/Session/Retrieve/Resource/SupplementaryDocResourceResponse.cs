using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve.Task;

namespace Yoti.Auth.DocScan.Session.Retrieve.Resource
{
    /// <summary>
    /// Represents a Supplementary Document resource for a given session
    /// </summary>
    public class SupplementaryDocResourceResponse : ResourceResponse
    {
        [JsonProperty(PropertyName = "document_type")]
        public string DocumentType { get; internal set; }

        [JsonProperty(PropertyName = "issuing_country")]
        public string IssuingCountry { get; internal set; }

        [JsonProperty(PropertyName = "pages")]
        public List<PageResponse> Pages { get; internal set; } = new List<PageResponse>();

        [JsonProperty(PropertyName = "document_fields")]
        public DocumentFieldsResponse DocumentFields { get; internal set; }

        [JsonProperty(PropertyName = "file")]
        public FileResponse DocumentFile { get; internal set; }

        /// <summary>
        /// Filters the tasks for the text extraction tasks associated with the ID document
        /// </summary>
        /// <returns>Returns a list of text extraction tasks</returns>
        public List<SupplementaryDocTextExtractionTaskResponse> GetTextExtractionTasks()
        {
            if (Tasks == null)
                return new List<SupplementaryDocTextExtractionTaskResponse>();

            return Tasks.OfType<SupplementaryDocTextExtractionTaskResponse>().ToList();
        }
    }
}