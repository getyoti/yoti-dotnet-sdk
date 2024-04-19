
/*using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Com.Yoti.Api.Client;
using Google.Protobuf;
using NLog.Fluent;
using Yoti.Auth.Attribute;
using Yoti.Auth.ProtoBuf.Common;

namespace Com.Yoti.Api.Client.Spi.Remote
{
    public class AttributeListReader
    {
        private readonly EncryptedDataReader encryptedDataReader;
        private readonly AttributeListConverter attributeListConverter;

        private AttributeListReader(EncryptedDataReader encryptedDataReader, AttributeListConverter attributeListConverter)
        {
            this.encryptedDataReader = encryptedDataReader;
            this.attributeListConverter = attributeListConverter;
        }

        public static AttributeListReader NewInstance()
        {
            return new AttributeListReader(EncryptedDataReader.NewInstance(), AttributeListConverter.NewInstance());
        }

        public List<Attribute<object>> Read(byte[] encryptedProfileBytes, Key secretKey)
        {
            List<Attribute<object>> attributeList = new List<Attribute<object>>();
            if (encryptedProfileBytes != null && encryptedProfileBytes.Length > 0)
            {
                byte[] profileData = encryptedDataReader.DecryptBytes(encryptedProfileBytes, secretKey);
                attributeList = attributeListConverter.ParseAttributeList(profileData);
            }
            return attributeList;
        }
    }

    public class EncryptedDataReader
    {
        private EncryptedDataReader() { }

        public static EncryptedDataReader NewInstance()
        {
            return new EncryptedDataReader();
        }

        public byte[] DecryptBytes(byte[] encryptedBytes, Key secretKey)
        {
            var encryptedData = ParseEncryptedContent(encryptedBytes);
            return Decrypt(encryptedData, secretKey);
        }

        private EncryptedData ParseEncryptedContent(byte[] encryptedBytes)
        {
            try
            {
                return EncryptedData.Parser.ParseFrom(encryptedBytes);
            }
            catch (InvalidProtocolBufferException e)
            {
                throw new ProfileException("Cannot decode profile", e);
            }
        }

        private byte[] Decrypt(EncryptedDataProto.EncryptedData encryptedData, Key secretKey)
        {
            ByteString initVector = encryptedData.Iv;
            if (initVector == null || initVector.Size == 0)
            {
                throw new ProfileException("Receipt key IV must not be null.");
            }
            try
            {
                using (AesManaged aesAlg = new AesManaged())
                {
                    aesAlg.Key = secretKey.GetEncoded();
                    aesAlg.IV = initVector.ToByteArray();
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.PKCS7;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(encryptedData.CipherText.ToByteArray()))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (MemoryStream decryptedData = new MemoryStream())
                            {
                                csDecrypt.CopyTo(decryptedData);
                                return decryptedData.ToArray();
                            }
                        }
                    }
                }
            }
            catch (CryptographicException ce)
            {
                throw new ProfileException("Error decrypting data", ce);
            }
        }
    }

    internal class ProfileException : Exception
    {
        public ProfileException(string cannotDecodeProfile, InvalidProtocolBufferException invalidProtocolBufferException)
        {
            throw new NotImplementedException();
        }
    }

    public class AttributeListConverter
    {
        private readonly AttributeConverter attributeConverter;
        private readonly AddressTransformer addressTransformer;

        private AttributeListConverter(AttributeConverter attributeConverter, AddressTransformer addressTransformer)
        {
            this.attributeConverter = attributeConverter;
            this.addressTransformer = addressTransformer;
        }

        public static AttributeListConverter NewInstance()
        {
            return new AttributeListConverter(AttributeConverter.NewInstance(), AddressTransformer.NewInstance());
        }

        public List<Attribute<object>> ParseAttributeList(byte[] attributeListBytes)
        {
            if (attributeListBytes == null || attributeListBytes.Length == 0)
            {
                return new List<Attribute<object>>();
            }

            AttributeListProto.AttributeList attributeList = ParseProto(attributeListBytes);
            List<Attribute<object>> attributes = ParseAttributes(attributeList);
            Log.Debug("{0} out of {1} attribute(s) parsed successfully ", attributes.Count, attributeList.AttributesCount);
            EnsurePostalAddress(attributes);
            return attributes;
        }

        private AttributeListProto.AttributeList ParseProto(byte[] attributeListBytes)
        {
            try
            {
                return AttributeListProto.AttributeList.Parser.ParseFrom(attributeListBytes);
            }
            catch (InvalidProtocolBufferException e)
            {
                throw new ProfileException("Cannot parse profile data", e);
            }
        }

        private List<Attribute<object>> ParseAttributes(AttributeListProto.AttributeList message)
        {
            List<Attribute<object>> parsedAttributes = new List<Attribute<object>>();
            foreach (AttrProto.Attribute attribute in message.AttributesList)
            {
                try
                {
                    parsedAttributes.Add(attributeConverter.ConvertAttribute(attribute));
                }
                catch (IOException | ParseException e)
                {
                    Log.Warn("Failed to parse attribute '{0}' due to '{1}'", attribute.Name, e.Message);
                }
            }
            return parsedAttributes;
        }

        private void EnsurePostalAddress(List<Attribute<object>> attributes)
        {
            Attribute<object> postalAddress = FindAttribute(HumanProfileAttributes.POSTAL_ADDRESS, attributes);
            if (postalAddress == null)
            {
                Attribute<object> structuredPostalAddress = FindAttribute(HumanProfileAttributes.STRUCTURED_POSTAL_ADDRESS, attributes);
                if (structuredPostalAddress != null)
                {
                    Attribute<string> transformedAddress = addressTransformer.Transform(structuredPostalAddress);
                    if (transformedAddress != null)
                    {
                        Log.Debug("Substituting '{0}' in place of missing '{1}'", HumanProfileAttributes.STRUCTURED_POSTAL_ADDRESS, HumanProfileAttributes.POSTAL_ADDRESS);
                        attributes.Add(transformedAddress);
                    }
                }
            }
        }

        private Attribute<object> FindAttribute(string name, List<Attribute<object>> attributes)
        {
            foreach (Attribute<object> attribute in attributes)
            {
                if (name.Equals(attribute.Name))
                {
                    return attribute;
                }
            }
            return null;
        }
    }
}

*/
