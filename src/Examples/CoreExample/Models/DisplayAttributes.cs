using System.Collections.Generic;
using Yoti.Auth.Anchors;

namespace CoreExample.Models
{
    public class DisplayAttributes
    {
        public List<DisplayAttribute> AttributeList;
        public string _base64Selfie;
        public string _fullName;

        internal DisplayAttributes()
        {
            AttributeList = new List<DisplayAttribute>();
        }

        public string Base64Selfie
        {
            get
            {
                return _base64Selfie;
            }
            internal set
            {
                _base64Selfie = value;
            }
        }

        public string FullName
        {
            get
            {
                return _fullName;
            }
            internal set
            {
                _fullName = value;
            }
        }

        internal void Add(DisplayAttribute displayAttribute)
        {
            AttributeList.Add(displayAttribute);
        }

        internal void Add(string displayName, string icon, List<Anchor> anchors, object value)
        {
            DisplayAttribute displayAttribute = new DisplayAttribute(displayName, icon, anchors, value);
            AttributeList.Add(displayAttribute);
        }
    }
}