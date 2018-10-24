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
            else
            {
                ReceiptDO receipt = parsedResponse.Receipt;

                ParseOtherPartyProfileContent(keyPair, parsedResponse);
                var applicationProfileAttributes = ParseApplicationProfileContent(keyPair, parsedResponse);

                ApplicationProfile applicationProfile = new ApplicationProfile(applicationProfileAttributes);

                return new ActivityDetails(parsedResponse.Receipt.remember_me_id, _yotiUserProfile, _yotiProfile, applicationProfile, parsedResponse.Receipt.receipt_id, ActivityOutcome.Success);
            }
        }

        private void ParseOtherPartyProfileContent(AsymmetricCipherKeyPair keyPair, ProfileDO parsedResponse)
        {
            AttrpubapiV1.AttributeList profileAttributes = CryptoEngine.DecryptCurrentUserReceipt(
                parsedResponse.Receipt.wrapped_receipt_key,
                parsedResponse.Receipt.other_party_profile_content,
                keyPair);

            _yotiUserProfile.Id = parsedResponse.Receipt.remember_me_id;
            _yotiProfile.Id = parsedResponse.Receipt.remember_me_id;

            AddAttributesToProfile(profileAttributes);
        }

        private Dictionary<string, YotiAttribute<object>> ParseApplicationProfileContent(AsymmetricCipherKeyPair keyPair, ProfileDO parsedResponse)
        {
            var parsedAttributes = new Dictionary<string, YotiAttribute<object>>();

            if (!string.IsNullOrEmpty(parsedResponse.Receipt.profile_content))
            {
                AttrpubapiV1.AttributeList applicationProfileAttributeList = CryptoEngine.DecryptCurrentUserReceipt(
                    parsedResponse.Receipt.wrapped_receipt_key,
                    parsedResponse.Receipt.profile_content,
                    keyPair);

                foreach (AttrpubapiV1.Attribute attribute in applicationProfileAttributeList.Attributes)
                {
                    parsedAttributes.Add(attribute.Name, AttributeConverter.ConvertAttribute(attribute));
                }
            }

            return parsedAttributes;
        }

        internal void AddAttributesToProfile(AttrpubapiV1.AttributeList attributes)
        {
            foreach (AttrpubapiV1.Attribute attribute in attributes.Attributes)
            {
                YotiAttribute<object> yotiAttribute = AttributeConverter.ConvertAttribute(attribute);
                byte[] byteValue = attribute.Value.ToByteArray();

                if (yotiAttribute == null)
                {
                    HandleOtherAttributes(_yotiProfile, attribute, byteValue);
                    return;
                }

                string stringValue = Conversion.BytesToUtf8(byteValue);

                LegacyAddAttribute(attribute, byteValue);

                PropertyInfo propertyInfo = GetProfilePropertyByProtobufName(yotiAttribute.GetName());

                if (propertyInfo == null)
                {
                    HandleOtherAttributes(_yotiProfile, attribute, byteValue);
                    return;
                }

                List<Yoti.Auth.Anchors.Anchor> anchors = yotiAttribute.GetAnchors();

                switch (attribute.ContentType)
                {
                    case AttrpubapiV1.ContentType.Json:
                        if (attribute.Name == YotiConstants.StructuredPostalAddressAttribute)
                        {
                            var structuredPostalAddressAttributeValue = new YotiAttributeValue(TypeEnum.Json, byteValue);
                            var structuredPostalAddressAttribute = new YotiAttribute<IEnumerable<Dictionary<string, JToken>>>(
                                YotiConstants.StructuredPostalAddressAttribute,
                                structuredPostalAddressAttributeValue,
                                anchors);

                            _yotiProfile.StructuredPostalAddress = structuredPostalAddressAttribute;
                        }
                        else
                        {
                            HandleOtherAttributes(_yotiProfile, attribute, byteValue);
                        }
                        break;

                    case AttrpubapiV1.ContentType.String:
                        if (yotiAttribute.GetName().StartsWith(YotiConstants.AgeOverAttribute)
                            || yotiAttribute.GetName().StartsWith(YotiConstants.AgeUnderAttribute))
                        {
                            bool parsed = Boolean.TryParse(stringValue, out bool AgeVerified);

                            if (!parsed)
                                throw new FormatException(
                                    String.Format(
                                        "'{0}' byte value was unable to be parsed into a bool",
                                        byteValue));

                            var AgeVerifiedAttributeValue = new YotiAttributeValue(TypeEnum.Bool, byteValue);
                            _yotiProfile.AgeVerified = new YotiAttribute<bool?>(
                                propertyInfo.Name,
                                AgeVerifiedAttributeValue,
                                anchors);

                            break;
                        }

                        SetStringAttribute(propertyInfo, stringValue, anchors);
                        break;

                    case AttrpubapiV1.ContentType.Jpeg:
                        var jpegYotiAttributeValue = new YotiAttributeValue(TypeEnum.Jpeg, byteValue);
                        _yotiProfile.Selfie = new YotiImageAttribute<Image>(
                            propertyInfo.Name,
                            jpegYotiAttributeValue,
                            anchors);
                        break;

                    case AttrpubapiV1.ContentType.Png:
                        var pngYotiAttributeValue = new YotiAttributeValue(TypeEnum.Png, byteValue);
                        _yotiProfile.Selfie = new YotiImageAttribute<Image>(
                            propertyInfo.Name,
                            pngYotiAttributeValue,
                            anchors);
                        break;

                    case AttrpubapiV1.ContentType.Date:

                        DateTime date;
                        if (DateTime.TryParseExact(stringValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            SetDateAttribute(propertyInfo, byteValue, anchors);
                        }
                        break;

                    default:
                        HandleOtherAttributes(_yotiProfile, attribute, byteValue);
                        break;
                }
            }

            SetAddressToBeFormattedAddressIfNull();
        }

        private void SetAddressToBeFormattedAddressIfNull()
        {
            YotiAttribute<IEnumerable<Dictionary<string, JToken>>> structuredPostalAddress = _yotiProfile.StructuredPostalAddress;

            if (_yotiProfile.Address == null && structuredPostalAddress != null)
            {
                PropertyInfo addressPropertyInfo = GetProfilePropertyByProtobufName("postal_address");

                Dictionary<string, JToken> jsonValue = structuredPostalAddress.GetJsonValue();
                jsonValue.TryGetValue("formatted_address", out JToken formattedAddressJToken);

                if (formattedAddressJToken != null)
                {
                    string formattedAddress = formattedAddressJToken.ToString();

                    SetStringAttribute(
                        addressPropertyInfo,
                        formattedAddress,
                        structuredPostalAddress.GetAnchors());

                    _yotiUserProfile.Address = formattedAddress;
                }
            }
        }

        private void LegacyAddAttribute(AttrpubapiV1.Attribute attribute, byte[] byteValue)
        {
            switch (attribute.Name)
            {
                case "selfie":

                    switch (attribute.ContentType)
                    {
                        case AttrpubapiV1.ContentType.Jpeg:
                            _yotiUserProfile.Selfie = new Image
                            {
                                Type = TypeEnum.Jpeg,
                                Data = byteValue
                            };
                            break;

                        case AttrpubapiV1.ContentType.Png:
                            _yotiUserProfile.Selfie = new Image
                            {
                                Type = TypeEnum.Png,
                                Data = byteValue
                            };
                            break;
                    }
                    break;

                case "given_names":
                    _yotiUserProfile.GivenNames = Conversion.BytesToUtf8(byteValue);
                    break;

                case "family_name":
                    _yotiUserProfile.FamilyName = Conversion.BytesToUtf8(byteValue);
                    break;

                case "full_name":
                    _yotiUserProfile.FullName = Conversion.BytesToUtf8(byteValue);
                    break;

                case "phone_number":
                    _yotiUserProfile.MobileNumber = Conversion.BytesToUtf8(byteValue);
                    break;

                case "email_address":
                    _yotiUserProfile.EmailAddress = Conversion.BytesToUtf8(byteValue);
                    break;

                case "date_of_birth":
                    {
                        if (DateTime.TryParseExact(Conversion.BytesToUtf8(byteValue), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                        {
                            _yotiUserProfile.DateOfBirth = date;
                        }
                    }
                    break;

                case "postal_address":
                    _yotiUserProfile.Address = Conversion.BytesToUtf8(byteValue);
                    break;

                case "structured_postal_address":
                    string utf8json = Conversion.BytesToUtf8(byteValue);
                    Dictionary<string, JToken> deserializedPostalAddress = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, JToken>>(utf8json);
                    _yotiUserProfile.StructuredPostalAddress = deserializedPostalAddress;
                    break;

                case "gender":
                    _yotiUserProfile.Gender = Conversion.BytesToUtf8(byteValue);
                    break;

                case "nationality":
                    _yotiUserProfile.Nationality = Conversion.BytesToUtf8(byteValue);
                    break;

                default:
                    if (attribute.Name.StartsWith(YotiConstants.AgeOverAttribute)
                        || attribute.Name.StartsWith(YotiConstants.AgeUnderAttribute))
                    {
                        bool parsed = Boolean.TryParse(Conversion.BytesToUtf8(byteValue), out bool IsAgeVerified);

                        if (!parsed)
                            throw new FormatException(
                                String.Format(
                                    "'{0}' value was unable to be parsed into a bool",
                                    byteValue));

                        _yotiUserProfile.IsAgeVerified = IsAgeVerified;
                        break;
                    }
                    else
                    {
                        HandleOtherAttributes(_yotiUserProfile, attribute, byteValue);
                        break;
                    }
            }
        }

        private void SetStringAttribute(PropertyInfo propertyInfo, string value, List<Anchor> anchors)
        {
            var yotiAttributeValue = new YotiAttributeValue(TypeEnum.Text, value);
            var yotiAttribute = new YotiAttribute<string>(
                propertyInfo.Name,
                yotiAttributeValue,
                anchors);

            propertyInfo.SetValue(_yotiProfile, yotiAttribute);
        }

        private void SetDateAttribute(PropertyInfo propertyInfo, byte[] value, List<Anchor> anchors)
        {
            var yotiAttributeValue = new YotiAttributeValue(TypeEnum.Date, value);
            var yotiAttribute = new YotiAttribute<DateTime?>(
                propertyInfo.Name,
                yotiAttributeValue,
                anchors);

            propertyInfo.SetValue(_yotiProfile, yotiAttribute);
        }

        private static PropertyInfo GetProfilePropertyByProtobufName(string protobufName)
        {
            IEnumerable<PropertyInfo> propertiesWithProtobufNameAttribute =
                typeof(YotiProfile).GetTypeInfo().DeclaredProperties.Where(
                    p => p.GetCustomAttribute(typeof(ProtobufNameAttribute)) != null);

            IEnumerable<PropertyInfo> matchingProperties = propertiesWithProtobufNameAttribute.Where(
                prop => prop.GetCustomAttribute<ProtobufNameAttribute>().ProtobufName == protobufName);

            if (!matchingProperties.Any())
            {
                IEnumerable<PropertyInfo> ageVerifiedAttributes = typeof(YotiProfile).GetTypeInfo().DeclaredProperties.Where(
                prop => prop.Name == YotiConstants.AgeVerified &&
                (protobufName.StartsWith(YotiConstants.AgeOverAttribute) ||
                protobufName.StartsWith(YotiConstants.AgeUnderAttribute)));

                if (ageVerifiedAttributes.Count() == 1)
                    return ageVerifiedAttributes.Single();

                return null;
            }

            if (matchingProperties.Count() > 1)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "More than one properties were found with the ProtobufName value of '{0}'",
                        protobufName));
            }

            return matchingProperties.Single();
        }

        private static void HandleOtherAttributes(YotiProfile profile, AttrpubapiV1.Attribute attribute, byte[] data)
        {
            profile.OtherAttributes = new Dictionary<string, YotiAttributeValue>();

            switch (attribute.ContentType)
            {
                case AttrpubapiV1.ContentType.Date:
                    profile.OtherAttributes.Add(
                        attribute.Name,
                        new YotiAttributeValue(TypeEnum.Date, data));
                    break;

                case AttrpubapiV1.ContentType.String:
                    profile.OtherAttributes.Add(
                        attribute.Name,
                        new YotiAttributeValue(TypeEnum.Text, data));
                    break;

                case AttrpubapiV1.ContentType.Jpeg:
                    profile.OtherAttributes.Add(
                        attribute.Name,
                        new YotiAttributeValue(TypeEnum.Jpeg, data));
                    break;

                case AttrpubapiV1.ContentType.Png:
                    profile.OtherAttributes.Add(
                        attribute.Name,
                        new YotiAttributeValue(TypeEnum.Png, data));
                    break;

                case AttrpubapiV1.ContentType.Json:
                    profile.OtherAttributes.Add(
                        attribute.Name,
                        new YotiAttributeValue(TypeEnum.Json, data));
                    break;

                // do not return attributes with undefined content types
                default:
                    break;
            }
        }

        private static void HandleOtherAttributes(YotiUserProfile profile, AttrpubapiV1.Attribute attribute, byte[] data)
        {
            switch (attribute.ContentType)
            {
                case AttrpubapiV1.ContentType.Date:
                    profile.OtherAttributes.Add(
                        attribute.Name,
                        new YotiAttributeValue(TypeEnum.Date, data));
                    break;

                case AttrpubapiV1.ContentType.String:
                    profile.OtherAttributes.Add(
                        attribute.Name,
                        new YotiAttributeValue(TypeEnum.Text, data));
                    break;

                case AttrpubapiV1.ContentType.Jpeg:
                    profile.OtherAttributes.Add(
                        attribute.Name,
                        new YotiAttributeValue(TypeEnum.Jpeg, data));
                    break;

                case AttrpubapiV1.ContentType.Png:
                    profile.OtherAttributes.Add(
                        attribute.Name,
                        new YotiAttributeValue(TypeEnum.Png, data));
                    break;

                case AttrpubapiV1.ContentType.Undefined:
                    // do not return attributes with undefined content types
                    break;
            }
        }
    }
}