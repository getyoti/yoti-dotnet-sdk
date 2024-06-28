using Newtonsoft.Json;
using Yoti.Auth.DigitalIdentity.Policy;

namespace Yoti.Auth.Tests.TestData
{
    internal static class IdentityProfiles
    {
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
        
        public static AdvancedIdentityProfile CreateAdvancedIdentityProfileRequirements()
        {
            string advancedIdentityProfileJson = @"
            {
                ""profiles"": [
                    {
                        ""trust_framework"": ""UK_TFIDA"",
                        ""schemes"": [
                            {
                                ""label"": ""LB912"",
                                ""type"": ""RTW""
                            },
                            {
                                ""label"": ""LB777"",
                                ""type"": ""DBS"",
                                ""objective"": ""BASIC""
                            }
                        ]
                    },
                    {
                        ""trust_framework"": ""YOTI_GLOBAL"",
                        ""schemes"": [
                            {
                                ""label"": ""LB321"",
                                ""type"": ""IDENTITY"",
                                ""objective"": ""AL_L1"",
                                ""config"": {}
                            }
                        ]
                    }
                ]
            }";
            var advancedIdentityProfile = JsonConvert.DeserializeObject<AdvancedIdentityProfile>(advancedIdentityProfileJson);
            return advancedIdentityProfile;
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
