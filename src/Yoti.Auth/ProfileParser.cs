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

namespace Yoti.Auth
{
    internal static class ProfileParser
    {
        public static ActivityDetails HandleResponse(AsymmetricCipherKeyPair keyPair, Response response)
        {
            if (!response.Success)
            {
                Response.CreateExceptionFromStatusCode<YotiProfileException>(response);
            }

            if (string.IsNullOrEmpty(response.Content))
            {
                throw new YotiProfileException("The content of the response is null");
            }

            ProfileDO parsedResponse = JsonConvert.DeserializeObject<ProfileDO>(response.Content);

            if (parsedResponse.Receipt == null)
            {
                throw new YotiProfileException("The receipt of the parsed response is null");
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

            return new ActivityDetails(parsedResponse.Receipt.RememberMeId, parsedResponse.Receipt.ParentRememberMeId, timestamp, userProfile, applicationProfile, parsedResponse.Receipt.ReceiptId);
        }

        private static Dictionary<string, BaseAttribute> ParseProfileContent(AsymmetricCipherKeyPair keyPair, string wrappedReceiptKey, string profileContent)
        {
            var parsedAttributes = new Dictionary<string, BaseAttribute>();

            if (!string.IsNullOrEmpty(profileContent))
            {
                ProtoBuf.Attribute.AttributeList profileAttributeList = CryptoEngine.DecryptCurrentUserReceipt(
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