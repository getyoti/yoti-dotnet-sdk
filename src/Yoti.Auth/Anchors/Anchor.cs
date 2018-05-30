using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Yoti.Auth.Anchors
{
    /// <summary>
    /// <para>A class to represent a Yoti anchor. Anchors are metadata associated
    /// to the attribute, which describe how an attribute has been provided
    /// to Yoti (SOURCE Anchor) and how it has been verified (VERIFIER Anchor)</para>
    /// <para>If an attribute has only one SOURCE Anchor with the value set to
    /// "USER_PROVIDED" and zero VERIFIER Anchors, then the attribute
    /// is a self-certified one</para>
    /// </summary>
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

        /// <summary>
        /// Gets the <see cref="AnchorType"/> of the given anchor
        /// </summary>
        /// <returns>The <see cref="AnchorType"/> of the given anchor</returns>
        public AnchorType GetAnchorType()
        {
            return _anchorType;
        }

        /// <summary>
        /// Gets the value of the given anchor.
        /// <para>For <see cref="AnchorType.Source"/> this can be "USER_PROVIDED", "PASSPORT",
        /// "DRIVING_LICENCE" or "AADHAAR"</para>
        /// <para>For <see cref="AnchorType.Verifier"/> this
        /// can be "YOTI_ADMIN", "YOTI_IDENTITY", "YOTI_OTP", "YOTI_UIDAI" or "PASSPORT_NFC_SIGNATURE"</para>
        /// </summary>
        /// <returns>A list of strings of the value(s) for this anchor type</returns>
        public List<string> GetValue()
        {
            return _value;
        }

        /// <summary>
        /// ArtifactLink gives the database ID of the Yoti stored copy of the backing artifact/document.
        /// </summary>
        /// <returns>The database ID as a byte array</returns>
        public byte[] GetArtifactLink()
        {
            return _artifactLink;
        }

        /// <summary>
        /// ArtifactSignature is a signature over the backing artifact. When a
        /// client validates the Signature field, the data being signed
        /// encompasses this signature. When the Yoti server validates the
        /// Signature field, it can further verify that this signature matches
        /// the one in the source artifact itself.
        /// </summary>
        /// <returns>An artifact signature as a byte array</returns>
        public byte[] GetArtifactSignature()
        {
            return _artifactSignature;
        }

        /// <summary>
        /// <para>OriginServerCerts are the X.509 certificate chain(DER-encoded ASN.1)
        /// from the service that assigned the attribute</para>
        ///
        /// <para>The first certificate in the chain holds the public key that can be
        /// used to verify the Signature field; any following entries (zero or
        /// more) are for intermediate certificate authorities (in order). The
        /// last certificate in the chain must be verified against the Yoti root
        /// CA certificate</para>
        ///
        /// <para>An extension in the first certificate holds the main artifact type,
        /// e.g. “PASSPORT”, which can alternatively be retrieved with <see cref="GetValue()"/></para>
        /// </summary>
        /// <returns>The X509 certificate chain from the service that assigned the attribute</returns>
        public List<X509Certificate2> GetOriginServerCerts()
        {
            return _originServerCerts;
        }

        /// <summary>
        /// Signature is a marshaled signature message which
        /// contains an RSA signature (currently SHA512WithRSA) along with the
        /// hash+sign algorithm details. The signature is over a subset of fields
        /// in the <see cref="YotiAttribute{T}"/>.
        /// </summary>
        /// <returns>Marshaled signature message in a byte array</returns>
        public byte[] GetSignature()
        {
            return _signature;
        }

        /// <summary>
        /// SignedTimeStamp is the time at which the signature was created. The
        /// message associated with the timestamp is the marshaled form of
        /// AttributeSigning (i.e. the same message that is signed in the
        /// Signature field).
        /// </summary>
        /// <returns>The signed timestamp in a byte array</returns>
        public byte[] GetSignedTimeStamp()
        {
            return _signedTimeStamp;
        }

        /// <summary>
        /// SubType is an indicator of any specific processing method, or
        /// subcategory, pertaining to an artifact. For example, for a passport, this would be
        /// either "NFC" or "OCR".
        /// </summary>
        /// <returns>The subtype of an artifact</returns>
        public string GetSubType()
        {
            return _subType;
        }
    }
}