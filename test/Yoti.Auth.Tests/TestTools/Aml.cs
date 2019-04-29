using Yoti.Auth.Aml;

namespace Yoti.Auth.Tests.TestTools
{
    internal static class Aml
    {
        public static AmlProfile CreateStandardAmlProfile()
        {
            AmlAddress amlAddress = CreateStandardAmlAddress();

            var amlProfile = new AmlProfile(
                givenNames: "Edward Richard George",
                familyName: "Heath",
                amlAddress: amlAddress);
            return amlProfile;
        }

        public static AmlAddress CreateStandardAmlAddress()
        {
            return new AmlAddress(
               country: "GBR");
        }
    }
}