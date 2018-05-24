using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Yoti.Auth.Anchors;

namespace Yoti.Auth
{
    public class YotiAttribute<T>
    {
        protected readonly YotiAttributeValue Value;
        private readonly string _name;
        private readonly List<Anchor> _anchors;

        public YotiAttribute(string name, YotiAttributeValue value)
        {
            _name = name;
            Value = value;
        }

        public YotiAttribute(string name, YotiAttributeValue value, List<Anchor> anchors)
        {
            _name = name;
            Value = value;
            _anchors = anchors;
        }

        public string GetName()
        {
            return _name;
        }

        public T GetValue()
        {
            if (Value == null)
                return default(T);

            return Value.ToBytes().ConvertType<T>();
        }

        public Dictionary<string, JToken> GetJsonValue()
        {
            return Value.ToJson();
        }

        public T GetValueOrDefault(T defaultValue)
        {
            T value = GetValue();

            if (value != null)
                return value;

            return defaultValue;
        }

        public List<Anchor> GetAnchors()
        {
            return _anchors.ToList();
        }

        public List<Anchor> GetSources()
        {
            return _anchors.Where(a => a.GetAnchorType() == AnchorType.Source).ToList();
        }

        public List<Anchor> GetVerifiers()
        {
            return _anchors.Where(a => a.GetAnchorType() == AnchorType.Verifier).ToList();
        }
    }
}