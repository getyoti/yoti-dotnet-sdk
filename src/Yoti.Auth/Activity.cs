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
        private readonly YotiProfile _yotiProfile;

        public Activity(YotiProfile yotiProfile)
        {
            _yotiProfile = yotiProfile;
        }

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
            SetAddressToBeFormattedAddressIfNull();

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

        internal void SetAddressToBeFormattedAddressIfNull()
        {
            YotiAttribute<IEnumerable<Dictionary<string, JToken>>> structuredPostalAddress = _yotiProfile.StructuredPostalAddress;

            if (_yotiProfile.Address == null && structuredPostalAddress != null)
            {
                Dictionary<string, JToken> jsonValue = structuredPostalAddress.GetJsonValue();
                jsonValue.TryGetValue("formatted_address", out JToken formattedAddressJToken);

                if (formattedAddressJToken != null)
                {
                    var addressValue = new YotiAttributeValue(YotiAttributeValue.TypeEnum.Text, formattedAddressJToken.ToString());

                    var addressAttribute = new YotiAttribute<string>(
                        name: Constants.UserProfile.PostalAddressAttribute,
                        value: addressValue,
                        anchors: structuredPostalAddress.GetAnchors());

                    _yotiProfile.Add(addressAttribute);
                }
            }
        }
    }
}