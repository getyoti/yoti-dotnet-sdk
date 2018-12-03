using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Yoti.Auth.Document;
using Yoti.Auth.Images;

namespace Yoti.Auth
{
    public class YotiProfile : BaseProfile
    {
        internal YotiProfile() : base()
        {
        }

        internal YotiProfile(Dictionary<string, BaseAttribute> attributes) : base(attributes)
        {
        }

        /// <summary>
        /// Selfie is a photograph of the user. This will be null if not provided by Yoti
        /// </summary>
        public YotiAttribute<Image> Selfie
        {
            get
            {
                return GetAttributeByName<Image>(name: Constants.UserProfile.SelfieAttribute);
            }
        }

        /// <summary>
        /// FullName represents the user's full name. This will be null if not provided by Yoti
        /// </summary>
        public YotiAttribute<string> FullName
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.UserProfile.FullNameAttribute);
            }
        }

        /// <summary>
        /// GivenNames represents the user's given names. This will be null if not provided by Yoti
        /// </summary>
        public YotiAttribute<string> GivenNames
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.UserProfile.GivenNamesAttribute);
            }
        }

        /// <summary>
        /// FamilyName represents the user's family name. This will be null if not provided by Yoti
        /// </summary>
        public YotiAttribute<string> FamilyName
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.UserProfile.FamilyNameAttribute);
            }
        }

        /// <summary>
        /// MobileNumber represents the user's mobile phone number. This will be null if not provided by Yoti
        /// </summary>
        public YotiAttribute<string> MobileNumber
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.UserProfile.PhoneNumberAttribute);
            }
        }

        /// <summary>
        /// EmailAddress represents the user's email address. This will be null if not provided by Yoti
        /// </summary>
        public YotiAttribute<string> EmailAddress
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.UserProfile.EmailAddressAttribute);
            }
        }

        /// <summary>
        /// DateOfBirth represents the user's date of birth. This will be null if not provided by Yoti
        /// </summary>
        public YotiAttribute<DateTime> DateOfBirth
        {
            get
            {
                return GetAttributeByName<DateTime>(name: Constants.UserProfile.DateOfBirthAttribute);
            }
        }

        /// <summary>
        /// Address represents the user's address. This will be null if not provided by Yoti
        /// </summary>
        public YotiAttribute<string> Address
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.UserProfile.PostalAddressAttribute);
            }
        }

        /// <summary>
        /// StructuredPostalAddress represents the user's address represented as a dictionary. This will be null if not provided by Yoti
        /// </summary>
        public YotiAttribute<Dictionary<string, JToken>> StructuredPostalAddress
        {
            get
            {
                return GetAttributeByName<Dictionary<string, JToken>>(name: Constants.UserProfile.StructuredPostalAddressAttribute);
            }
        }

        /// <summary>
        /// Gender represents the user's gender. This will be null if not provided by Yoti
        /// </summary>
        public YotiAttribute<string> Gender
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.UserProfile.GenderAttribute);
            }
        }

        /// <summary>
        /// Nationality represents the user's nationality. This will be null if not provided by Yoti
        /// </summary>
        public YotiAttribute<string> Nationality
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.UserProfile.NationalityAttribute);
            }
        }

        /// <summary>
        /// Nationality represents the user's nationality. This will be null if not provided by Yoti
        /// </summary>
        public YotiAttribute<DocumentDetails> DocumentDetails
        {
            get
            {
                return GetAttributeByName<DocumentDetails>(name: Constants.UserProfile.DocumentDetailsAttribute);
            }
        }
    }
}