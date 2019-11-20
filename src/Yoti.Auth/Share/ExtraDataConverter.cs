using System.Collections.Generic;

namespace Yoti.Auth.Share
{
    internal static class ExtraDataConverter
    {
        public static ExtraData ParseExtraDataProto(byte[] extraDataBytes)
        {
            List<object> parsedDataEntries = ParseDataEntries(extraDataBytes);

            return new ExtraData(parsedDataEntries);
        }

        private static List<object> ParseDataEntries(byte[] extraDataBytes)
        {
            List<object> parsedDataEntries = new List<object>();

            var extraDataProto = ProtoBuf.Share.ExtraData.Parser.ParseFrom(extraDataBytes);

            foreach (var dataEntry in extraDataProto.List)
            {
                var parsedDataEntry = DataEntryConverter.ConvertDataEntry(dataEntry);
                parsedDataEntries.Add(parsedDataEntry);
            }

            return parsedDataEntries;
        }
    }
}