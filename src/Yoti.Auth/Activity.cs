using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Anchors;
using Yoti.Auth.CustomAttributes;
using Yoti.Auth.DataObjects;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth
{
    internal class Activity
    {
        private readonly YotiUserProfile _yotiUserProfile; //Deprecated old profile class, will be removed
        private readonly YotiProfile _yotiProfile; //New profile class

        public Activity(YotiProfile yotiProfile, YotiUserProfile yotiUserProfile)
        {
            _yotiUserProfile = yotiUserProfile;
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

            Dictionary<string, BaseAttribute> userProfileAttributes = ParseProfileContent(keyPair, receipt.wrapped_receipt_key, receipt.other_party_profile_content);
            var userProfile = new YotiProfile(userProfileAttributes);
            SetAddressToBeFormattedAddressIfNull();

            Dictionary<string, BaseAttribute> applicationProfileAttributes = ParseProfileContent(keyPair, receipt.wrapped_receipt_key, receipt.profile_content);
            var applicationProfile = new ApplicationProfile(applicationProfileAttributes);

            DateTime? timestamp = null;
            if (receipt.timestamp != null)
            {
                if (DateTime.TryParseExact(receipt.timestamp, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    timestamp = parsedDate;
                }
            }

            return new ActivityDetails(parsedResponse.Receipt.remember_me_id, timestamp, _yotiUserProfile, _yotiProfile, applicationProfile, parsedResponse.Receipt.receipt_id, ActivityOutcome.Success);
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

        private void SetAddressToBeFormattedAddressIfNull()
        {
            YotiAttribute<IEnumerable<Dictionary<string, JToken>>> structuredPostalAddress = _yotiProfile.StructuredPostalAddress;

            if (_yotiProfile.Address == null && structuredPostalAddress != null)
            {
                Dictionary<string, JToken> jsonValue = structuredPostalAddress.GetJsonValue();
                jsonValue.TryGetValue("formatted_address", out JToken formattedAddressJToken);

                if (formattedAddressJToken != null)
                {
                    string formattedAddress = formattedAddressJToken.ToString();
                }
            }
        }
    }
}