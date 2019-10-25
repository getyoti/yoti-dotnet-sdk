using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.ShareUrl.Policy
{
    public class SourceConstraint : Constraint
    {
        private const string _constraintTypeSource = "SOURCE";

        [JsonProperty(PropertyName = "preferred_sources")]
        public PreferredSources PreferredSources { get; private set; }

        public SourceConstraint(List<WantedAnchor> wantedAnchors, bool softPreference) : base(constraintType: _constraintTypeSource)
        {
            PreferredSources = new PreferredSources(wantedAnchors, softPreference);
        }
    }
}