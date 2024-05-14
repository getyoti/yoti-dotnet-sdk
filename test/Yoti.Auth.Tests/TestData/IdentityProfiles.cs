using Newtonsoft.Json;
using Yoti.Auth.DigitalIdentity.Policy;
using System.Collections.Generic;

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
        
        public static Yoti.Auth.DocScan.Session.Create.AdvancedIdentityProfile CreateStandardAdvancedIdentityProfileRequirements()
        {
            Yoti.Auth.DocScan.Session.Create.AdvancedIdentityProfile data = new Yoti.Auth.DocScan.Session.Create.AdvancedIdentityProfile
            {
                profiles = new List<Yoti.Auth.DocScan.Session.Create.Profile>
            {
                new Yoti.Auth.DocScan.Session.Create.Profile
                {
                    trust_framework = "UK_TFIDA",
                    schemes = new List<Yoti.Auth.DocScan.Session.Create.Scheme>
                    {
                        new Yoti.Auth.DocScan.Session.Create.Scheme
                        {
                            label = "LB912",
                            type = "RTW"
                        }
                    }
                },
                new Yoti.Auth.DocScan.Session.Create.Profile
                {
                    trust_framework = "YOTI_GLOBAL",
                    schemes = new List<Yoti.Auth.DocScan.Session.Create.Scheme>
                    {
                        new Yoti.Auth.DocScan.Session.Create.Scheme
                        {
                            label = "LB321",
                            type = "IDENTITY",
                            objective = "AL_L1"
                        }
                    }
                }
            }
            };

            return data;
        }
    }
}
