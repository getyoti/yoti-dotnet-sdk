using System.Collections.Generic;
using Yoti.Auth.Anchors;

namespace CoreExample.Models
{
    public class DisplayAttributes
    {
        public List<DisplayAttribute> AttributeList { get; internal set; }
        public string Base64Selfie { get; internal set; }
        public string FullName { get; internal set; }

        internal DisplayAttributes()
        {
            AttributeList = new List<DisplayAttribute>();
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

