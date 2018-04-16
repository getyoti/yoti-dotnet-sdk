using System;
using System.Collections.Generic;

namespace Yoti.Auth
{
    public class YotiUserProfile
    {
        /// <summary>
        /// Creates a <see cref="YotiUserProfile" />
        /// </summary>
        public YotiUserProfile()
        {
            OtherAttributes = new Dictionary<string, YotiAttributeValue>(StringComparer.OrdinalIgnoreCase);
        }

        public List<YotiAttribute<object>> AttributeList;

        [Obsolete("Id is deprecated, please use IdAttribute instead")]
        public string Id { get; set; }

        /// <summary>
        /// The unique identifier returned by Yoti.
        /// </summary>
        public YotiAttribute<string> IdAttribute { get; set; }

        [Obsolete("Selfie is deprecated, please use SelfieAttribute instead")]
        public Image Selfie { get; set; }

        /// <summary>
        /// SelfieAttribute is a photograph of the user. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("selfie")]
        public YotiAttribute<string> SelfieAttribute { get; set; }

        [Obsolete("FullName is deprecated, please use FullNameAttribute instead")]
        public string FullName { get; set; }

        /// <summary>
        /// FullNameAttribute represents the user's full name. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("full_name")]
        public YotiAttribute<string> FullNameAttribute { get; set; }

        [Obsolete("GivenNames is deprecated, please use GivenNamesAttribute instead")]
        public string GivenNames { get; set; }

        /// <summary>
        // GivenNamesAttribute represents the user's given names. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("given_names")]
        public YotiAttribute<string> GivenNamesAttribute { get; set; }

        [Obsolete("FamilyName is deprecated, please use FamilyNameAttribute instead")]
        public string FamilyName { get; set; }

        /// <summary>
        /// FamilyNameAttribute represents the user's family name. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("family_name")]
        public YotiAttribute<string> FamilyNameAttribute { get; set; }

        [Obsolete("MobileNumber is deprecated, please use MobileNumberAttribute instead")]
        public string MobileNumber { get; set; }

        /// <summary>
        /// MobileNumberAttribute represents the user's mobile phone number. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("phone_number")]
        public YotiAttribute<string> MobileNumberAttribute { get; set; }

        [Obsolete("EmailAddress is deprecated, please use EmailAddressAttribute instead")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// EmailAddressAttribute represents the user's email address. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("email_address")]
        public YotiAttribute<string> EmailAddressAttribute { get; set; }

        [Obsolete("DateOfBirth is deprecated, please use DateOfBirthAttribute instead")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// DateOfBirthAttribute represents the user's date of birth. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("date_of_birth")]
        public YotiAttribute<DateTime?> DateOfBirthAttribute { get; set; }

        [Obsolete("IsAgeVerified is deprecated, please use IsAgeVerifiedAttribute instead")]
        public bool? IsAgeVerified { get; set; }

        /// <summary>
        /// IsAgeVerified Did the user pass the age verification check? Returns True if they passed, False if they failed, and null if there was no check
        /// </summary>
        public YotiAttribute<bool?> IsAgeVerifiedAttribute { get; set; }

        [Obsolete("Address is deprecated, please use AddressAttribute instead")]
        public string Address { get; set; }

        /// <summary>
        /// AddressAttribute represents the user's address. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("postal_address")]
        public YotiAttribute<string> AddressAttribute { get; set; }

        [Obsolete("StructuredPostalAddress is deprecated, please use StructuredPostalAddressAttribute instead")]
        public Dictionary<string, string> StructuredPostalAddress { get; set; }

        /// <summary>
        /// StructuredPostalAddressAttribute represents the user's address represented as a dictionary. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("structured_postal_address")]
        public YotiAttribute<Dictionary<string, string>> StructuredPostalAddressAttribute { get; set; }

        [Obsolete("Gender is deprecated, please use GenderAttribute instead")]
        public string Gender { get; set; }

        /// <summary>
        /// GenderAttribute represents the user's gender. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("gender")]
        public YotiAttribute<string> GenderAttribute { get; set; }

        [Obsolete("Nationality is deprecated, please use NationalityAttribute instead")]
        public string Nationality { get; set; }

        /// <summary>
        /// NationalityAttribute represents the user's nationality. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("nationality")]
        public YotiAttribute<string> NationalityAttribute { get; set; }

        /// <summary>
        /// Other profile data returned by Yoti.
        /// </summary>
        public Dictionary<string, YotiAttributeValue> OtherAttributes { get; set; }
    }
}