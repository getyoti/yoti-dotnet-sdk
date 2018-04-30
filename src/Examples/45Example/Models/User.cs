using System;
using System.Collections.Generic;
using Yoti.Auth;

namespace Example.Models
{
    public class User
    {
        public User()
        {
        }

        public string YotiId { get; set; }
        public byte[] Photo { get; set; }
        public string Base64Photo { get; set; }
        public int Id { get; set; }
        public YotiAttribute<Image> Selfie { get; set; }
        public YotiAttribute<string> FullName { get; set; }
        public YotiAttribute<string> GivenNames { get; set; }
        public YotiAttribute<string> FamilyName { get; set; }
        public YotiAttribute<string> MobileNumber { get; set; }
        public YotiAttribute<string> EmailAddress { get; set; }

        public YotiAttribute<DateTime?> DateOfBirth { get; set; }

        public YotiAttribute<bool?> IsAgeVerified { get; set; }

        public YotiAttribute<string> Address { get; set; }

        public YotiAttribute<Dictionary<string, object>> StructuredPostalAddress { get; set; }
        public Dictionary<string, object> StandardStructuredPostalAddress { get; set; }

        public YotiAttribute<string> Gender { get; set; }

        public YotiAttribute<string> Nationality { get; set; }

        public Dictionary<string, YotiAttributeValue> OtherAttributes { get; set; }
    }
}