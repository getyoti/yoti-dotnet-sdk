using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Yoti.Auth.Verifications
{
    internal class AgeVerificationParser
    {
        private Dictionary<string, AgeVerification> _ageUnderVerificationsDict;
        private Dictionary<string, AgeVerification> _ageOverVerificationsDict;
        private List<AgeVerification> _allVerificationsDict;

        public AgeVerificationParser(BaseProfile baseProfile)
        {
            _ageUnderVerificationsDict = FindAgeUnderVerifications(baseProfile);
            _ageOverVerificationsDict = FindAgeOverVerifications(baseProfile);

            _allVerificationsDict = _ageUnderVerificationsDict.Values
                .Concat(_ageOverVerificationsDict.Values).ToList();
        }

        public ReadOnlyCollection<AgeVerification> FindAllAgeVerifications()
        {
            return _allVerificationsDict.AsReadOnly();
        }

        public AgeVerification FindAgeUnderVerification(int age)
        {
            return _ageUnderVerificationsDict?.FirstOrDefault(
                x => x.Key == string.Format("{0}:{1}", Constants.UserProfile.AgeUnderAttribute, age))
                .Value;
        }

        public AgeVerification FindAgeOverVerification(int age)
        {
            return _ageOverVerificationsDict?.FirstOrDefault(
                x => x.Key == string.Format("{0}:{1}", Constants.UserProfile.AgeOverAttribute, age))
                .Value;
        }

        private static Dictionary<string, AgeVerification> FindAgeUnderVerifications(BaseProfile baseProfile)
        {
            var ageUnderVerificationsDict = new Dictionary<string, AgeVerification>();

            foreach (YotiAttribute<string> ageUnderAttribute in baseProfile.FindAttributesStartingWith<string>(Constants.UserProfile.AgeUnderAttribute))
            {
                ageUnderVerificationsDict.Add(ageUnderAttribute.GetName(), new AgeVerification(ageUnderAttribute));
            }

            return ageUnderVerificationsDict;
        }

        private static Dictionary<string, AgeVerification> FindAgeOverVerifications(BaseProfile baseProfile)
        {
            var ageOverVerificationsDict = new Dictionary<string, AgeVerification>();

            foreach (YotiAttribute<string> ageOverAttribute in baseProfile.FindAttributesStartingWith<string>(Constants.UserProfile.AgeOverAttribute))
            {
                ageOverVerificationsDict.Add(ageOverAttribute.GetName(), new AgeVerification(ageOverAttribute));
            }

            return ageOverVerificationsDict;
        }
    }
}