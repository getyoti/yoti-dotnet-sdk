using System;
using System.IO;
using Newtonsoft.Json;
using Yoti.Auth;
using Yoti.Auth.Aml;

namespace AmlExample
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Yoti AML Example");

            CheckForEnvPresence();

            string sdkId = Environment.GetEnvironmentVariable("YOTI_CLIENT_SDK_ID");
            Console.WriteLine(string.Format("sdkId='{0}'", sdkId));

            string yotiKeyFilePath = Environment.GetEnvironmentVariable("YOTI_KEY_FILE_PATH");
            Console.WriteLine(
                string.Format(
                    "yotiKeyFilePath='{0}'",
                    yotiKeyFilePath));

            using (StreamReader privateKeyStream = File.OpenText(yotiKeyFilePath))
            {
                var yotiClient = new YotiClient(sdkId, privateKeyStream);

                AmlAddress amlAddress = new AmlAddress(country: "GBR");

                AmlProfile amlProfile = new AmlProfile(
                    givenNames: "Edward Richard George",
                    familyName: "Heath",
                    amlAddress: amlAddress);

                AmlResult amlResult = yotiClient.PerformAmlCheck(amlProfile);

                Console.WriteLine(string.Format(
                    "{0}{0}Completing check for AML profile: '{1}'",
                    Environment.NewLine,
                    JsonConvert.SerializeObject(amlProfile, Formatting.Indented)));

                Console.WriteLine(string.Format(
                   "{0}{0}Result:",
                   Environment.NewLine));
                Console.WriteLine(string.Format("Is on PEP list: {0}", amlResult.IsOnPepList()));
                Console.WriteLine(string.Format("Is on Fraud list: {0}", amlResult.IsOnFraudList()));
                Console.WriteLine(string.Format("Is on Watch list: {0}", amlResult.IsOnWatchList()));
            }
        }

        private static void CheckForEnvPresence()
        {
            if (File.Exists(".env"))
            {
                Console.WriteLine(string.Format(
                    "{0}Using environment variables from .env file:", Environment.NewLine));
                DotNetEnv.Env.Load();
            }
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("YOTI_CLIENT_SDK_ID")))
                throw new InvalidOperationException("'YOTI_CLIENT_SDK_ID' environment variable not found. " +
                    "Either pass these in the .env file, or as a standard environment variable.");
        }
    }
}