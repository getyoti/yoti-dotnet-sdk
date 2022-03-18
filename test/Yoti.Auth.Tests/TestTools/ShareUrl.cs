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
              .WithDateOfBirth()
              .WithAgeOver(18)
              .WithAgeUnder(30)
              .WithAgeUnder(40)
              .WithPinAuthentication(true)
              .WithIdentityProfileRequirements(CreateStandardIdentityProfileRequirements())
              .Build();
        }

        public static object CreateStandardIdentityProfileRequirements()
        {
            return new
            {
                trust_framework = "UK_TFIDA",
                scheme = new
                {
                    type = "DBS",
                    objective = "STANDARD"
                }
            };
        }

        public static object CreateStandardSubject()
        {
            return new
            {
                subject_id = "some_subject_id_string"
            };
        }
    }
}