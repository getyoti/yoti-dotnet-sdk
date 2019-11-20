using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Share;
using Yoti.Auth.Share.ThirdParty;

namespace Yoti.Auth.Tests.Share
{
    [TestClass]
    public class ExtraDataTests
    {
        [TestMethod]
        public void AttributeIssuanceDetailsShouldBeNullWhenNoDataEntries()
        {
            ExtraData extraData = new ExtraData(new List<object>());

            Assert.IsNull(extraData.AttributeIssuanceDetails);
        }

        [TestMethod]
        public void ShouldFilterDataEntriesForAttributeIssuanceDetails()
        {
            var dataEntries = new List<object>();
            string token = "tokenValue";

            AttributeIssuanceDetails attributeIssuanceDetails = new AttributeIssuanceDetails(token, null, new List<AttributeDefinition>());

            dataEntries.Add(DateTime.Now);
            dataEntries.Add(attributeIssuanceDetails);

            ExtraData extraData = new ExtraData(dataEntries);

            Assert.AreEqual(token, extraData.AttributeIssuanceDetails.Token);
        }
    }
}