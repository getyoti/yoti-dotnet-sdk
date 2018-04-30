using System;
using System.Collections.Generic;

namespace Yoti.Auth
{
    public class YotiProfile
    {
        /// <summary>
        /// The unique identifier returned by Yoti.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Selfie is a photograph of the user. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("selfie")]
        public YotiAttribute<Image> Selfie { get; set; }

        /// <summary>
        /// FullName represents the user's full name. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("full_name")]
        public YotiAttribute<string> FullName { get; set; }

        /// <summary>
        // GivenNames represents the user's given names. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("given_names")]
        public YotiAttribute<string> GivenNames { get; set; }

        /// <summary>
        /// FamilyName represents the user's family name. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("family_name")]
        public YotiAttribute<string> FamilyName { get; set; }

        /// <summary>
        /// MobileNumber represents the user's mobile phone number. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("phone_number")]
        public YotiAttribute<string> MobileNumber { get; set; }

        /// <summary>
        /// EmailAddress represents the user's email address. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("email_address")]
        public YotiAttribute<string> EmailAddress { get; set; }

        /// <summary>
        /// DateOfBirth represents the user's date of birth. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("date_of_birth")]
        public YotiAttribute<DateTime?> DateOfBirth { get; set; }

        /// <summary>
        /// IsAgeVerified Did the user pass the age verification check? Returns True if they passed, False if they failed, and null if there was no check
        /// </summary>
        [ProtobufName("age_over:")]
        public YotiAttribute<bool?> IsAgeVerified { get; set; }

        /// <summary>
        /// Address represents the user's address. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("postal_address")]
        public YotiAttribute<string> Address { get; set; }

        /// <summary>
        /// StructuredPostalAddress represents the user's address represented as a dictionary. This will be null if not provided by Yoti
        /// </summary>
        [IsJson]
        [ProtobufName("structured_postal_address")]
        public YotiAttribute<Dictionary<string, object>> StructuredPostalAddress { get; set; }

        /// <summary>
        /// Gender represents the user's gender. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("gender")]
        public YotiAttribute<string> Gender { get; set; }

        /// <summary>
        /// Nationality represents the user's nationality. This will be null if not provided by Yoti
        /// </summary>
        [ProtobufName("nationality")]
        public YotiAttribute<string> Nationality { get; set; }

        /// <summary>
        /// Other profile data returned by Yoti.
        /// </summary>
        public Dictionary<string, YotiAttributeValue> OtherAttributes { get; set; }
    }
}