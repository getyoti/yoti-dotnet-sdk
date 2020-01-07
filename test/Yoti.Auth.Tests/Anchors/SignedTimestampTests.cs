using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Anchors;
using Yoti.Auth.Constants;

namespace Yoti.Auth.Tests.Anchors
{
    [TestClass]
    public class SignedTimestampTests
    {
        [TestMethod]
        public void ShouldHandleNegativeTimestamp()
        {
            var signedUnixTimestamp = -1571630945999999;
            var unsignedUnixTimestamp = (ulong)signedUnixTimestamp;
            var protoBufSignedTimestamp = new ProtoBuf.Common.SignedTimestamp { Timestamp = unsignedUnixTimestamp };

            var signedTimestamp = new SignedTimestamp(protoBufSignedTimestamp);
            string result = signedTimestamp.GetTimestamp().ToString(Format.RFC3339PatternMilli, DateTimeFormatInfo.InvariantInfo);

            Assert.AreEqual("1920-03-13T19:50:54.000001Z", result);
        }
    }
}