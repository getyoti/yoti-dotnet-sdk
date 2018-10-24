namespace Yoti.Auth
{
    internal static class YotiConstants
    {
        private const string DefaultYotiHost = @"https://api.yoti.com";
        public static string YotiApiPathPrefix = "/api/v1";
        public static string DefaultYotiApiUrl = DefaultYotiHost + YotiApiPathPrefix;
        public static string StagingYotiApiUrl = "https://staging0.api.yoti.com:8443" + YotiApiPathPrefix;
        public static string AuthKeyHeader = "X-Yoti-Auth-Key";
        public static string DigestHeader = "X-Yoti-Auth-Digest";
        public static string YotiSdkHeader = "X-Yoti-SDK";
        public static string SdkIdentifier = ".NET";
        public const string AgeVerified = "AgeVerified";

        public const string AgeOverAttribute = "age_over:";
        public const string AgeUnderAttribute = "age_under:";
        public const string GivenNamesAttribute = "given_names";
        public const string FamilyNameAttribute = "family_name";
        public const string FullNameAttribute = "full_name";
        public const string GenderAttribute = "gender";
        public const string SelfieAttribute = "selfie";
        public const string PhoneNumberAttribute = "phone_number";
        public const string EmailAddressAttribute = "email_address";
        public const string NationalityAttribute = "nationality";
        public const string AgeOver18Attribute = "age_over:18";
        public const string AgeUnder18Attribute = "age_under:18";
        public const string PostalAddressAttribute = "postal_address";
        public const string StructuredPostalAddressAttribute = "structured_postal_address";
        public const string DateOfBirthAttribute = "date_of_birth";

        public const string ApplicationNameAttribute = "application_name";
        public const string ApplicationLogoAttribute = "application_logo";
        public const string ApplicationURLAttribute = "application_url";
        public const string ApplicationReceiptBgColourAttribute = "application_receipt_bgcolor";
    }
}