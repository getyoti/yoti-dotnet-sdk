using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Yoti.Auth.Attribute;
using Yoti.Auth.Profile;
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

        private readonly List<YotiAttribute<string>> _allAgeVerificationAttributes =
            new List<YotiAttribute<string>>(_ageUnderAttributes.Concat(_ageOverAttributes).ToList());

        private readonly List<YotiAttribute<string>> _emptyAttributeStringList = new List<YotiAttribute<string>>();

        [TestMethod]
        public void AgeVerifications_ShouldBeNullSafe()
        {
            ReadOnlyCollection<AgeVerification> result = new YotiProfile().AgeVerifications;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void AgeVerifications_IncludesAllResults()
        {
            var mockBaseProfile = new Mock<IBaseProfile>();
            mockBaseProfile.Setup(x => x.FindAttributesStartingWith<string>(Constants.UserProfile.AgeOverAttribute))
                .Returns(_ageOverAttributes);
            mockBaseProfile.Setup(x => x.FindAttributesStartingWith<string>(Constants.UserProfile.AgeUnderAttribute))
               .Returns(_ageUnderAttributes);
            var yotiProfile = new YotiProfile(mockBaseProfile.Object);

            ReadOnlyCollection<AgeVerification> result = yotiProfile.AgeVerifications;

            Assert.AreEqual(4, result.Count);
            CollectionAssert.Contains(_allAgeVerificationAttributes, result[0].Attribute());
            CollectionAssert.Contains(_allAgeVerificationAttributes, result[1].Attribute());
            CollectionAssert.Contains(_allAgeVerificationAttributes, result[2].Attribute());
            CollectionAssert.Contains(_allAgeVerificationAttributes, result[3].Attribute());
        }

        [TestMethod]
        public void FindAgeOverVerification_returnsNullForNoMatch()
        {
            var mockBaseProfile = new Mock<IBaseProfile>();
            mockBaseProfile.Setup(
                x => x.FindAttributesStartingWith<string>(It.IsAny<string>()))
               .Returns(_emptyAttributeStringList);
            var yotiProfile = new YotiProfile(mockBaseProfile.Object);

            AgeVerification result = yotiProfile.FindAgeOverVerification(18);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindAgeOverVerification_ReturnsCorrectResult()
        {
            int age = 18;
            var ageover18List = new List<YotiAttribute<string>> { _ageOver18Attribute };

            var mockBaseProfile = new Mock<IBaseProfile>();

            mockBaseProfile.Setup(
                x => x.FindAttributesStartingWith<string>(Constants.UserProfile.AgeUnderAttribute))
               .Returns(_emptyAttributeStringList);
            mockBaseProfile.Setup(
                x => x.FindAttributesStartingWith<string>(Constants.UserProfile.AgeOverAttribute))
               .Returns(ageover18List);
            var yotiProfile = new YotiProfile(mockBaseProfile.Object);

            AgeVerification ageOverVerification = yotiProfile.FindAgeOverVerification(age);

            Assert.AreEqual(true, ageOverVerification.Result());
            Assert.AreEqual(age, ageOverVerification.Age());
            Assert.AreEqual(Constants.UserProfile.AgeOverAttribute, ageOverVerification.CheckType());
            Assert.AreEqual(_ageOver18Attribute, ageOverVerification.Attribute());
        }

        [TestMethod]
        public void FindAgeUnderVerification_returnsNullForNoMatch()
        {
            var mockBaseProfile = new Mock<IBaseProfile>();
            mockBaseProfile.Setup(
                x => x.FindAttributesStartingWith<string>(It.IsAny<string>()))
               .Returns(_emptyAttributeStringList);
            var yotiProfile = new YotiProfile(mockBaseProfile.Object);

            AgeVerification result = yotiProfile.FindAgeUnderVerification(18);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindAgeUnderVerification_ReturnsCorrectResult()
        {
            int age = 21;

            var mockBaseProfile = new Mock<IBaseProfile>();
            mockBaseProfile.Setup(
                x => x.FindAttributesStartingWith<string>(Constants.UserProfile.AgeOverAttribute))
               .Returns(_emptyAttributeStringList);
            mockBaseProfile.Setup(
                x => x.FindAttributesStartingWith<string>(Constants.UserProfile.AgeUnderAttribute))
               .Returns(new List<YotiAttribute<string>> { _ageUnder21Attribute });
            var yotiProfile = new YotiProfile(mockBaseProfile.Object);

            AgeVerification ageUnderVerification = yotiProfile.FindAgeUnderVerification(age);

            Assert.AreEqual(true, ageUnderVerification.Result());
            Assert.AreEqual(age, ageUnderVerification.Age());
            Assert.AreEqual(Constants.UserProfile.AgeUnderAttribute, ageUnderVerification.CheckType());
            Assert.AreEqual(_ageUnder21Attribute, ageUnderVerification.Attribute());
        }
    }
}