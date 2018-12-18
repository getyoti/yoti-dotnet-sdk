using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Verifications;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class AgeVerificationTests
    {
        private static readonly YotiAttribute<string> _ageUnder18Attribute =
            new YotiAttribute<string>("age_under:18", "false", anchors: null);

        private static readonly YotiAttribute<string> _ageUnder21Attribute =
            new YotiAttribute<string>("age_under:21", "true", anchors: null);

        private static readonly YotiAttribute<string> _ageOver18Attribute =
            new YotiAttribute<string>("age_over:18", "true", anchors: null);

        private static readonly YotiAttribute<string> _ageOver21Attribute =
            new YotiAttribute<string>("age_over:21", "false", anchors: null);

        private static readonly List<YotiAttribute<string>> _ageUnderAttributes =
            new List<YotiAttribute<string>> { _ageUnder18Attribute, _ageUnder21Attribute };

        private static readonly List<YotiAttribute<string>> _ageOverAttributes =
            new List<YotiAttribute<string>> { _ageOver18Attribute, _ageOver21Attribute };

        private readonly Collection<YotiAttribute<string>> _allAgeVerificationAttributes =
            new Collection<YotiAttribute<string>>(_ageUnderAttributes.Concat(_ageOverAttributes).ToList());

        private readonly List<YotiAttribute<string>> _emptyAttributeStringList = new List<YotiAttribute<string>>();
        private YotiProfile _yotiProfile;

        [TestMethod]
        public void AgeVerifications_ShouldBeNullSafe()
        {
            using (ShimsContext.Create())
            {
                ConfigureNoChecksPresent();

                ReadOnlyCollection<AgeVerification> result = _yotiProfile.AgeVerifications;

                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod]
        public void AgeVerifications_IncludesAllResults()
        {
            using (ShimsContext.Create())
            {
                ConfigureAgeOverAndAgeUnderChecks();

                var shimUserProfile = new Fakes.ShimBaseProfile(_yotiProfile);

                ReadOnlyCollection<AgeVerification> result = _yotiProfile.AgeVerifications;

                Assert.AreEqual(4, result.Count);
                CollectionAssert.Contains(_allAgeVerificationAttributes, result[0].Attribute());
                CollectionAssert.Contains(_allAgeVerificationAttributes, result[1].Attribute());
                CollectionAssert.Contains(_allAgeVerificationAttributes, result[2].Attribute());
                CollectionAssert.Contains(_allAgeVerificationAttributes, result[3].Attribute());
            }
        }

        [TestMethod]
        public void FindAgeOverVerification_returnsNullForNoMatch()
        {
            using (ShimsContext.Create())
            {
                ConfigureNoChecksPresent();

                AgeVerification result = _yotiProfile.FindAgeOverVerification(18);
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void FindAgeOverVerification_ReturnsCorrectResult()
        {
            int age = 18;

            using (ShimsContext.Create())
            {
                ConfigureAgeOverAndAgeUnderChecks();

                AgeVerification ageOverVerification = _yotiProfile.FindAgeOverVerification(age);

                Assert.AreEqual(true, ageOverVerification.Result());
                Assert.AreEqual(age, ageOverVerification.Age());
                Assert.AreEqual(Constants.UserProfile.AgeOverAttribute, ageOverVerification.CheckType());
                Assert.AreEqual(_ageOver18Attribute, ageOverVerification.Attribute());
            }
        }

        [TestMethod]
        public void FindAgeUnderVerification_returnsNullForNoMatch()
        {
            using (ShimsContext.Create())
            {
                ConfigureNoChecksPresent();

                AgeVerification result = _yotiProfile.FindAgeUnderVerification(18);
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void FindAgeUnderVerification_ReturnsCorrectResult()
        {
            int age = 21;

            using (ShimsContext.Create())
            {
                ConfigureAgeOverAndAgeUnderChecks();

                AgeVerification ageUnderVerification = _yotiProfile.FindAgeUnderVerification(age);

                Assert.AreEqual(true, ageUnderVerification.Result());
                Assert.AreEqual(age, ageUnderVerification.Age());
                Assert.AreEqual(Constants.UserProfile.AgeUnderAttribute, ageUnderVerification.CheckType());
                Assert.AreEqual(_ageUnder21Attribute, ageUnderVerification.Attribute());
            }
        }

        private void ConfigureNoChecksPresent()
        {
            _yotiProfile = new YotiProfile();

            var shimUserProfile = new Fakes.ShimBaseProfile(_yotiProfile);

            shimUserProfile.FindAttributesStartingWithOf1String((prefix) =>
            {
                return _emptyAttributeStringList;
            });
        }

        private void ConfigureAgeOverAndAgeUnderChecks()
        {
            var attributes = new Dictionary<string, BaseAttribute>()
            {
                { _ageUnder18Attribute.GetName(), _ageUnder18Attribute },
                { _ageUnder21Attribute.GetName(), _ageUnder21Attribute },
                { _ageOver18Attribute.GetName(), _ageOver18Attribute },
                { _ageOver21Attribute.GetName(), _ageOver21Attribute }
            };

            _yotiProfile = new YotiProfile(attributes);
        }
    }
}