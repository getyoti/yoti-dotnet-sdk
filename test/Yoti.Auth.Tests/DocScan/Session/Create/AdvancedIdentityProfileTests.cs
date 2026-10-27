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
            // Profile/Scheme are fully qualified here (not brought in via `using` or alias)
            // because this file's own namespace is nested under Yoti.Auth, which also directly
            // contains the unrelated Yoti.Auth.Profile namespace — enclosing-namespace member
            // lookup resolves bare "Profile" to that namespace before any using directive (plain
            // or alias) is considered, causing CS0118 ("Profile is a namespace but is used like a
            // type") otherwise.
            var advancedIdentityProfile = new AdvancedIdentityProfile
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
                                type = "RTW",
                                objective = "STANDARD"
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

            Assert.AreEqual(1, deserialized.profiles.Count);
            Assert.AreEqual("UK_TFIDA", deserialized.profiles[0].trust_framework);
            Assert.AreEqual(1, deserialized.profiles[0].schemes.Count);
            Assert.AreEqual("LB912", deserialized.profiles[0].schemes[0].label);
            Assert.AreEqual("RTW", deserialized.profiles[0].schemes[0].type);
            Assert.AreEqual("STANDARD", deserialized.profiles[0].schemes[0].objective);
        }
    }
}
