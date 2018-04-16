using System;
using System.Reflection;

namespace Yoti.Auth
{
    public class YotiAttribute<T>
    {
        private readonly string _name;
        private readonly Object _value;
        private readonly string _sources;
        private readonly string _verifiers;

        public YotiAttribute(string name, Object value)
        {
            _name = name;
            _value = value;
        }

        public YotiAttribute(string name, Object value, string sources)
        {
            _name = name;
            _value = value;
            _sources = sources;
        }

        public YotiAttribute(string name, Object value, string sources, string verifiers)
        {
            _name = name;
            _value = value;
            _sources = sources;
            _verifiers = verifiers;
        }

        public string GetName()
        {
            return _name;
        }

        public object GetValue()
        {
            Type type = _value.GetType();

            return type.IsAssignableFrom((Type)_value) ? (Type)_value : null;
        }

        public object GetValueOrDefault(object defaultValue)
        {
            return GetValue() ?? defaultValue;
        }

        public string GetSources()
        {
            return _sources;
        }

        public string GetVerifiers()
        {
            return _verifiers;
        }
    }
}