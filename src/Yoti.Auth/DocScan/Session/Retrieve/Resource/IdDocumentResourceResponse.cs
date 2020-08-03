using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve.Task;

namespace Yoti.Auth.DocScan.Session.Retrieve.Resource
{
    /// <summary>
    /// Represents an Identity Document resource for a given session
    /// </summary>
    public class IdDocumentResourceResponse : ResourceResponse
    {
        [JsonProperty(PropertyName = "document_type")]
        public string DocumentType { get; internal set; }

        [JsonProperty(PropertyName = "issuing_country")]
        public string IssuingCountry { get; internal set; }

        [JsonProperty(PropertyName = "pages")]
        public List<PageResponse> Pages { get; internal set; }

        [JsonProperty(PropertyName = "document_fields")]
        public DocumentFieldsResponse DocumentFields { get; internal set; }

        [JsonProperty(PropertyName = "document_id_photo")]
        public DocumentIdPhotoResponse DocumentIdPhoto { get; internal set; }

        public List<TextExtractionTaskResponse> GetTextExtractionTasks()
        {
            return Tasks?.OfType<TextExtractionTaskResponse>()?.ToList();
        }
    }
}