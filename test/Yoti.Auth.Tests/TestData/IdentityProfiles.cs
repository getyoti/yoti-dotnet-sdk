using System.Collections.Generic;
using Yoti.Auth.DocScan.Session.Create;

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

        public static object CreateStandardSubject()
        {
            return new
            {
                subject_id = "some_subject_id_string"
            };
        }
        
        public static AdvancedIdentityProfile CreateStandardAdvancedIdentityProfileRequirements()
        {
            AdvancedIdentityProfile data = new AdvancedIdentityProfile
            {
                profiles = new List<Yoti.Auth.DocScan.Session.Create.Profile>
            {
                new Yoti.Auth.DocScan.Session.Create.Profile
                {
                    trust_framework = "UK_TFIDA",
                    schemes = new List<Scheme>
                    {
                        new Scheme
                        {
                            label = "LB912",
                            type = "RTW"
                        }
                    }
                },
                new Yoti.Auth.DocScan.Session.Create.Profile
                {
                    trust_framework = "YOTI_GLOBAL",
                    schemes = new List<Scheme>
                    {
                        new Scheme
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
