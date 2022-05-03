using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Attribute;
using Yoti.Auth.DataObjects;
using Yoti.Auth.Exceptions;
using Yoti.Auth.Profile;
using Yoti.Auth.Share;

namespace Yoti.Auth
{
    internal static class ActivityDetailsParser
    {
        public static ActivityDetails HandleResponse(AsymmetricCipherKeyPair keyPair, string responseContent)
        {
            if (string.IsNullOrEmpty(responseContent))
            {
                throw new YotiProfileException(Properties.Resources.NullOrEmptyResponseContent);
            }

            ProfileDO parsedResponse = JsonConvert.DeserializeObject<ProfileDO>(responseContent);

            if (parsedResponse.Receipt == null)
            {
                throw new YotiProfileException(Properties.Resources.NullParsedResponse);
            }
            else if (parsedResponse.Receipt.SharingOutcome != "SUCCESS")
            {
                throw new YotiProfileException(
                    $"The share was not successful, sharing_outcome: '{parsedResponse.Receipt.SharingOutcome}'");
            }

            ReceiptDO receipt = parsedResponse.Receipt;

            var userProfile = new YotiProfile(
                ParseProfileContent(keyPair, receipt.WrappedReceiptKey, receipt.OtherPartyProfileContent));
            SetAddressToBeFormattedAddressIfNull(userProfile);

            var applicationProfile = new ApplicationProfile(
                ParseProfileContent(keyPair, receipt.WrappedReceiptKey, receipt.ProfileContent));

            ExtraData extraData = new ExtraData();

            if (!string.IsNullOrEmpty(parsedResponse.Receipt.ExtraDataContent))
            {
                extraData = CryptoEngine.DecryptExtraData(
                    receipt.WrappedReceiptKey,
                    parsedResponse.Receipt.ExtraDataContent,
                    keyPair);
            }

            DateTime? timestamp = null;
            if (receipt.Timestamp != null
                && DateTime.TryParseExact(
                    receipt.Timestamp,
                    "yyyy-MM-ddTHH:mm:ssZ",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal,
                    out DateTime parsedDate))
            {
                timestamp = parsedDate;
            }

            return new ActivityDetails(parsedResponse.Receipt.RememberMeId, parsedResponse.Receipt.ParentRememberMeId, timestamp, userProfile, applicationProfile, parsedResponse.Receipt.ReceiptId, extraData);
        }

        private static Dictionary<string, List<BaseAttribute>> ParseProfileContent(AsymmetricCipherKeyPair keyPair, string wrappedReceiptKey, string profileContent)
        {
            var parsedAttributes = new Dictionary<string, List<BaseAttribute>>();

            if (!string.IsNullOrEmpty(profileContent))
            {
                ProtoBuf.Attribute.AttributeList profileAttributeList = CryptoEngine.DecryptAttributeList(
                    wrappedReceiptKey,
                    profileContent,
                    keyPair);

                parsedAttributes = AttributeConverter.ConvertToBaseAttributes(profileAttributeList);
            }

            return parsedAttributes;
        }

        internal static void SetAddressToBeFormattedAddressIfNull(YotiProfile yotiProfile)
        {
            YotiAttribute<Dictionary<string, JToken>> structuredPostalAddress = yotiProfile.StructuredPostalAddress;

            if (yotiProfile.Address == null && structuredPostalAddress != null)
            {
                structuredPostalAddress.GetValue().TryGetValue("formatted_address", out JToken formattedAddressJToken);

                if (formattedAddressJToken != null)
                {
                    var addressAttribute = new YotiAttribute<string>(
                        name: Constants.UserProfile.PostalAddressAttribute,
                        value: formattedAddressJToken.ToString(),
                        anchors: structuredPostalAddress.GetAnchors());

                    yotiProfile.Add(addressAttribute);
                }
            }
        }
    }
}