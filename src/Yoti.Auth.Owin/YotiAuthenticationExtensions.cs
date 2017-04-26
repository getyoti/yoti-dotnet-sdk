#if NETSTANDARD1_6
#else
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Yoti.Auth.Owin
{
    /// <summary>
    /// Extension methods for using <see cref="YotiAuthenticationMiddleware"/>
    /// </summary>
    public static class YotiAuthenticationExtensions
    {
        /// <summary>
        /// Authenticate users using Yoti
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder"/> passed to the configuration method</param>
        /// <param name="options">Middleware configuration options</param>
        /// <returns>The updated <see cref="IAppBuilder"/></returns>
        public static IAppBuilder UseYotiAuthentication(this IAppBuilder app, YotiAuthenticationOptions options, StreamReader privateKeyStream)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            app.Use(typeof(YotiAuthenticationMiddleware), app, options, privateKeyStream);
            return app;
        }

        /// <summary>
        /// Authenticate users using Yoti
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder"/> passed to the configuration method</param>
        /// <param name="appId">The application id assigned by Yoti</param>
        /// <param name="sdkId">The sdk id assigned by Yoti</param>
        /// <param name="privateKeyPath">A path to the private key file provided by Yoti</param>
        /// <returns></returns>
        public static IAppBuilder UseYotiAuthentication(
            this IAppBuilder app,
            string appId,
            string sdkId,
            StreamReader privateKeyStream,
            string callbackPath)
        {
            return UseYotiAuthentication(
                app,
                new YotiAuthenticationOptions
                {
                    AppId = appId,
                    SdkId = sdkId,
                    CallbackPath = new PathString(callbackPath)
                }, privateKeyStream);
        }

        /// <summary>
        /// Gets the <see cref="YotiUserProfile"/> provided by Yoti after a successful login
        /// </summary>
        /// <param name="loginInfo">The <see cref="ExternalLoginInfo"/> retrieved after an external login event.</param>
        /// <returns>The <see cref="YotiUserProfile"/> or null if this was not a Yoti login.</returns>
        public static YotiUserProfile GetYotiProfile(this ExternalLoginInfo loginInfo)
        {
            if (loginInfo.Login.LoginProvider != Constants.DefaultAuthenticationType)
            {
                return null;
            }

            var profile = new YotiUserProfile();

            foreach(Claim claim in loginInfo.ExternalIdentity.Claims)
            {
                switch (claim.Type)
                {
                    case ClaimTypes.NameIdentifier:
                        profile.Id = claim.Value;
                        break;

                    case "selfie":
                        ImageType imageType;
                        if (Enum.TryParse(claim.ValueType, out imageType))
                        {
                            profile.Selfie = new Image
                            {
                                Type = imageType,
                                Data = Convert.FromBase64String(claim.Value)
                            };
                        }
                        break;

                    case "given_names":
                        profile.GivenNames = claim.Value;
                        break;

                    case "family_name":
                        profile.FamilyName = claim.Value;
                        break;

                    case "phone_number":
                        profile.MobileNumber = claim.Value;
                        break;

                    case "email_address":
                        profile.EmailAddress = claim.Value;
                        break;

                    case "date_of_birth":
                        {
                            DateTime date;
                            if (DateTime.TryParseExact(claim.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                            {
                                profile.DateOfBirth = date;
                            }
                        }
                        break;

                    case "postal_address":
                        profile.Address = claim.Value;
                        break;

                    case "gender":
                        profile.Gender = claim.Value;
                        break;

                    case "nationality":
                        profile.Nationality = claim.Value;
                        break;

                    default:
                        YotiAttributeValue.TypeEnum valueType;
                        if (Enum.TryParse(claim.ValueType, out valueType))
                        {
                            profile.OtherAttributes.Add(claim.Type, new YotiAttributeValue(valueType, claim.Value));
                        }
                        break;
                }
            }
            return profile;
        }
    }
}
#endif
