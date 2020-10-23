using System.Collections.Generic;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Create.Objectives;

namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public class RequiredSupplementaryDocument : RequiredDocument
    {
        [JsonProperty(PropertyName = "objective")]
        public Objective Objective { get; }

        [JsonProperty(PropertyName = "document_types")]
        public List<string> DocumentTypes { get; }

        [JsonProperty(PropertyName = "country_codes")]
        public List<string> CountryCodes { get; }

        public RequiredSupplementaryDocument(Objective objective, List<string> documentTypes, List<string> countryCodes)
        {
            Objective = objective;
            DocumentTypes = documentTypes;
            CountryCodes = countryCodes;
        }

        public override string Type => Constants.DocScanConstants.SupplementaryDocument;
    }
}