using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Google.Protobuf;
using Org.BouncyCastle.Asn1;
using Yoti.Auth.CustomAttributes;

namespace Yoti.Auth.Anchors
{
    public static class AnchorCertificateParser
    {
        public static AnchorVerifierSourceData GetTypesFromAnchor(AttrpubapiV1.Anchor anchor)
        {
            var types = new HashSet<string>();
            AnchorType anchorType = AnchorType.Unknown;

            foreach (ByteString byteString in anchor.OriginServerCerts)
            {
                var extensions = new List<string>();
                X509Certificate2 certificate = new X509Certificate2(byteString.ToByteArray());
                var anchorEnum = typeof(AnchorType);

                foreach (AnchorType type in Enum.GetValues(anchorEnum))
                {
                    var name = Enum.GetName(anchorEnum, type);
                    string extensionOid = anchorEnum.GetRuntimeField(name)
                        .GetCustomAttributes(inherit: false)
                        .OfType<ExtensionOidAttribute>()
                        .Single().ExtensionOid;

                    extensions = GetListOfStringsFromExtension(certificate, extensionOid);

                    if (extensions.Any())
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

            X509Extension matchingExtension =
                certificate.Extensions.OfType<X509Extension>()
                .FirstOrDefault(ext => ext.Oid.Value == extensionOid);

            byte[] extensionBytes = matchingExtension?.RawData;

            if (extensionBytes != null)
            {
                Asn1InputStream stream = new Asn1InputStream(extensionBytes);

                DerSequence obj = (DerSequence)stream.ReadObject();

                foreach (object innerObj in obj)
                {
                    Asn1TaggedObject seqObject = (Asn1TaggedObject)innerObj;
                    Asn1OctetString octetString = Asn1OctetString.GetInstance(obj: seqObject, isExplicit: false);

                    extensionStrings.Add(System.Text.Encoding.UTF8.GetString(octetString.GetOctets()));
                }
            }

            return extensionStrings;
        }
    }
}