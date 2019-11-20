using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Exceptions;
using Yoti.Auth.ProtoBuf.Share;
using Yoti.Auth.Share;

namespace Yoti.Auth.Tests.Share
{
    [TestClass]
    public class DataEntryConverterTests
    {
        [TestMethod]
        public void ShouldThrowExtraDataExceptionForInvalidProtobuf()
        {
            var dataEntry = new ProtoBuf.Share.DataEntry
            {
                Value = ByteString.CopyFromUtf8(",̆"),
                Type = DataEntry.Types.Type.ThirdPartyAttribute
            };

            Assert.ThrowsException<ExtraDataException>(() =>
            {
                DataEntryConverter.ConvertDataEntry(dataEntry);
            });
        }

        [DataTestMethod]
        [DataRow(DataEntry.Types.Type.AgeVerificationSecret)]
        [DataRow(DataEntry.Types.Type.Invoice)]
        [DataRow(DataEntry.Types.Type.Location)]
        [DataRow(DataEntry.Types.Type.PaymentTransaction)]
        [DataRow(DataEntry.Types.Type.Transaction)]
        [DataRow(DataEntry.Types.Type.Undefined)]
        public void ShouldIgnoreNonThirdPartyAttributeDataEntries(ProtoBuf.Share.DataEntry.Types.Type dataEntryType)
        {
            var dataEntry = new DataEntry
            {
                Type = dataEntryType,
                Value = ByteString.CopyFromUtf8("value")
            };

            Assert.IsNull(DataEntryConverter.ConvertDataEntry(dataEntry));
        }
    }
}