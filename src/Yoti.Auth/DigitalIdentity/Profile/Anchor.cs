/*
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Yoti.Auth.DigitalIdentity
{
    public class Anchor
    {
        private readonly Type _anchorType;
        private readonly List<X509Certificate2> _originServerCerts;
        private readonly SignedTimestamp _signedTimestamp;
        private readonly string _subtype;
        private readonly string _value;

        public Anchor(Type anchorType, List<X509Certificate2> originServerCerts, SignedTimestamp signedTimestamp, string subtype, string value)
        {
            _anchorType = anchorType;
            _originServerCerts = originServerCerts;
            _signedTimestamp = signedTimestamp;
            _subtype = subtype;
            _value = value;
        }

        public Type Type() => _anchorType;

        public List<X509Certificate2> OriginServerCerts() => _originServerCerts;

        public SignedTimestamp SignedTimestamp() => _signedTimestamp;

        public string SubType() => _subtype;

        public string Value() => _value;
    }

    public enum Type
    {
        Unknown = 1,
        Source,
        Verifier
    }

    public static class AnchorExtensions
    {
        public static List<Anchor> GetSources(List<Anchor> anchors) => FilterAnchors(anchors, Type.Source);

        public static List<Anchor> GetVerifiers(List<Anchor> anchors) => FilterAnchors(anchors, Type.Verifier);

        private static List<Anchor> FilterAnchors(List<Anchor> anchors, Type anchorType)
        {
            List<Anchor> result = new List<Anchor>();
            foreach (var anchor in anchors)
            {
                if (anchor.Type() == anchorType)
                    result.Add(anchor);
            }
            return result;
        }
    }
}

*/
