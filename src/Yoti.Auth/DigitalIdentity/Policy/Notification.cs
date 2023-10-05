using System;
using System.Collections.Generic;

namespace Yoti.Auth.DigitalIdentity.Policy
{
    public class Notification
    {
        public string Url { get; set; } // Required if 'notification' is defined
        public string Method { get; set; } = "POST"; // Optional, defaults to 'POST'
        public Dictionary<string, string> Headers { get; set; } // Optional
        public bool VerifyTls { get; set; } = true; // Optional, defaults to 'true' if URL is HTTPS
    }
}

