using System;
using Xunit;
using Yoti.Auth.Sandbox.Profile.Request.Attribute;

namespace Yoti.Auth.Sandbox.Tests.Profile.Request.Attribute
{
    public static class SandboxAnchorTests
    {
        private static SandboxAnchorBuilder _builder;

        [Fact]
        public static void BuilderShouldSetEnumType()
        {
            _builder = SandboxAnchor.Builder();

            _builder.WithType(Anchors.AnchorType.SOURCE);
            SandboxAnchor result = _builder.Build();

            Assert.Equal("SOURCE", result.Type);
        }

        [Fact]
        public static void BuilderShouldSetStringType()
        {
            string type = "SOURCE";

            _builder = SandboxAnchor.Builder();

            _builder.WithType(type);
            SandboxAnchor result = _builder.Build();

            Assert.Equal(type, result.Type);
        }

        [Fact]
        public static void BuilderShouldSetValue()
        {
            string value = "value";

            _builder = SandboxAnchor.Builder();

            _builder.WithValue(value);
            SandboxAnchor result = _builder.Build();

            Assert.Equal(value, result.Value);
        }

        [Fact]
        public static void BuilderShouldSetSubType()
        {
            string subType = "subType";

            _builder = SandboxAnchor.Builder();

            _builder.WithSubType(subType);
            SandboxAnchor result = _builder.Build();

            Assert.Equal(subType, result.SubType);
        }

        [Fact]
        public static void BuilderShouldSetTimestamp()
        {
            DateTime timestamp = new DateTime(2019, 12, 31, 23, 59, 59, DateTimeKind.Utc);

            DateTimeOffset dto = new DateTimeOffset(timestamp);
            var expectedUnixTime = dto.ToUnixTimeMilliseconds() * 1000;

            _builder = SandboxAnchor.Builder();

            _builder.WithTimestamp(timestamp);
            SandboxAnchor result = _builder.Build();

            Assert.Equal(expectedUnixTime, result.UnixMicrosecondTimestamp);
        }
    }
}