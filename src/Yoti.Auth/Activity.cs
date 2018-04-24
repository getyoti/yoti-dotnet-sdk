using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AttrpubapiV1;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.DataObjects;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth
{
    internal class Activity
    {
        private static YotiUserProfile _yotiUserProfile;

        public ActivityDetails HandleSuccessfulResponse(AsymmetricCipherKeyPair keyPair, Response response)
        {
            ProfileDO parsedResponse = JsonConvert.DeserializeObject<ProfileDO>(response.Content);

            if (parsedResponse.receipt == null)
            {
                return new ActivityDetails
                {
                    Outcome = ActivityOutcome.Failure
                };
            }
            else if (parsedResponse.receipt.sharing_outcome != "SUCCESS")
            {
                return new ActivityDetails
                {
                    Outcome = ActivityOutcome.SharingFailure
                };
            }
            else
            {
                ReceiptDO receipt = parsedResponse.receipt;

                AttributeList attributes = CryptoEngine.DecryptCurrentUserReceipt(
                    parsedResponse.receipt.wrapped_receipt_key,
                    parsedResponse.receipt.other_party_profile_content,
                    keyPair);

                _yotiUserProfile = new YotiUserProfile
                {
                    Id = parsedResponse.receipt.remember_me_id
                };

                AddAttributesToProfile(attributes);

                return new ActivityDetails
                {
                    Outcome = ActivityOutcome.Success,
                    UserProfile = _yotiUserProfile
                };
            }
        }

        private static void AddAttributesToProfile(AttributeList attributes)
        {
            foreach (AttrpubapiV1.Attribute attribute in attributes.Attributes)
            {
                YotiAttribute<object> yotiAttribute = AttributeConverter.ConvertAttribute(attribute);

                PropertyInfo propertyInfo = GetProfilePropertyByProtobufName(yotiAttribute.GetName());
                string stringValue = yotiAttribute.GetStringValue();
                var byteValue = Conversion.UtfToBytes(stringValue);
                switch (attribute.ContentType)
                {
                    case ContentType.String:
                        if (attribute.Name == YotiConstants.AttributeStructuredPostalAddress
                             && propertyInfo.IsDefined(typeof(IsJsonAttribute)))
                        {
                            var structuredPostalAddressAttributeValue = new YotiAttributeValue(TypeEnum.Json, byteValue);
                            var structuredPostalAddressAttribute = new YotiAttribute<Dictionary<string, object>>(YotiConstants.AttributeStructuredPostalAddress, structuredPostalAddressAttributeValue);

                            _yotiUserProfile.StructuredPostalAddressAttribute = structuredPostalAddressAttribute;
                            break;
                        }

                        if (yotiAttribute.GetName().StartsWith(YotiConstants.AttributeAgeOver)
                            || yotiAttribute.GetName().StartsWith(YotiConstants.AttributeAgeUnder))
                        {
                            bool parsed = Boolean.TryParse(stringValue, out bool IsAgeVerified);

                            if (!parsed)
                                throw new FormatException(
                                    String.Format(
                                        "'{0}' byte value was unable to be parsed into a bool",
                                        byteValue));

                            _yotiUserProfile.IsAgeVerified = IsAgeVerified;
                            break;
                        }

                        SetStringAttribute(propertyInfo, stringValue);
                        break;

                    case ContentType.Jpeg:
                        var jpegYotiAttributeValue = new YotiAttributeValue(TypeEnum.Jpeg, byteValue);
                        _yotiUserProfile.SelfieAttribute = new YotiAttribute<Image>(propertyInfo.Name, jpegYotiAttributeValue);
                        break;

                    case ContentType.Png:
                        var pngYotiAttributeValue = new YotiAttributeValue(TypeEnum.Png, byteValue);
                        _yotiUserProfile.SelfieAttribute = new YotiAttribute<Image>(propertyInfo.Name, pngYotiAttributeValue);
                        break;

                    case ContentType.Date:

                        DateTime date;
                        if (DateTime.TryParseExact(stringValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                        {
                            SetDateAttribute(propertyInfo, byteValue);
                        }
                        break;

                    default:
                        HandleOtherAttributes(_yotiUserProfile, attribute, byteValue);
                        break;
                }
            }
        }

        private static void SetStringAttribute(PropertyInfo propertyInfo, string value)
        {
            var yotiAttributeValue = new YotiAttributeValue(TypeEnum.Text, value);
            var yotiAttribute = new YotiAttribute<string>(propertyInfo.Name, yotiAttributeValue);

            propertyInfo.SetValue(_yotiUserProfile, yotiAttribute);
        }

        private static void SetDateAttribute(PropertyInfo propertyInfo, byte[] value)
        {
            var yotiAttributeValue = new YotiAttributeValue(TypeEnum.Date, value);
            var yotiAttribute = new YotiAttribute<DateTime?>(propertyInfo.Name, yotiAttributeValue);

            propertyInfo.SetValue(_yotiUserProfile, yotiAttribute);
        }

        private static PropertyInfo GetProfilePropertyByProtobufName(string protobufName)
        {
            IEnumerable<PropertyInfo> propertiesWithProtobufNameAttribute =
                typeof(YotiUserProfile).GetTypeInfo().DeclaredProperties.Where(
                    p => p.GetCustomAttribute(typeof(ProtobufNameAttribute)) != null);

            IEnumerable<PropertyInfo> matchingProperties = propertiesWithProtobufNameAttribute.Where(
                prop => prop.GetCustomAttribute<ProtobufNameAttribute>().ProtobufName == protobufName);

            if (matchingProperties == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "No matching attribute found for Protobuf name '{0}'",
                        protobufName));
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

        private static void HandleOtherAttributes(YotiUserProfile profile, AttrpubapiV1.Attribute attribute, byte[] data)
        {
            switch (attribute.ContentType)
            {
                case ContentType.Date:
                    profile.OtherAttributes.Add(
                        attribute.Name,
                        new YotiAttributeValue(TypeEnum.Date, data));
                    break;

                case ContentType.String:
                    profile.OtherAttributes.Add(
                        attribute.Name,
                        new YotiAttributeValue(TypeEnum.Text, data));
                    break;

                case ContentType.Jpeg:
                    profile.OtherAttributes.Add(
                        attribute.Name,
                        new YotiAttributeValue(TypeEnum.Jpeg, data));
                    break;

                case ContentType.Png:
                    profile.OtherAttributes.Add(
                        attribute.Name,
                        new YotiAttributeValue(TypeEnum.Png, data));
                    break;

                case ContentType.Undefined:
                    // do not return attributes with undefined content types
                    break;

                default:
                    break;
            }
        }
    }
}