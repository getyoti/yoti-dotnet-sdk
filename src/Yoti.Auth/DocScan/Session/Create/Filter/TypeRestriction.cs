using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public class TypeRestriction
    {
        public TypeRestriction(string inclusion, List<string> documentTypes)
        {
            Validation.NotNull(inclusion, nameof(inclusion));
            Validation.CollectionNotEmpty(documentTypes, nameof(documentTypes));

            Inclusion = inclusion;
            DocumentTypes = documentTypes;
        }

        [JsonProperty(PropertyName = "inclusion")]
        public string Inclusion { get; }

        [JsonProperty(PropertyName = "document_types")]
        public List<string> DocumentTypes { get; }
    }
}