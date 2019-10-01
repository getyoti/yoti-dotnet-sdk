using System;
using Yoti.Auth.Anchors;

namespace Yoti.Auth.Sandbox.Profile.Request.Attribute
{
    public class SandboxAnchorBuilder
    {
        private string _type;
        private string _value;
        private string _subType;
        private long _unixMicrosecondTimestamp;

        internal SandboxAnchorBuilder()
        {
        }

        public SandboxAnchorBuilder WithType(AnchorType anchorType)
        {
            _type = anchorType.ToString();
            return this;
        }

        public SandboxAnchorBuilder WithType(string type)
        {
            _type = type;
            return this;
        }

        public SandboxAnchorBuilder WithValue(string value)
        {
            _value = value;
            return this;
        }

        public SandboxAnchorBuilder WithSubType(string subType)
        {
            _subType = subType;
            return this;
        }

        public SandboxAnchorBuilder WithTimestamp(DateTime dateTime)
        {
            _unixMicrosecondTimestamp = Timestamp.GetUnixTimeMicroseconds(dateTime);
            return this;
        }

        public SandboxAnchor Build()
        {
            return new SandboxAnchor(_type, _value, _subType, _unixMicrosecondTimestamp);
        }
    }
}