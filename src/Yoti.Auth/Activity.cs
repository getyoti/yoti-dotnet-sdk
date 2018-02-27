using System;
using System.Globalization;
using AttrpubapiV1;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.DataObjects;

namespace Yoti.Auth
{
    internal class Activity
    {
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

                var profile = new YotiUserProfile
                {
                    Id = parsedResponse.receipt.remember_me_id
                };

                AddAttributesToProfile(attributes, profile);

                return new ActivityDetails
                {
                    Outcome = ActivityOutcome.Success,
                    UserProfile = profile
                };
            }
        }

        private static void AddAttributesToProfile(AttributeList attributes, YotiUserProfile profile)
        {
            foreach (AttrpubapiV1.Attribute attribute in attributes.Attributes)
            {
                byte[] data = attribute.Value.ToByteArray();
                switch (attribute.Name)
                {
                    case "selfie":

                        switch (attribute.ContentType)
                        {
                            case ContentType.Jpeg:
                                profile.Selfie = new Image
                                {
                                    Type = ImageType.Jpeg,
                                    Data = data,
                                    Base64URI = "data:image/jpeg;base64," + Convert.ToBase64String(data)
                                };
                                break;

                            case ContentType.Png:
                                profile.Selfie = new Image
                                {
                                    Type = ImageType.Png,
                                    Data = data,
                                    Base64URI = "data:image/png;base64," + Convert.ToBase64String(data)
                                };
                                break;
                        }
                        break;

                    case "given_names":
                        profile.GivenNames = Conversion.BytesToUtf8(data);
                        break;

                    case "family_name":
                        profile.FamilyName = Conversion.BytesToUtf8(data);
                        break;

                    case "full_name":
                        profile.FullName = Conversion.BytesToUtf8(data);
                        break;

                    case "phone_number":
                        profile.MobileNumber = Conversion.BytesToUtf8(data);
                        break;

                    case "email_address":
                        profile.EmailAddress = Conversion.BytesToUtf8(data);
                        break;

                    case "date_of_birth":
                        {
                            DateTime date;
                            if (DateTime.TryParseExact(Conversion.BytesToUtf8(data), "yyyy-MM-dd", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                            {
                                profile.DateOfBirth = date;
                            }
                        }
                        break;

                    case "postal_address":
                        profile.Address = Conversion.BytesToUtf8(data);
                        break;

                    case "gender":
                        profile.Gender = Conversion.BytesToUtf8(data);
                        break;

                    case "nationality":
                        profile.Nationality = Conversion.BytesToUtf8(data);
                        break;

                    default:
                        HandleOtherAttributes(profile, attribute, data);
                        break;
                }
            }
        }

        private static void HandleOtherAttributes(YotiUserProfile profile, AttrpubapiV1.Attribute attribute, byte[] data)
        {
            switch (attribute.ContentType)
            {
                case ContentType.Date:
                    profile.OtherAttributes.Add(
                        attribute.Name,
                        new YotiAttributeValue(YotiAttributeValue.TypeEnum.Date, data));
                    break;

                case ContentType.String:
                    profile.OtherAttributes.Add(
                        attribute.Name,
                        new YotiAttributeValue(YotiAttributeValue.TypeEnum.Text, data));
                    break;

                case ContentType.Jpeg:
                    profile.OtherAttributes.Add(
                        attribute.Name,
                        new YotiAttributeValue(YotiAttributeValue.TypeEnum.Jpeg, data));
                    break;

                case ContentType.Png:
                    profile.OtherAttributes.Add(
                        attribute.Name,
                        new YotiAttributeValue(YotiAttributeValue.TypeEnum.Png, data));
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