using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Yoti.Auth.Verifications
{
    internal class AgeVerificationParser
    {
        private Dictionary<string, AgeVerification> _ageUnderVerificationsDict;
        private Dictionary<string, AgeVerification> _ageOverVerificationsDict;
        private ReadOnlyCollection<AgeVerification> _allVerificationsDict;

        public AgeVerificationParser(BaseProfile baseProfile)
        {
            _ageUnderVerificationsDict = FindVerifications(Constants.UserProfile.AgeUnderAttribute, baseProfile);
            _ageOverVerificationsDict = FindVerifications(Constants.UserProfile.AgeOverAttribute, baseProfile);

            _allVerificationsDict = _ageUnderVerificationsDict.Values
                .Concat(_ageOverVerificationsDict.Values).ToList().AsReadOnly();
        }

        public ReadOnlyCollection<AgeVerification> FindAllAgeVerifications()
        {
            return _allVerificationsDict;
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

        private static Dictionary<string, AgeVerification> FindVerifications(string ageVerificationPrefix, BaseProfile baseProfile)
        {
            var ageVerificationsDict = new Dictionary<string, AgeVerification>();

            foreach (YotiAttribute<string> attribute in baseProfile.FindAttributesStartingWith<string>(ageVerificationPrefix))
            {
                ageVerificationsDict.Add(attribute.GetName(), new AgeVerification(attribute));
            }

            return ageVerificationsDict;
        }
    }
}