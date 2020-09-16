using System.Collections.Generic;

namespace Yoti.Auth.ShareUrl.Policy
{
    public class SourceConstraintBuilder
    {
        private readonly List<WantedAnchor> _wantedAnchors = new List<WantedAnchor>();
        private bool _softPreference;

        /// <summary>
        /// Add an anchor to the source constraints list.
        /// This is ordered, from the most preferred one (first in the list)
        /// to the least preferred one (last in the list).
        /// </summary>
        /// <param name="anchor"></param>
        public SourceConstraintBuilder WithAnchor(WantedAnchor anchor)
        {
            _wantedAnchors.Add(anchor);
            return this;
        }

        /// <summary>
        /// If set to false, it means that only anchors in the list are
        /// accepted, in order of preference.
        /// If set to true, it instead means that if none of the anchors
        /// in the list can be satisfied, then any other anchor that is
        /// not in the list is accepted.
        /// </summary>
        /// <param name="softPreference"></param>
        public SourceConstraintBuilder WithSoftPreference(bool softPreference)
        {
            _softPreference = softPreference;
            return this;
        }

        public SourceConstraintBuilder WithAnchorByValue(string value, string subType)
        {
            _wantedAnchors.Add(
                new WantedAnchorBuilder()
                .WithValue(value)
                .WithSubType(subType)
                .Build());

            return this;
        }

        public SourceConstraintBuilder WithPassport(string subType = "")
        {
            return WithAnchorByValue(Constants.DocumentDetails.DocumentTypePassport, subType);
        }

        public SourceConstraintBuilder WithDrivingLicense(string subType = "")
        {
            return WithAnchorByValue(Constants.DocumentDetails.DocumentTypeDrivingLicense, subType);
        }

        public SourceConstraintBuilder WithNationalId(string subType = "")
        {
            return WithAnchorByValue(Constants.DocumentDetails.DocumentTypeNationalId, subType);
        }

        public SourceConstraintBuilder WithPasscard(string subType = "")
        {
            return WithAnchorByValue(Constants.DocumentDetails.DocumentTypePassCard, subType);
        }

        public SourceConstraint Build()
        {
            return new SourceConstraint(_wantedAnchors, _softPreference);
        }
    }
}