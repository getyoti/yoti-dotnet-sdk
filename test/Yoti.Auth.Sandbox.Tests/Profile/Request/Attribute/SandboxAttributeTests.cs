using Xunit;
using Yoti.Auth.Sandbox.Profile.Request.Attribute;

namespace Yoti.Auth.Sandbox.Tests.Profile.Request.Attribute
{
    public static class SandboxAttributeTests
    {
        private const string _someName = "someName";
        private const string _someDerivation = "someDerivation";
        private const string _someValue = "someValue";

        [Fact]
        public static void ShouldNotBeOptionalByDefault()
        {
            SandboxAttribute result = SandboxAttribute.Builder()
                    .WithName(_someName)
                    .WithDerivation(_someDerivation)
                    .WithValue(_someValue)
                    .Build();

            Assert.Equal(_someName, result.Name);
            Assert.Equal(_someDerivation, result.Derivation);
            Assert.Equal(_someValue, result.Value);
            Assert.Equal("False", result.Optional);
        }

        [Fact]
        public static void ShouldBeOptionalWhenSpecified()
        {
            SandboxAttribute result = SandboxAttribute.Builder()
                    .WithName(_someName)
                    .WithDerivation(_someDerivation)
                    .WithValue(_someValue)
                    .WithOptional(true)
                    .Build();

            Assert.Equal(_someName, result.Name);
            Assert.Equal(_someDerivation, result.Derivation);
            Assert.Equal(_someValue, result.Value);
            Assert.Equal("True", result.Optional);
        }
    }
}