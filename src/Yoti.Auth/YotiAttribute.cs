using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Yoti.Auth
{
    public class YotiAttribute<T>
    {
        protected readonly YotiAttributeValue Value;
        private readonly string _name;
        private readonly HashSet<string> _sources;
        private readonly HashSet<string> _verifiers;

        public YotiAttribute(string name, YotiAttributeValue value)
        {
            _name = name;
            Value = value;
        }

        public YotiAttribute(string name, YotiAttributeValue value, HashSet<string> sources)
        {
            _name = name;
            Value = value;
            _sources = sources;
        }

        public YotiAttribute(string name, YotiAttributeValue value, HashSet<string> sources, HashSet<string> verifiers)
        {
            _name = name;
            Value = value;
            _sources = sources;
            _verifiers = verifiers;
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

        public HashSet<string> GetSources()
        {
            return _sources;
        }

        public HashSet<string> GetVerifiers()
        {
            return _verifiers;
        }
    }
}