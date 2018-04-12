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

        /// <summary>
        /// The unique identifier returned by Yoti.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Selfie is a photograph of the user. This will be null if not provided by Yoti
        /// </summary>
        public Image Selfie { get; set; }

        /// <summary>
        /// FullName represents the user's full name. This will be null if not provided by Yoti
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        // GivenNames represents the user's given names. This will be null if not provided by Yoti
        /// </summary>
        public string GivenNames { get; set; }

        /// <summary>
        /// Family represents the user's family name. This will be null if not provided by Yoti
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// MobileNumber represents the user's mobile phone number. This will be null if not provided by Yoti
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// EmailAddress represents the user's email address. This will be null if not provided by Yoti
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// DateOfBirth represents the user's date of birth. This will be null if not provided by Yoti
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// IsAgeVerified Did the user pass the age verification check? Returns True if they passed, False if they failed, and null if there was no check
        /// </summary>
        public bool? IsAgeVerified { get; set; }

        /// <summary>
        /// Address represents the user's address. This will be null if not provided by Yoti
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Structured Postal Address represents the user's address represented as a dictionary. This will be null if not provided by Yoti
        /// </summary>
        public Dictionary<string, string> StructuredPostalAddress { get; set; }

        /// <summary>
        /// Gender represents the user's gender. This will be null if not provided by Yoti
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Nationality represents the user's nationality. This will be null if not provided by Yoti
        /// </summary>
        public string Nationality { get; set; }

        /// <summary>
        /// Other profile data returned by Yoti.
        /// </summary>
        public Dictionary<string, YotiAttributeValue> OtherAttributes { get; set; }
    }
}