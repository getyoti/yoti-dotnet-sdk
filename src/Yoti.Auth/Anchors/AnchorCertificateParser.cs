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
    internal static class AnchorCertificateParser
    {
        public static AnchorVerifierSourceData GetTypesFromAnchor(Anchor anchor, AnchorType type)
        {
            var types = new HashSet<string>();
            AnchorType anchorType = AnchorType.Unknown;

            foreach (ByteString byteString in anchor.OriginServerCerts)
            {
                X509Certificate2 certificate = new X509Certificate2(byteString.ToByteArray());
                var extensions = new List<string>();

                foreach (AnchorType enumType in Enum.GetValues(typeof(AnchorType)))
                {
                    extensions = GetExtensions(enumType, certificate, extensions);

                    if (extensions.Count() > 0)
                    {
                        anchorType = enumType;
                        break;
                    }
                }

                types.UnionWith(extensions);
            }

            return new AnchorVerifierSourceData(types, anchorType);
        }

        private static List<string> GetExtensions(AnchorType anchorType, X509Certificate2 certificate, List<string> extensions)
        {
            var anchorEnum = typeof(AnchorType);

            var name = Enum.GetName(anchorEnum, anchorType);
            string extensionOid = anchorEnum.GetRuntimeField(name)
                .GetCustomAttributes(inherit: false)
                .OfType<ExtensionOidAttribute>()
                .Single().ExtensionOid;

            return GetListOfStringsFromExtension(certificate, extensionOid);
        }

        private static List<string> GetListOfStringsFromExtension(X509Certificate2 certificate, string extensionOid)
        {
            var extensionStrings = new List<string>();

            X509Extension matchingExtension =
                certificate.Extensions.OfType<X509Extension>()
                .FirstOrDefault(ext => ext.Oid.Value == extensionOid);

            if (matchingExtension != null)
            {
                byte[] extensionBytes = matchingExtension.RawData;

                if (extensionBytes != null)
                {
                    Asn1InputStream stream = new Asn1InputStream(extensionBytes);

                    DerSequence obj = (DerSequence)stream.ReadObject();

                    foreach (var innerObj in obj)
                    {
                        Asn1TaggedObject seqObject = (Asn1TaggedObject)innerObj;
                        Asn1OctetString octetString = Asn1OctetString.GetInstance(seqObject, isExplicit: false);

                        extensionStrings.Add(System.Text.Encoding.UTF8.GetString(octetString.GetOctets()));
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
    }
}