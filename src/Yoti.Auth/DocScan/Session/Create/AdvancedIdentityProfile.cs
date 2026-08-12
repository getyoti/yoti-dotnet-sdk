using System;
using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Create
{
    public class Scheme
    {
        public string label { get; set; }
        public string type { get; set; }
        public string objective { get; set; }
    }

    public class Profile
    {
        public string trust_framework { get; set; }
        public List<Scheme> schemes { get; set; }
    }

    public class AdvancedIdentityProfile
    {
        public List<Profile> profiles { get; set; }
    }
}

