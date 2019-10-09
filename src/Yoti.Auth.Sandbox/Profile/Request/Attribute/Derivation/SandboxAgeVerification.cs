using System;
using System.Collections.Generic;
using System.Globalization;
using Yoti.Auth.Constants;

namespace Yoti.Auth.Sandbox.Profile.Request.Attribute.Derivation
{
    public class SandboxAgeVerification
    {
        private readonly DateTime _dateOfBirth;
        private readonly string _supportedAgeDerivation;
        private readonly List<SandboxAnchor> _anchors;

        public static SandboxAgeVerificationBuilder Builder()
        {
            return new SandboxAgeVerificationBuilder();
        }

        internal SandboxAgeVerification(DateTime dateOfBirth, string supportedAgeDerivation, List<SandboxAnchor> anchors)
        {
            Validation.IsNotDefault(dateOfBirth, nameof(dateOfBirth));
            Validation.NotNullOrEmpty(supportedAgeDerivation, nameof(supportedAgeDerivation));

            _dateOfBirth = dateOfBirth;
            _supportedAgeDerivation = supportedAgeDerivation;
            _anchors = anchors;
        }

        public SandboxAttribute ToAttribute()
        {
            return SandboxAttribute.Builder()
                    .WithName(UserProfile.DateOfBirthAttribute)
                    .WithValue(_dateOfBirth.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                    .WithDerivation(_supportedAgeDerivation)
                    .WithAnchors(_anchors)
                    .Build();
        }
    }
}