using System.Collections.Generic;

namespace Yoti.Auth.Anchors
{
    public class AnchorVerifierSourceData
    {
        private readonly HashSet<string> _entries;
        private readonly AnchorType _type;

        public AnchorVerifierSourceData(HashSet<string> entries, AnchorType anchorType)
        {
            _entries = entries;
            _type = anchorType;
        }

        public HashSet<string> GetEntries()
        {
            return _entries;
        }

        public AnchorType GetAnchorType()
        {
            return _type;
        }
    }
}