/*
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Google.Protobuf.Collections;
namespace DigitalIdentity
{
    public class UserProfile
    {
        private readonly BaseProfile baseProfile;

        public UserProfile(BaseProfile profile)
        {
            baseProfile = profile;
        }

        public Attribute.ImageAttribute Selfie()
        {
            return baseProfile.GetImageAttribute(Consts.AttrSelfie);
        }

        public Attribute.ImageAttribute GetSelfieAttributeByID(string attributeID)
        {
            foreach (var a in baseProfile.AttributeSlice)
            {
                if (a.EphemeralId == attributeID)
                {
                    return Attribute.ImageAttribute.New(a);
                }
            }
            return null;
        }

        public Attribute.StringAttribute GivenNames()
        {
            return baseProfile.GetStringAttribute(Consts.AttrGivenNames);
        }

        public Attribute.StringAttribute FamilyName()
        {
            return baseProfile.GetStringAttribute(Consts.AttrFamilyName);
        }

        public Attribute.StringAttribute FullName()
        {
            var givenNames = GivenNames()?.Value ?? "";
            var familyName = FamilyName()?.Value ?? "";
            return new Attribute.StringAttribute(givenNames + " " + familyName);
        }

        public Attribute.StringAttribute MobileNumber()
        {
            return baseProfile.GetStringAttribute(Consts.AttrMobileNumber);
        }

        public Attribute.StringAttribute EmailAddress()
        {
            return baseProfile.GetStringAttribute(Consts.AttrEmailAddress);
        }

        public (Attribute.DateAttribute, bool) DateOfBirth()
        {
            foreach (var a in baseProfile.AttributeSlice)
            {
                if (a.Name == Consts.AttrDateOfBirth)
                {
                    return Attribute.DateAttribute.New(a);
                }
            }
            return (null, false);
        }

        public Attribute.StringAttribute Address()
        {
            var addressAttribute = baseProfile.GetStringAttribute(Consts.AttrAddress);
            return addressAttribute ?? EnsureAddressProfile();
        }

        private Attribute.StringAttribute EnsureAddressProfile()
        {
            // Implementation goes here
            return null;
        }

        public (Attribute.JSONAttribute, bool) StructuredPostalAddress()
        {
            return baseProfile.GetJSONAttribute(Consts.AttrStructuredPostalAddress);
        }

        public Attribute.StringAttribute Gender()
        {
            return baseProfile.GetStringAttribute(Consts.AttrGender);
        }

        public Attribute.StringAttribute Nationality()
        {
            return baseProfile.GetStringAttribute(Consts.AttrNationality);
        }

        public (Attribute.ImageSliceAttribute, bool) DocumentImages()
        {
            foreach (var a in baseProfile.AttributeSlice)
            {
                if (a.Name == Consts.AttrDocumentImages)
                {
                    return Attribute.ImageSliceAttribute.New(a);
                }
            }
            return (null, false);
        }

        public Attribute.ImageSliceAttribute GetDocumentImagesAttributeByID(string attributeID)
        {
            foreach (var a in baseProfile.AttributeSlice)
            {
                if (a.EphemeralId == attributeID)
                {
                    return Attribute.ImageSliceAttribute.New(a);
                }
            }
            return null;
        }

        public (Attribute.DocumentDetailsAttribute, bool) DocumentDetails()
        {
            foreach (var a in baseProfile.AttributeSlice)
            {
                if (a.Name == Consts.AttrDocumentDetails)
                {
                    return Attribute.DocumentDetailsAttribute.New(a);
                }
            }
            return (null, false);
        }

        public (Attribute.JSONAttribute, bool) IdentityProfileReport()
        {
            return baseProfile.GetJSONAttribute(Consts.AttrIdentityProfileReport);
        }

        public List<Attribute.AgeVerification> AgeVerifications()
        {
            var ageUnderString = Consts.AttrAgeUnder.Replace("%d", "");
            var ageOverString = Consts.AttrAgeOver.Replace("%d", "");
            var verifications = new List<Attribute.AgeVerification>();

            foreach (var a in baseProfile.AttributeSlice)
            {
                if (a.Name.StartsWith(ageUnderString) || a.Name.StartsWith(ageOverString))
                {
                    var verification = Attribute.AgeVerification.New(a);
                    verifications.Add(verification);
                }
            }
            return verifications;
        }
    }

    public class BaseProfile
    {
        public List<YotiProtoAttr.Attribute> AttributeSlice { get; }

        public BaseProfile(List<YotiProtoAttr.Attribute> attributes)
        {
            AttributeSlice = attributes;
        }

        public Attribute.GenericAttribute GetAttribute(string attributeName)
        {
            return AttributeSlice.FirstOrDefault(a => a.Name == attributeName);
        }

        public Attribute.GenericAttribute GetAttributeByID(string attributeID)
        {
            return AttributeSlice.FirstOrDefault(a => a.EphemeralId == attributeID);
        }

        public List<Attribute.GenericAttribute> GetAttributes(string attributeName)
        {
            return AttributeSlice.Where(a => a.Name == attributeName)
                                 .Select(a => Attribute.GenericAttribute.New(a))
                                 .ToList();
        }

        public Attribute.StringAttribute GetStringAttribute(string attributeName)
        {
            return AttributeSlice.FirstOrDefault(a => a.Name == attributeName) is var attribute ? Attribute.StringAttribute.New(attribute) : null;
        }

        public Attribute.ImageAttribute GetImageAttribute(string attributeName)
        {
            var attribute = AttributeSlice.FirstOrDefault(a => a.Name == attributeName);
            return attribute != null ? Attribute.ImageAttribute.New(attribute) : null;
        }

        public (Attribute.JSONAttribute, bool) GetJSONAttribute(string attributeName)
        {
            var attribute = AttributeSlice.FirstOrDefault(a => a.Name == attributeName);
            return attribute != null ? Attribute.JSONAttribute.New(attribute) : (null, false);
        }
    }
}

*/
