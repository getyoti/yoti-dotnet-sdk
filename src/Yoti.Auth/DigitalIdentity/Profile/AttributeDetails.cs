/*
using System.Collections.Generic;

namespace Yoti.Auth.DigitalIdentity
{
    public class AttributeDetails
    {
        private readonly string _name;
        private readonly string _contentType;
        private readonly List<Anchor> _anchors;
        private readonly string _id;

        public AttributeDetails(string name, string contentType, List<Anchor> anchors, string id)
        {
            _name = name;
            _contentType = contentType;
            _anchors = anchors;
            _id = id;
        }

        public string Name() => _name;

        public string ID() => _id;

        public string ContentType() => _contentType;

        public List<Anchor> Anchors() => _anchors;

        public List<Anchor> Sources() => AnchorExtensions.GetSources(_anchors);

        public List<Anchor> Verifiers() => AnchorExtensions.GetVerifiers(_anchors);
    }
}

*/
