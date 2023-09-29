using Yoti.Auth.ShareUrl;
using Yoti.Auth.DigitalIdentity.Policy;
using Yoti.Auth.Tests.TestData;
using Yoti.Auth.DigitalIdentity;

namespace Yoti.Auth.Tests.TestTools
{
    internal static class ShareSession
    {
        public static ShareSessionRequest CreateStandardShareSessionRequest()
        {
            return new ShareSessionRequest("callback", CreateStandardPolicy());
        }

        public static DynamicPolicy CreateStandardPolicy()
        {
            return new DynamicPolicyBuilder()
              .WithDateOfBirth()
              .WithAgeOver(18)
              .WithAgeUnder(30)
              .WithAgeUnder(40)
              .WithPinAuthentication(true)
              .WithIdentityProfileRequirements(IdentityProfiles.CreateStandardIdentityProfileRequirements())
              .Build();
        }
    }
}
