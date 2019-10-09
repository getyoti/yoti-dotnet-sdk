using System.Collections.Generic;
using Yoti.Auth.Anchors;

namespace Example.Models
{
    public class DisplayAttribute
    {
        private readonly string _displayName;
        private readonly string _preValue;
        private readonly string _icon;
        private readonly List<Anchor> _anchors;
        private readonly object _value;

        public DisplayAttribute(string displayName, string icon, List<Anchor> anchors, object value)
        {
            _displayName = displayName;
            _preValue = "";
            _icon = icon;
            _anchors = anchors;
            _value = value;
        }

        public DisplayAttribute(string preValue, string displayName, string icon, List<Anchor> anchors, object value)
        {
            _displayName = displayName;
            _preValue = preValue;
            _icon = icon;
            _anchors = anchors;
            _value = value;
        }

        public string GetDisplayName()
        {
            return _displayName;
        }

        public string GetPreValue()
        {
            return _preValue;
        }

        public string GetIcon()
        {
            return _icon;
        }

        public List<Anchor> GetAnchors()
        {
            return _anchors;
        }

        public string GetDisplayValue()
        {
            return _preValue + _value.ToString();
        }

        public object GetValue()
        {
            return _value;
        }
    }
}