using System;

namespace Example.Models
{
    public class User
    {
        public User()
        {
        }

        public int Id { get; set; }
        public string YotiId { get; set; }
        public byte[] Photo { get; set; }
        public string Base64Photo { get; set; }
        public string FullName { get; set; }
        public string GivenNames { get; set; }
        public string FamilyName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? IsAgeVerified { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
    }
}