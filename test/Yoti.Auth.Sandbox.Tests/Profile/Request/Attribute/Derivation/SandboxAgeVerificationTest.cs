using System;
using Xunit;
using Yoti.Auth.Sandbox.Profile.Request.Attribute;
using Yoti.Auth.Sandbox.Profile.Request.Attribute.Derivation;

namespace Yoti.Auth.Sandbox.Tests.Profile.Request.Attribute.Derivation
{
    public static class SandboxAgeVerificationTest
    {
        private const string _validDateString = "1980-08-05";
        private static readonly DateTime _validDate = new DateTime(1980, 8, 5);

        [Fact]
        public static void ShouldErrorForBadDateOfBirth()
        {
            var exception = Assert.Throws<InvalidCastException>(() =>
            {
                SandboxAgeVerification.Builder()
                .WithDateOfBirth("2011-15-35")
                .Build();
            });

            Assert.Contains("Error when converting string value '2011-15-35' to a DateTime", exception.Message, StringComparison.Ordinal);
        }

        [Fact]
        public static void ShouldErrorForNullAnchors()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                SandboxAgeVerification.Builder()
                .WithAnchors(null);
            });

            Assert.Contains("Value cannot be null.", exception.Message, StringComparison.Ordinal);
            Assert.Contains("Parameter name: anchors", exception.Message, StringComparison.Ordinal);
        }

        [Fact]
        public static void ShouldErrorForMissingDateOfBirth()
        {
            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                SandboxAgeVerification.Builder()
                .Build();
            });

            Assert.Contains(
                "the value of 'dateOfBirth' must not be equal to the default value for 'System.DateTime'",
                exception.Message,
                StringComparison.Ordinal);
        }

        [Fact]
        public static void ShouldErrorForMissingDerivation()
        {
            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                SandboxAgeVerification.Builder()
                .WithDateOfBirth(_validDateString)
                .Build();
            });

            Assert.Contains("'supportedAgeDerivation' must not be empty or null", exception.Message, StringComparison.Ordinal);
        }

        [Fact]
        public static void ShouldParseDateOfBirthSuccessfully()
        {
            SandboxAgeVerification result = SandboxAgeVerification.Builder()
                .WithDateOfBirth(_validDateString)
                .WithAgeOver(7)
                .Build();

            Assert.Equal(_validDateString, result.ToAttribute().Value);
        }

        [Fact]
        public static void ShouldCreateAgeOverSandboxAttribute()
        {
            SandboxAttribute result = SandboxAgeVerification.Builder()
                .WithDateOfBirth(_validDateString)
                .WithAgeOver(21)
                .Build()
                .ToAttribute();

            Assert.Equal(Constants.UserProfile.DateOfBirthAttribute, result.Name);
            Assert.Equal(_validDateString, result.Value);
            Assert.Equal($"{Constants.UserProfile.AgeOverAttribute}:{21}", result.Derivation);
            Assert.Equal("False", result.Optional);
            Assert.Empty(result.Anchors);
        }

        [Fact]
        public static void ShouldCreateAgeUnderSandboxAttribute()
        {
            SandboxAttribute result = SandboxAgeVerification.Builder()
                .WithDateOfBirth(_validDateString)
                .WithAgeUnder(16)
                .Build()
                .ToAttribute();

            Assert.Equal(Constants.UserProfile.DateOfBirthAttribute, result.Name);
            Assert.Equal(_validDateString, result.Value);
            Assert.Equal($"{Constants.UserProfile.AgeUnderAttribute}:{16}", result.Derivation);
            Assert.Equal("False", result.Optional);
            Assert.Empty(result.Anchors);
        }

        [Fact]
        public static void ShouldCreateAgeVerificationWithAnchors()
        {
            SandboxAnchor sandboxAnchor = SandboxAnchor.Builder().Build();
            SandboxAttribute result = SandboxAgeVerification.Builder()
                .WithDateOfBirth(_validDate)
                .WithAgeUnder(16)
                .WithAnchors(new System.Collections.Generic.List<SandboxAnchor> { sandboxAnchor })
                .Build()
                .ToAttribute();

            Assert.Equal(Constants.UserProfile.DateOfBirthAttribute, result.Name);
            Assert.Equal(_validDateString, result.Value);
            Assert.Equal($"{Constants.UserProfile.AgeUnderAttribute}:{16}", result.Derivation);
            Assert.Equal("False", result.Optional);
            Assert.Equal(new System.Collections.Generic.List<SandboxAnchor> { sandboxAnchor }, result.Anchors);
        }
    }
}