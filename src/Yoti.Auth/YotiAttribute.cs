using System.Collections.Generic;
using System.Linq;
using AttrpubapiV1;
using Google.Protobuf.Collections;
using Newtonsoft.Json.Linq;
using Yoti.Auth.Anchors;
using static Yoti.Auth.Anchors.AnchorCertificateParser;

namespace Yoti.Auth
{
    public class YotiAttribute<T>
    {
        protected readonly YotiAttributeValue Value;
        private readonly string _name;
        private readonly RepeatedField<Anchor> _anchors;

        public YotiAttribute(string name, YotiAttributeValue value)
        {
            _name = name;
            Value = value;
        }

        public YotiAttribute(string name, YotiAttributeValue value, RepeatedField<Anchor> anchors)
        {
            _name = name;
            Value = value;
            _anchors = anchors;
        }

        public string GetName()
        {
            return _name;
        }

        public object GetValue()
        {
            if (Value == null)
                return null;

            return Value.ToBytes().ChangeType<T>();
        }

        public Dictionary<string, JToken> GetJsonValue()
        {
            return Value.ToJson();
        }

        public object GetValueOrDefault(object defaultValue)
        {
            return GetValue() ?? defaultValue;
        }

        public HashSet<string> GetSources()
        {
            var sources = new HashSet<string>();

            foreach (Anchor anchor in _anchors)
            {
                AnchorVerifierSourceData anchorTypes = AnchorCertificateParser.GetTypesFromAnchor(anchor, AnchorType.Source);
                sources.UnionWith(anchorTypes.GetEntries());
            }

            return sources;
        }

        public HashSet<string> GetVerifiers()
        {
            var verifiers = new HashSet<string>();

            foreach (Anchor anchor in _anchors)
            {
                AnchorVerifierSourceData anchorTypes = AnchorCertificateParser.GetTypesFromAnchor(anchor, AnchorType.Verifier);
                verifiers.UnionWith(anchorTypes.GetEntries());
            }

            return verifiers;
        }

        internal RepeatedField<Anchor> GetRawAnchors()
        {
            return _anchors;
        }

        public List<Anchor> GetAnchors()
        {
            return _anchors.Cast<Anchor>().ToList();
        }
    }
}