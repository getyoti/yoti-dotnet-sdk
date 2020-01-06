using System.Collections.Generic;
using Yoti.Auth.Anchors;

namespace _47Example.Models
{
    public class DisplayAttributes
    {
        internal DisplayAttributes()
        {
            AttributeList = new List<DisplayAttribute>();
        }

        public List<DisplayAttribute> AttributeList { get; internal set; }
        public string Base64Selfie { get; internal set; }
        public string FullName { get; internal set; }

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