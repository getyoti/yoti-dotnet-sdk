using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Yoti.Auth;

namespace Example.Models
{
    public class User
    {
        public User()
        {
        }

        public string RememberMeID { get; set; }
        public byte[] Photo { get; set; }
        public string Base64Photo { get; set; }
        public YotiAttribute<Image> Selfie { get; set; }
        public YotiAttribute<string> FullName { get; set; }
        public YotiAttribute<string> GivenNames { get; set; }
        public YotiAttribute<string> FamilyName { get; set; }
        public YotiAttribute<string> MobileNumber { get; set; }
        public YotiAttribute<string> EmailAddress { get; set; }

        public YotiAttribute<DateTime> DateOfBirth { get; set; }

        public YotiAttribute<string> Address { get; set; }

        public YotiAttribute<IEnumerable<Dictionary<string, JToken>>> StructuredPostalAddress { get; set; }

        public YotiAttribute<string> Gender { get; set; }

        public YotiAttribute<string> Nationality { get; set; }
    }
}