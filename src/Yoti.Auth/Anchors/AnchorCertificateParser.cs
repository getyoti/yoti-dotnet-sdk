using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Google.Protobuf;
using Org.BouncyCastle.Asn1;

namespace Yoti.Auth.Anchors
{
    public static class AnchorCertificateParser
    {
        public static AnchorVerifierSourceData GetTypesFromAnchor(ProtoBuf.Attribute.Anchor anchor)
        {
            Validation.NotNull(anchor, nameof(anchor));

            var types = new HashSet<string>();
            AnchorType anchorType = AnchorType.UNKNOWN;

            foreach (ByteString byteString in anchor.OriginServerCerts)
            {
                var extensions = new List<string>();
                X509Certificate2 certificate = new X509Certificate2(byteString.ToByteArray());
                var anchorEnum = typeof(AnchorType);

                foreach (X509Extension x509Extension in certificate.Extensions.OfType<X509Extension>())
                {
                    var extensionOid = x509Extension.Oid.Value;

                    if (extensionOid == AnchorType.SOURCE.ExtensionOid())
                    {
                        anchorType = AnchorType.SOURCE;
                    }
                    else if (extensionOid == AnchorType.VERIFIER.ExtensionOid())
                    {
                        anchorType = AnchorType.VERIFIER;
                    }
                    else
                    {
                        continue;
                    }

                    extensions = GetListOfStringsFromExtension(certificate, extensionOid);
                }

                if (extensions.Count == 0)
                {
                    return new AnchorVerifierSourceData(new HashSet<string> { "" }, AnchorType.UNKNOWN);
                }

                types.UnionWith(extensions);
            }

            return new AnchorVerifierSourceData(types, anchorType);
        }

        private static List<string> GetListOfStringsFromExtension(X509Certificate2 certificate, string extensionOid)
        {
            var extensionStrings = new List<string>();

            X509Extension matchingExtension =
                certificate.Extensions.OfType<X509Extension>()
                .FirstOrDefault(ext => ext.Oid.Value == extensionOid);

            byte[] extensionBytes = matchingExtension?.RawData;

            if (extensionBytes != null)
            {
                using (Asn1InputStream stream = new Asn1InputStream(extensionBytes))
                {
                    DerSequence obj = (DerSequence)stream.ReadObject();

                    foreach (object innerObj in obj)
                    {
                        Asn1TaggedObject seqObject = (Asn1TaggedObject)innerObj;
                        Asn1OctetString octetString = Asn1OctetString.GetInstance(obj: seqObject, isExplicit: false);

                        extensionStrings.Add(System.Text.Encoding.UTF8.GetString(octetString.GetOctets()));
                    }
                }
            }

            return extensionStrings;
        }

        private static string ExtensionOid(this Enum value)
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);

            return enumType.GetField(name)
                .GetCustomAttributes(false)
                .OfType<ExtensionOidAttribute>()
                .SingleOrDefault()
                .ExtensionOid;
        }
    }
}