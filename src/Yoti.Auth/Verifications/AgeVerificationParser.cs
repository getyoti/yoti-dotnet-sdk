using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Yoti.Auth.Verifications
{
    internal class AgeVerificationParser
    {
        private readonly BaseProfile _baseProfile;
        private Dictionary<string, AgeVerification> _ageUnderVerificationsDict;
        private Dictionary<string, AgeVerification> _ageOverVerificationsDict;
        private List<AgeVerification> _allVerificationsDict;

        public AgeVerificationParser(BaseProfile baseProfile)
        {
            _baseProfile = baseProfile;
        }

        public ReadOnlyCollection<AgeVerification> FindAllAgeVerifications()
        {
            if (_allVerificationsDict == null)
            {
                FindAgeUnderVerifications();
                FindAgeOverVerifications();

                _allVerificationsDict = _ageUnderVerificationsDict.Values
                    .Concat(_ageOverVerificationsDict.Values).ToList();
            }

            return _allVerificationsDict.AsReadOnly();
        }

        public AgeVerification AgeUnderVerification(int age)
        {
            FindAgeUnderVerifications();
            return _ageUnderVerificationsDict?.FirstOrDefault(x => x.Key == string.Format("{0}:{1}", Constants.UserProfile.AgeUnderAttribute, age)).Value;
        }

        public AgeVerification AgeOverVerification(int age)
        {
            FindAgeOverVerifications();
            return _ageOverVerificationsDict?.FirstOrDefault(x => x.Key == string.Format("{0}:{1}", Constants.UserProfile.AgeOverAttribute, age)).Value;
        }

        private void FindAgeUnderVerifications()
        {
            if (_ageUnderVerificationsDict == null)
            {
                var ageUnderVerificationsDict = new Dictionary<string, AgeVerification>();

                foreach (YotiAttribute<string> ageUnderAttribute in _baseProfile.FindAttributesStartingWith<string>(Constants.UserProfile.AgeUnderAttribute))
                {
                    ageUnderVerificationsDict.Add(ageUnderAttribute.GetName(), new AgeVerification(ageUnderAttribute));
                }

                _ageUnderVerificationsDict = ageUnderVerificationsDict;
            }
        }

        private void FindAgeOverVerifications()
        {
            if (_ageOverVerificationsDict == null)
            {
                var ageOverVerificationsDict = new Dictionary<string, AgeVerification>();

                foreach (YotiAttribute<string> ageOverAttribute in _baseProfile.FindAttributesStartingWith<string>(Constants.UserProfile.AgeOverAttribute))
                {
                    ageOverVerificationsDict.Add(ageOverAttribute.GetName(), new AgeVerification(ageOverAttribute));
                }

                _ageOverVerificationsDict = ageOverVerificationsDict;
            }
        }
    }
}