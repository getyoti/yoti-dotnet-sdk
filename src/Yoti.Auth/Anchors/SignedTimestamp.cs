using System;

namespace Yoti.Auth.Anchors
{
    /// <summary>
    /// SignedTimestamp is a timestamp associated with a message that has a cryptographic signature
    /// proving that it was issued by the correct authority
    /// </summary>
    public class SignedTimestamp
    {
        private readonly Int32 _version;
        private readonly DateTime _timestamp;
        private readonly byte[] _messageDigest;
        private readonly byte[] _chainDigest;
        private readonly byte[] _chainDigestSkip1;
        private readonly byte[] _chainDigestSkip2;

        public SignedTimestamp(ProtoBuf.Common.SignedTimestamp protobufSignedTimestamp)
        {
            Validation.NotNull(protobufSignedTimestamp, nameof(protobufSignedTimestamp));
            _timestamp = ConvertMicroSecondsSinceEpochToDateTime(protobufSignedTimestamp);
            _version = protobufSignedTimestamp.Version;
            _messageDigest = protobufSignedTimestamp.MessageDigest.ToByteArray();
            _chainDigest = protobufSignedTimestamp.ChainDigest.ToByteArray();
            _chainDigestSkip1 = protobufSignedTimestamp.ChainDigestSkip1.ToByteArray();
            _chainDigestSkip2 = protobufSignedTimestamp.ChainDigestSkip2.ToByteArray();
        }

        private static DateTime ConvertMicroSecondsSinceEpochToDateTime(ProtoBuf.Common.SignedTimestamp protobufSignedTimestamp)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            long timeInMicroSecondsSinceEpoch = (long)protobufSignedTimestamp.Timestamp;
            long timeInTicksSinceEpoch = timeInMicroSecondsSinceEpoch * 10;
            return epoch.Add(new TimeSpan(timeInTicksSinceEpoch));
        }

        /// <summary>
        /// Version indicates how the digests within this object are calculated
        /// </summary>
        public Int32 GetVersion()
        {
            return _version;
        }

        /// <summary>
        /// The actual timestamp with microsecond-level accuracy
        /// </summary>
        public DateTime GetTimestamp()
        {
            return _timestamp;
        }

        /// <summary>
        /// MessageDigest is the digest of the message this timestamp is associated with. The first
        /// step in verifying the timestamp is ensuring the MessageDigest matches the original
        /// message data
        /// <para>For version 1 objects, the message digest algorithm is SHA-512/224</para>
        /// </summary>
        public byte[] GetMessageDigest()
        {
            return _messageDigest;
        }

        /// <summary>
        /// ChainDigest is the digest of the previous SignedTimestamp message in the chain. The
        /// second step in verifying the timestamp is walking back over the chain and checking each
        /// SignedTimestamp's ChainDigest field. The SignedTimestamp at the beginning of the chain
        /// has this field set to a specific, publish value
        /// <para>
        /// For version 1 objects, the chain digest algorithm is HMAC-SHA-512/224, with the secret
        /// being equal to the MessageDigest field
        /// </para>
        /// </summary>
        public byte[] GetChainDigest()
        {
            return _chainDigest;
        }

        /// <summary>
        /// ChainDigestSkip1 is only populated once every 500 nodes. It is the ChainDigest value of
        /// the timestamp 500 nodes previously
        /// </summary>
        public byte[] GetChainDigestSkip1()
        {
            return _chainDigestSkip1;
        }

        /// <summary>
        /// ChainDigestSkip2 is only populated once every 250000 nodes (or once every 500 nodes that
        /// have ChainDigestSkip1 populated). It is the ChainDigest value of the timestamp 250000
        /// nodes previously
        /// </summary>
        public byte[] GetChainDigestSkip2()
        {
            return _chainDigestSkip2;
        }
    }
}