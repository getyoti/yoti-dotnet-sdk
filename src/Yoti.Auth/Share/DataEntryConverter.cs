using Google.Protobuf;
using Yoti.Auth.Exceptions;
using Yoti.Auth.Properties;
using Yoti.Auth.Share.ThirdParty;

namespace Yoti.Auth.Share
{
    internal static class DataEntryConverter
    {
        public static object ConvertDataEntry(ProtoBuf.Share.DataEntry dataEntry)
        {
            object entry;

            try
            {
                entry = ConvertValue(dataEntry.Type, dataEntry.Value);
            }
            catch (InvalidProtocolBufferException ex)
            {
                throw new ExtraDataException(Resources.DataEntryParsingFail, ex);
            }

            return entry;
        }

        private static object ConvertValue(ProtoBuf.Share.DataEntry.Types.Type dataEntryType, ByteString value)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            switch (dataEntryType)
            {
                case ProtoBuf.Share.DataEntry.Types.Type.ThirdPartyAttribute:
                    return ThirdPartyAttributeConverter.ParseThirdPartyAttribute(value.ToByteArray());

                default:
                    logger.Warn($"Unsupported data entry '{dataEntryType.ToString()}', skipping...");
                    return null;
            }
        }
    }
}