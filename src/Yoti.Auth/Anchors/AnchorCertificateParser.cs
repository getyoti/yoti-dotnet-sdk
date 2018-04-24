using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using AttrpubapiV1;
using Google.Protobuf;
using Org.BouncyCastle.Asn1;

namespace Yoti.Auth.Anchors
{
    public static class AnchorCertificateParser
    {
        public static AnchorVerifierSourceData GetTypesFromAnchor(Anchor anchor)
        {
            var types = new HashSet<string>();
            AnchorType anchorType = AnchorType.Unknown;

            foreach (ByteString byteString in anchor.OriginServerCerts)
            {
                var extensions = new List<string>();

                X509Certificate2 certificate = new X509Certificate2(byteString.ToByteArray());

                foreach (AnchorType type in Enum.GetValues(typeof(AnchorType)))
                {
                    string extensionOid = type.GetType().GetTypeInfo().GetCustomAttribute(typeof(ExtensionOidAttribute)).ToString();
                    extensions = GetListOfStringsFromExtension(certificate, extensionOid);

                    if (extensions.Count() > 0)
                    {
                        anchorType = type;
                        break;
                    }
                }
                types.UnionWith(extensions);
            }

            return new AnchorVerifierSourceData(types, anchorType);
        }

        private static List<string> GetListOfStringsFromExtension(X509Certificate2 certificate, string extensionOid)
        {
            var extensionStrings = new List<string>();

            byte[] extensionBytes = certificate.Extensions.OfType<X509Extension>().FirstOrDefault(ext => ext.Oid.ToString() == extensionOid).RawData;

            if (extensionBytes != null)
            {
                var asn1InputStream = new Asn1InputStream(extensionBytes);

                // Distinguished Encoding Rules (DER) object
                Asn1Object derObject = asn1InputStream.ReadObject();

                if (derObject != null && derObject is DerOctetString)
                {
                    var derOctetString = (DerOctetString)derObject;

                    // Read the sub object which is expected to be a sequence
                    Asn1InputStream derAsn1stream = new Asn1InputStream(derOctetString.GetOctets());
                    Asn1Sequence asn1Sequence = (Asn1Sequence)derAsn1stream.ReadObject();

                    // Enumerate all the objects in the sequence, we expect only one
                    foreach (Asn1TaggedObject obj in asn1Sequence)
                    {
                        // This object is OctetString we are looking for
                        Asn1OctetString octetString = DerOctetString.GetInstance(obj, isExplicit: false);

                        // Convert to string
                        string stringValue = Convert.ToString(octetString);
                        extensionStrings.Add(stringValue);
                    }
                }
            }

            return extensionStrings;
        }

        public class AnchorVerifierSourceData
        {
            private readonly HashSet<string> _entries;

            private readonly AnchorType _type;

            public AnchorVerifierSourceData(HashSet<string> entries, AnchorType anchorType)
            {
                _entries = entries;
                _type = anchorType;
            }

            public HashSet<string> GetEntries()
            {
                return _entries;
            }

            public AnchorType GetAnchorType()
            {
                return _type;
            }
        }

        public static Asn1Object FindAsn1Value(string oid, Asn1Object obj)
        {
            Asn1Object result = null;
            if (obj is Asn1Sequence)
            {
                bool foundOID = false;
                foreach (Asn1Object entry in (Asn1Sequence)obj)
                {
                    var derOID = entry as DerObjectIdentifier;
                    if (derOID != null && derOID.Id == oid)
                    {
                        foundOID = true;
                    }
                    else if (foundOID)
                    {
                        return entry;
                    }
                    else
                    {
                        result = FindAsn1Value(oid, entry);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }
            else if (obj is DerTaggedObject)
            {
                result = FindAsn1Value(oid, ((DerTaggedObject)obj).GetObject());
                if (result != null)
                {
                    return result;
                }
            }
            else
            {
                if (obj is DerSet)
                {
                    foreach (Asn1Object entry in (DerSet)obj)
                    {
                        result = FindAsn1Value(oid, entry);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }
            return null;
        }
    }
}