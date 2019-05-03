using Yoti.Auth.ShareUrl;
using Yoti.Auth.ShareUrl.Policy;

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
              .WithDateOfBirth(optional: true)
              .WithAgeOver(18, optional: true)
              .WithAgeUnder(30, optional: true)
              .WithAgeUnder(40, optional: true)
              .WithPinAuthorisation(true)
              .Build();
        }
    }
}