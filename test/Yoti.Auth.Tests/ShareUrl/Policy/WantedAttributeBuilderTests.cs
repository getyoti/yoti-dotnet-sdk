using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.ShareUrl.Policy;

namespace Yoti.Auth.Tests.ShareUrl.Policy
{
    [TestClass]
    public class WantedAttributeBuilderTests
    {
        private const string _someName = "some name";
        private const string _someDerivation = "some derivation";

        [TestMethod]
        public void BuildsAnAttribute()
        {
            SourceConstraint sourceConstraint = new SourceConstraintBuilder()
                .WithDrivingLicense()
                .Build();

            WantedAttribute result = new WantedAttributeBuilder()
                .WithName(_someName)
                .WithDerivation(_someDerivation)
                .WithConstraint(sourceConstraint)
                .Build();

            Assert.AreEqual(_someName, result.Name);
            Assert.AreEqual(_someDerivation, result.Derivation);
        }

        [TestMethod]
        public void ShouldSetAcceptSelfAssertedToFalseByDefault()
        {
            WantedAttribute result = new WantedAttributeBuilder()
                .WithName("name")
                .Build();

            Assert.AreEqual(false, result.AcceptSelfAsserted);
        }

        [TestMethod]
        public void ShouldRetainLatestAcceptSelfAsserted()
        {
            WantedAttribute result = new WantedAttributeBuilder()
                .WithName("name")
                .WithAcceptSelfAsserted(false)
                .WithAcceptSelfAsserted(true)
                .Build();

            Assert.AreEqual(true, result.AcceptSelfAsserted);
        }

        [TestMethod]
        public void ShouldGenerateWithAnchor()
        {
            string wantedAnchorName = "name";
            string wantedAnchorSubType = "subType";
            Constraint sourceConstraint = new SourceConstraintBuilder()
                .WithAnchor(new WantedAnchor(wantedAnchorName, wantedAnchorSubType))
                .Build();

            WantedAttribute wantedAttribute = new WantedAttributeBuilder()
                .WithName("attribute_name")
                .WithConstraint(sourceConstraint)
                .Build();

            var result = (SourceConstraint)wantedAttribute.Constraints.Single();

            Assert.AreEqual(wantedAnchorName, result.PreferredSources.WantedAnchors.Single().Name);
            Assert.AreEqual(wantedAnchorSubType, result.PreferredSources.WantedAnchors.Single().SubType);
        }

        [TestMethod]
        public void ShouldGenerateWithPasscard()
        {
            Constraint sourceConstraint = new SourceConstraintBuilder()
                .WithPasscard()
                .Build();

            WantedAttribute wantedAttribute = new WantedAttributeBuilder()
                .WithName("attribute_name")
                .WithConstraint(sourceConstraint)
                .Build();

            var result = (SourceConstraint)wantedAttribute.Constraints.Single();
            Assert.AreEqual("PASS_CARD", result.PreferredSources.WantedAnchors[0].Name);
            Assert.AreEqual("", result.PreferredSources.WantedAnchors[0].SubType);
        }

        [TestMethod]
        public void ShouldGenerateTwoSourceConstraints()
        {
            Constraint sourceConstraint = new SourceConstraintBuilder()
                .WithPassport()
                .WithNationalId("AADHAR")
                .WithSoftPreference(true)
                .Build();

            WantedAttribute wantedAttribute = new WantedAttributeBuilder()
                .WithName("attribute_name")
                .WithConstraint(sourceConstraint)
                .Build();

            var result = (SourceConstraint)wantedAttribute.Constraints.Single();
            Assert.IsTrue(result.PreferredSources.SoftPreference);
            Assert.AreEqual("SOURCE", result.ConstraintType);

            Assert.AreEqual("PASSPORT", result.PreferredSources.WantedAnchors[0].Name);
            Assert.AreEqual("", result.PreferredSources.WantedAnchors[0].SubType);

            Assert.AreEqual("NATIONAL_ID", result.PreferredSources.WantedAnchors[1].Name);
            Assert.AreEqual("AADHAR", result.PreferredSources.WantedAnchors[1].SubType);
        }

        [TestMethod]
        public void WithConstraintShouldAddToCurrentConstraints()
        {
            Constraint drivingLicenseConstraint = new SourceConstraintBuilder()
               .WithDrivingLicense()
               .Build();

            Constraint passcardConstraint = new SourceConstraintBuilder()
               .WithPasscard()
               .Build();

            WantedAttribute wantedAttribute = new WantedAttributeBuilder()
                .WithName("attribute_name")
                .WithConstraints(new List<Constraint> { drivingLicenseConstraint })
                .WithConstraint(passcardConstraint)
                .Build();

            Assert.AreEqual(2, wantedAttribute.Constraints.Count);

            var sourceConstraint1 = (SourceConstraint)wantedAttribute.Constraints.First();
            Assert.AreEqual("DRIVING_LICENCE", sourceConstraint1.PreferredSources.WantedAnchors[0].Name);

            var sourceConstraint2 = (SourceConstraint)wantedAttribute.Constraints.Last();
            Assert.AreEqual("PASS_CARD", sourceConstraint2.PreferredSources.WantedAnchors[0].Name);
        }

        [TestMethod]
        public void WithConstraintsShouldOverrideCurrentConstraint()
        {
            Constraint drivingLicenseConstraint = new SourceConstraintBuilder()
               .WithDrivingLicense()
               .Build();

            Constraint passcardConstraint = new SourceConstraintBuilder()
               .WithPasscard()
               .Build();

            WantedAttribute wantedAttribute = new WantedAttributeBuilder()
                .WithName("attribute_name")
                .WithConstraint(passcardConstraint)
                .WithConstraints(new List<Constraint> { drivingLicenseConstraint })
                .Build();

            var result = (SourceConstraint)wantedAttribute.Constraints.Single();
            Assert.AreEqual(1, result.PreferredSources.WantedAnchors.Count);
            Assert.AreEqual("DRIVING_LICENCE", result.PreferredSources.WantedAnchors[0].Name);
        }
    }
}