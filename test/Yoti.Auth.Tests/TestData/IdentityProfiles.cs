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
    }
}
