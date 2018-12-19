using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.DataObjects;

namespace Yoti.Auth
{
    internal class Activity
    {
        public ActivityDetails HandleSuccessfulResponse(AsymmetricCipherKeyPair keyPair, Response response)
        {
            ProfileDO parsedResponse = JsonConvert.DeserializeObject<ProfileDO>(response.Content);

            if (parsedResponse.Receipt == null)
            {
                return new ActivityDetails(ActivityOutcome.Failure);
            }
            else if (parsedResponse.Receipt.sharing_outcome != "SUCCESS")
            {
                return new ActivityDetails(ActivityOutcome.SharingFailure);
            }

            ReceiptDO receipt = parsedResponse.Receipt;

            var userProfile = new YotiProfile(
                ParseProfileContent(keyPair, receipt.wrapped_receipt_key, receipt.other_party_profile_content));
            SetAddressToBeFormattedAddressIfNull(userProfile);

            var applicationProfile = new ApplicationProfile(
                ParseProfileContent(keyPair, receipt.wrapped_receipt_key, receipt.profile_content));

            DateTime? timestamp = null;
            if (receipt.timestamp != null)
            {
                if (DateTime.TryParseExact(receipt.timestamp, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    timestamp = parsedDate;
                }
            }

            return new ActivityDetails(parsedResponse.Receipt.remember_me_id, timestamp, userProfile, applicationProfile, parsedResponse.Receipt.receipt_id, ActivityOutcome.Success);
        }

        private Dictionary<string, BaseAttribute> ParseProfileContent(AsymmetricCipherKeyPair keyPair, string wrappedReceiptKey, string profileContent)
        {
            var parsedAttributes = new Dictionary<string, BaseAttribute>();

            if (!string.IsNullOrEmpty(profileContent))
            {
                AttrpubapiV1.AttributeList profileAttributeList = CryptoEngine.DecryptCurrentUserReceipt(
                    wrappedReceiptKey,
                    profileContent,
                    keyPair);

                foreach (AttrpubapiV1.Attribute attribute in profileAttributeList.Attributes)
                {
                    parsedAttributes.Add(attribute.Name, AttributeConverter.ConvertToBaseAttribute(attribute));
                }
            }

            return parsedAttributes;
        }

        internal void SetAddressToBeFormattedAddressIfNull(YotiProfile yotiProfile)
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