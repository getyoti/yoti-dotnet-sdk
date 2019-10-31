using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
using Yoti.Auth.Attribute;
using Yoti.Auth.Document;
using Yoti.Auth.Images;
using Yoti.Auth.Verifications;

namespace Yoti.Auth.Profile
{
    public class YotiProfile : BaseProfile
    {
        private readonly AgeVerificationParser _ageVerificationParser;

        internal YotiProfile() : base()
        {
            _ageVerificationParser = new AgeVerificationParser(baseProfile: this);
        }

        internal YotiProfile(IBaseProfile baseProfile)
        {
            _ageVerificationParser = new AgeVerificationParser(baseProfile);
        }

        internal YotiProfile(Dictionary<string, List<BaseAttribute>> attributes) : base(attributes)
        {
            _ageVerificationParser = new AgeVerificationParser(baseProfile: this);
        }

        /// <summary>
        /// Selfie is a photograph of the user. This will be null if not provided by Yoti.
        /// </summary>
        public YotiAttribute<Image> Selfie
        {
            get
            {
                return GetAttributeByName<Image>(name: Constants.UserProfile.SelfieAttribute);
            }
        }

        /// <summary>
        /// FullName represents the user's full name. If family_name/given_names are present, the
        /// value will be equal to the string 'given_names + " " + family_name'. Will be null if not
        /// provided by Yoti.
        /// </summary>
        public YotiAttribute<string> FullName
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.UserProfile.FullNameAttribute);
            }
        }

        /// <summary>
        /// GivenNames corresponds to secondary names in passport, and first/middle names in English.
        /// Will be null if not provided by Yoti.
        /// </summary>
        public YotiAttribute<string> GivenNames
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.UserProfile.GivenNamesAttribute);
            }
        }

        /// <summary>
        /// FamilyName corresponds to primary name in passport, and surname in English. Will be null
        /// if not provided by Yoti.
        /// </summary>
        public YotiAttribute<string> FamilyName
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.UserProfile.FamilyNameAttribute);
            }
        }

        /// <summary>
        /// MobileNumber represents the user's mobile phone number, as verified at registration time.
        /// The value will be a number in E.164 format (i.e. '+' for international prefix and no
        /// spaces, e.g. "+447777123456"). Will be null if not provided by Yoti.
        /// </summary>
        public YotiAttribute<string> MobileNumber
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.UserProfile.PhoneNumberAttribute);
            }
        }

        /// <summary>
        /// EmailAddress represents the user's verified email address. Will be null if not provided
        /// by Yoti.
        /// </summary>
        public YotiAttribute<string> EmailAddress
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.UserProfile.EmailAddressAttribute);
            }
        }

        /// <summary>
        /// DateOfBirth represents the user's date of birth. Will be null if not provided by Yoti.
        /// The time part of this DateTime will default to 00:00:00. Will be null if not provided by Yoti.
        /// </summary>
        public YotiAttribute<DateTime> DateOfBirth
        {
            get
            {
                return GetAttributeByName<DateTime>(name: Constants.UserProfile.DateOfBirthAttribute);
            }
        }

        /// <summary>
        /// Address represents the user's address. This will be null if not provided by Yoti.
        /// </summary>
        public YotiAttribute<string> Address
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.UserProfile.PostalAddressAttribute);
            }
        }

        /// <summary>
        /// StructuredPostalAddress represents the user's address represented as a dictionary. This
        /// will be null if not provided by Yoti.
        /// </summary>
        public YotiAttribute<Dictionary<string, JToken>> StructuredPostalAddress
        {
            get
            {
                return GetAttributeByName<Dictionary<string, JToken>>(name: Constants.UserProfile.StructuredPostalAddressAttribute);
            }
        }

        /// <summary>
        /// Gender corresponds to the gender in the registered document; the value will be one of the
        /// strings "MALE", "FEMALE", "TRANSGENDER" or "OTHER". Will be null if not provided by Yoti.
        /// </summary>
        public YotiAttribute<string> Gender
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.UserProfile.GenderAttribute);
            }
        }

        /// <summary>
        /// Nationality corresponds to the nationality in the passport. The value is an ISO-3166-1
        /// alpha-3 code with ICAO9303 (passport) extensions. Will be null if not provided by Yoti.
        /// </summary>
        public YotiAttribute<string> Nationality
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.UserProfile.NationalityAttribute);
            }
        }

        /// <summary>
        /// Document Details. This will be null if not provided by Yoti.
        /// </summary>
        public YotiAttribute<DocumentDetails> DocumentDetails
        {
            get
            {
                return GetAttributeByName<DocumentDetails>(name: Constants.UserProfile.DocumentDetailsAttribute);
            }
        }

        /// <summary>
        /// Document Images. This will be null if not provided by Yoti.
        /// </summary>
        public YotiAttribute<List<Image>> DocumentImages
        {
            get
            {
                return GetAttributeByName<List<Image>>(name: Constants.UserProfile.DocumentImagesAttribute);
            }
        }

        /// <summary>
        /// Finds all the 'Age Over' and 'Age Under' derived attributes returned with the profile,
        /// and returns them wrapped in <see cref="AgeVerification"/> objects. Returns null if no
        /// matches were found.
        /// </summary>
        public ReadOnlyCollection<AgeVerification> AgeVerifications
        {
            get
            {
                return _ageVerificationParser.FindAllAgeVerifications();
            }
        }

        /// <summary>
        /// Searches for an <see cref="AgeVerification"/> corresponding to an `Age Under` check for
        /// the given age. Returns null if no match was found.
        /// </summary>
        public AgeVerification FindAgeUnderVerification(int age)
        {
            return _ageVerificationParser.FindAgeUnderVerification(age);
        }

        /// <summary>
        /// Searches for an <see cref="AgeVerification"/> corresponding to an `Age Over` check for
        /// the given age. Returns null if no match was found.
        /// </summary>
        public AgeVerification FindAgeOverVerification(int age)
        {
            return _ageVerificationParser.FindAgeOverVerification(age);
        }
    }
}