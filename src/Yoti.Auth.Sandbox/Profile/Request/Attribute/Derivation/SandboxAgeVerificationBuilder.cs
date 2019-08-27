using System;
using System.Collections.Generic;
using System.Globalization;
using Yoti.Auth.Constants;

namespace Yoti.Auth.Sandbox.Profile.Request.Attribute.Derivation
{
    public class SandboxAgeVerificationBuilder
    {
        private DateTime _dateOfBirth;
        private string _derivation;
        private List<SandboxAnchor> _anchors;

        public SandboxAgeVerificationBuilder WithDateOfBirth(string value)
        {
            bool success = DateTime.TryParseExact(
               s: value,
               format: "yyyy-MM-dd",
               provider: CultureInfo.InvariantCulture,
               style: DateTimeStyles.None,
               result: out DateTime parsedDate);

            if (success)
            {
                WithDateOfBirth(parsedDate);
                return this;
            }

            throw new InvalidCastException($"Error when converting string value '{value}' to a DateTime");
        }

        public SandboxAgeVerificationBuilder WithDateOfBirth(DateTime value)
        {
            Validation.NotNull(value, nameof(value));
            _dateOfBirth = value;
            return this;
        }

        public SandboxAgeVerificationBuilder WithAgeOver(int value)
        {
            return WithDerivation($"{UserProfile.AgeOverAttribute}:{value}");
        }

        public SandboxAgeVerificationBuilder WithAgeUnder(int value)
        {
            return WithDerivation($"{UserProfile.AgeUnderAttribute}:{value}");
        }

        public SandboxAgeVerificationBuilder WithDerivation(string value)
        {
            Validation.NotNullOrEmpty(value, nameof(value));
            _derivation = value;
            return this;
        }

        public SandboxAgeVerificationBuilder WithAnchors(List<SandboxAnchor> anchors)
        {
            Validation.NotNull(anchors, nameof(anchors));
            _anchors = anchors;
            return this;
        }

        public SandboxAgeVerification Build()
        {
            return new SandboxAgeVerification(_dateOfBirth, _derivation, _anchors);
        }
    }
}