using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Create;

namespace Yoti.Auth.Tests.DocScan.Session.Create
{
    [TestClass]
    public class AdvancedIdentityProfileTests
    {
        [TestMethod]
        public void ShouldSerializeAndDeserializeAdvancedIdentityProfile()
        {
            var advancedIdentityProfile = new AdvancedIdentityProfile
            {
                Profiles = new List<AdvancedIdentityProfile.Profile>
                {
                    new AdvancedIdentityProfile.Profile
                    {
                        TrustFramework = "UK_TFIDA",
                        Schemes = new List<AdvancedIdentityProfile.Scheme>
                        {
                            new AdvancedIdentityProfile.Scheme
                            {
                                Label = "LB912",
                                Type = "RTW",
                                Objective = "STANDARD"
                            }
                        }
                    }
                }
            };

            string json = JsonConvert.SerializeObject(advancedIdentityProfile);

            StringAssert.Contains(json, "\"trust_framework\":\"UK_TFIDA\"");
            StringAssert.Contains(json, "\"label\":\"LB912\"");
            StringAssert.Contains(json, "\"type\":\"RTW\"");
            StringAssert.Contains(json, "\"objective\":\"STANDARD\"");

            var deserialized = JsonConvert.DeserializeObject<AdvancedIdentityProfile>(json);

            Assert.AreEqual(1, deserialized.Profiles.Count);
            Assert.AreEqual("UK_TFIDA", deserialized.Profiles[0].TrustFramework);
            Assert.AreEqual(1, deserialized.Profiles[0].Schemes.Count);
            Assert.AreEqual("LB912", deserialized.Profiles[0].Schemes[0].Label);
            Assert.AreEqual("RTW", deserialized.Profiles[0].Schemes[0].Type);
            Assert.AreEqual("STANDARD", deserialized.Profiles[0].Schemes[0].Objective);
        }
    }
}
