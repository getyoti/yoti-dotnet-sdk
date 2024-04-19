using System.Collections.Generic;
using Yoti.Auth.DigitalIdentity;

/*
namespace Yoti.Auth.DigitalIdentity.Attribute
{
    
    public class StringAttribute
    {
        private readonly AttributeDetails _attributeDetails;
        public string Value { get; set; }

        // NewString creates a new String attribute
        public static StringAttribute NewString(Yoti.Auth.ProtoBuf.Attribute.Attribute a)
        {
            var parsedAnchors = Anchor.ParseAnchors(a.Anchors);

            return new StringAttribute
            {
                _attributeDetails = new AttributeDetails
                {
                    Name = a.Name,
                    ContentType = a.ContentType.ToString(),
                    Anchors = parsedAnchors,
                    Id = a.EphemeralId,
                },
                Value = System.Text.Encoding.UTF8.GetString(a.Value)
            };
        }

        // Value returns the value of the StringAttribute as a string
        public string GetValue()
        {
            return Value;
        }
    }
}

*/
