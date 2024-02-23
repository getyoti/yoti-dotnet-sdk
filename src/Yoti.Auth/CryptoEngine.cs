using System;
using System.IO;
using System.Security.Cryptography;
using Google.Protobuf;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Generators;



using Yoti.Auth.ProtoBuf.Attribute;
using Yoti.Auth.ProtoBuf.Common;
using Yoti.Auth.Share;

namespace Yoti.Auth
{
    public static class CryptoEngine
    {
        private const string DigestAlgorithm = "SHA-256withRSA";

        public static AsymmetricCipherKeyPair LoadRsaKey(StreamReader keyStream)
        {
            var pemReader = new PemReader(keyStream);
            return (AsymmetricCipherKeyPair)pemReader.ReadObject();
        }

        internal static byte[] DecipherAes(byte[] key, byte[] iv, byte[] cipherBytes)
        {
            var keyParam = new KeyParameter(key);
            var keyParamWithIv = new ParametersWithIV(keyParam, iv);

            // decrypt using aes with private key and PKCS5/PKCS7
            var engine = new AesEngine();
            var blockCipher = new CbcBlockCipher(engine);
            var paddedBlockCipher = new PaddedBufferedBlockCipher(blockCipher); //Default scheme is PKCS5/PKCS7
            var outputBuffer = new byte[paddedBlockCipher.GetOutputSize(cipherBytes.Length)];

            paddedBlockCipher.Init(false, keyParamWithIv);
            int numOutputBytes = paddedBlockCipher.ProcessBytes(cipherBytes, outputBuffer, 0);
            numOutputBytes += paddedBlockCipher.DoFinal(outputBuffer, numOutputBytes);

            var result = new byte[numOutputBytes];
            Array.Copy(outputBuffer, result, numOutputBytes);

            return result;
        }

        internal static byte[] DecryptRsa(byte[] cipherBytes, AsymmetricCipherKeyPair keypair)
        {
            // decrypt using rsa with private key and PKCS 1 v1.5 padding
            var engine = new RsaEngine();
            var blockCipher = new Pkcs1Encoding(engine);

            blockCipher.Init(false, keypair.Private);
            return blockCipher.ProcessBlock(cipherBytes, 0, cipherBytes.Length);
        }

        internal static byte[] SignDigest(byte[] digestBytes, AsymmetricCipherKeyPair keypair)
        {
            // create a signature from the digest using SHA256 hashing with RSA
            ISigner signer = SignerUtilities.GetSigner(DigestAlgorithm);
            signer.Init(true, keypair.Private);
            signer.BlockUpdate(digestBytes, 0, digestBytes.Length);
            return signer.GenerateSignature();
        }

        internal static byte[] GetDerEncodedPublicKey(AsymmetricCipherKeyPair keypair)
        {
            return Org.BouncyCastle.X509.SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keypair.Public).GetDerEncoded();
        }

        internal static string GenerateNonce()
        {
            var random = new SecureRandom();

            var bytes = new byte[16];
            random.NextBytes(bytes);

            return new Guid(bytes).ToString("D");
        }

        internal static string DecryptToken(string encryptedConnectToken, AsymmetricCipherKeyPair keyPair)
        {
            Validation.NotNullOrEmpty(encryptedConnectToken, "one time use token");

            // token was encoded as a URL-safe base64 so it can be transferred in a URL
            byte[] cipherBytes = Conversion.UrlSafeBase64ToBytes(encryptedConnectToken);

            byte[] decipheredBytes = DecryptRsa(cipherBytes, keyPair);

            return Conversion.BytesToUtf8(decipheredBytes);
        }

        internal static byte[] UnwrapKey(string wrappedKey, AsymmetricCipherKeyPair keyPair)
        {
            byte[] cipherBytes = Conversion.Base64ToBytes(wrappedKey);

            return DecryptRsa(cipherBytes, keyPair);
        }

        internal static AttributeList DecryptAttributeList(string wrappedReceiptKey, string profileContent, AsymmetricCipherKeyPair keyPair)
        {
            byte[] decipheredBytes = DecipherContent(wrappedReceiptKey, profileContent, keyPair);

            return AttributeList.Parser.ParseFrom(decipheredBytes);
        }

        internal static ExtraData DecryptExtraData(string wrappedReceiptKey, string extraDataContent, AsymmetricCipherKeyPair keyPair)
        {
            byte[] decipheredBytes = DecipherContent(wrappedReceiptKey, extraDataContent, keyPair);

            return ExtraDataConverter.ParseExtraDataProto(decipheredBytes);
        }

        private static byte[] DecipherContent(string wrappedReceiptKey, string content, AsymmetricCipherKeyPair keyPair)
        {
            byte[] unwrappedKey = UnwrapKey(wrappedReceiptKey, keyPair);

            byte[] contentBytes = Conversion.Base64ToBytes(content);
            EncryptedData encryptedData = EncryptedData.Parser.ParseFrom(contentBytes);

            byte[] iv = encryptedData.Iv.ToByteArray();
            byte[] cipherText = encryptedData.CipherText.ToByteArray();

            byte[] decipheredBytes = DecipherAes(unwrappedKey, iv, cipherText);
            return decipheredBytes;
        }

        internal static string GetAuthKey(AsymmetricCipherKeyPair keyPair)
        {
            byte[] publicKey = GetDerEncodedPublicKey(keyPair);

            return Conversion.BytesToBase64(publicKey);
        }
        
        public static byte[] DecryptAesGcm(byte[] cipherText, byte[] iv, byte[] secret)
        {
            try
            {
                GcmBlockCipher cipher = new GcmBlockCipher(new Org.BouncyCastle.Crypto.Engines.AesEngine());
                ParametersWithIV parameters = new ParametersWithIV(new KeyParameter(secret), iv);
            
                cipher.Init(false, parameters);

                byte[] plainText = new byte[cipher.GetOutputSize(cipherText.Length)];
                int length = cipher.ProcessBytes(cipherText, 0, cipherText.Length, plainText, 0);
                cipher.DoFinal(plainText, length);

                return plainText;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to decrypt receipt key: {ex.Message}", ex);
            }
        }
        
        public static byte[] UnwrapReceiptKey(byte[] wrappedReceiptKey, byte[] encryptedItemKey, byte[] itemKeyIv, AsymmetricCipherKeyPair key)
        {
            try
            {
                byte[] decryptedItemKey = DecryptRsa(encryptedItemKey, key);

                byte[] plainText = DecryptAesGcm(wrappedReceiptKey, itemKeyIv, decryptedItemKey);

                return plainText;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to unwrap receipt key: {ex.Message}", ex);
            }
        }
        
        public static byte[] DecryptReceiptContent(byte[] content, byte[] receiptContentKey)
        {
            try
            {
                if (content == null)
                {
                    throw new ArgumentNullException("content", "Failed to decrypt receipt content: content is null");
                }

                var decodedData = new EncryptedData();
                decodedData.MergeFrom(content);

                return DecipherAes(receiptContentKey, decodedData.Iv.ToByteArray(), decodedData.CipherText.ToByteArray());
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to decrypt receipt content: {ex.Message}", ex);
            }
        }
    }
}