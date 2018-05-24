using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Yoti.Auth.Anchors
{
    public class Anchor
    {
        private readonly AnchorType _anchorType;
        private readonly byte[] _artifactLink;
        private readonly byte[] _artifactSignature;
        private readonly List<X509Certificate2> _originServerCerts;
        private readonly byte[] _signature;
        private readonly byte[] _signedTimeStamp;
        private readonly string _subType;
        private readonly List<string> _value;

        public Anchor(AttrpubapiV1.Anchor protobufAnchor)
        {
            AnchorVerifierSourceData anchorSourceData = AnchorCertificateParser.GetTypesFromAnchor(protobufAnchor);

            _anchorType = anchorSourceData.GetAnchorType();
            _value = anchorSourceData.GetEntries().ToList();

            _artifactLink = protobufAnchor.ArtifactLink.ToByteArray();
            _artifactSignature = protobufAnchor.ArtifactSignature.ToByteArray();
            _signature = protobufAnchor.Signature.ToByteArray();
            _signedTimeStamp = protobufAnchor.SignedTimeStamp.ToByteArray();
            _subType = protobufAnchor.SubType;
            _originServerCerts = ConvertRawCertToX509List(protobufAnchor.OriginServerCerts);
        }

        private List<X509Certificate2> ConvertRawCertToX509List(RepeatedField<ByteString> rawOriginServerCerts)
        {
            var X509originServerCerts = new List<X509Certificate2>();
            foreach (ByteString byteString in rawOriginServerCerts)
            {
                X509Certificate2 certificate = new X509Certificate2(byteString.ToByteArray());
                X509originServerCerts.Add(certificate);
            }
            return X509originServerCerts;
        }

        public AnchorType GetAnchorType()
        {
            return _anchorType;
        }

        public List<string> GetValue()
        {
            return _value;
        }

        public byte[] GetArtifactLink()
        {
            return _artifactLink;
        }

        public byte[] GetArtifactSignature()
        {
            return _artifactSignature;
        }

        public List<X509Certificate2> GetOriginServerCerts()
        {
            return _originServerCerts;
        }

        public byte[] GetSignature()
        {
            return _signature;
        }

        public byte[] GetSignedTimeStamp()
        {
            return _signedTimeStamp;
        }

        public string GetSubType()
        {
            return _subType;
        }
    }
}