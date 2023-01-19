using Yoti.Auth.ShareUrl;
using Yoti.Auth.ShareUrl.Policy;
using Yoti.Auth.Tests.TestData;

namespace Yoti.Auth.Tests.TestTools
{
    internal static class ShareUrl
    {
        public static DynamicScenario CreateStandardDynamicScenario()
        {
            return new DynamicScenario("callback", CreateStandardPolicy());
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
              .WithCreateIdentityProfilePreview(true)
              .Build();
        }
    }
}
