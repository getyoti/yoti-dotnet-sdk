﻿using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth
{
    public class YotiAttribute<T>
    {
        private readonly string _name;
        private readonly YotiAttributeValue _value;
        private readonly HashSet<string> _sources;
        private readonly HashSet<string> _verifiers;

        public YotiAttribute(string name, YotiAttributeValue value)
        {
            _name = name;
            _value = value;
        }

        public YotiAttribute(string name, YotiAttributeValue value, HashSet<string> sources)
        {
            _name = name;
            _value = value;
            _sources = sources;
        }

        public YotiAttribute(string name, YotiAttributeValue value, HashSet<string> sources, HashSet<string> verifiers)
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
            if (_value == null)
                return null;

            return _value.ToBytes().ChangeType<T>();
        }

        public Dictionary<string, JToken> GetJsonValue()
        {
            return _value.ToJson();
        }

        public Image GetImage()
        {
            return new Image
            {
                Base64URI = Base64URI,
                Data = _value.ToBytes(),
                Type = _value.Type
            };
        }

        public object GetValueOrDefault(object defaultValue)
        {
            return GetValue() ?? defaultValue;
        }

        public HashSet<string> GetSources()
        {
            return _sources;
        }

        public HashSet<string> GetVerifiers()
        {
            return _verifiers;
        }

        public string Base64URI
        {
            get
            {
                switch (_value.Type)

                {
                    case TypeEnum.Jpeg:
                        return "data:image/jpeg;base64," + Conversion.BytesToBase64(_value.ToBytes());
                    case TypeEnum.Png:
                        return "data:image/png;base64," + Conversion.BytesToBase64(_value.ToBytes());
                    default:
                        return null;
                }
            }
        }
    }
}