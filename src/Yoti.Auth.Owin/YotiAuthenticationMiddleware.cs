#if NETSTANDARD1_6
#else
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Owin;
using Owin;
using System.Globalization;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.DataHandler;
using System.Diagnostics.CodeAnalysis;
using Yoti.Auth;
using System.IO;

namespace Yoti.Auth.Owin
{
    /// <summary>
    /// OWIN middleware for authenticating users with Yoti
    /// </summary>
    public class YotiAuthenticationMiddleware : AuthenticationMiddleware<YotiAuthenticationOptions>
    {
        private readonly ILogger _logger;
        private readonly YotiClient _yotiClient;

        /// <summary>
        /// Initializes a <see cref="YotiAuthenticationMiddleware"/>
        /// </summary>
        /// <param name="next">The next middleware in the OWIN pipeline to invoke</param>
        /// <param name="app">The OWIN application</param>
        /// <param name="options">Configuration options for the middleware</param>
        public YotiAuthenticationMiddleware(
            OwinMiddleware next,
            IAppBuilder app,
            YotiAuthenticationOptions options,
            StreamReader privateKeySteam)
            : base(next, options)
        {
            if (string.IsNullOrWhiteSpace(Options.AppId))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Exception_OptionMustBeProvided, "AppId"));
            }

            if (string.IsNullOrWhiteSpace(Options.SdkId))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Exception_OptionMustBeProvided, "SdkId"));
            }

            if (privateKeySteam == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Exception_OptionMustBeProvided, "PrivateKeyPath"));
            }

            _logger = app.CreateLogger<YotiAuthenticationMiddleware>();

            if (Options.Provider == null)
            {
                Options.Provider = new YotiAuthenticationProvider();
            }
            if (Options.StateDataFormat == null)
            {
                IDataProtector dataProtecter = app.CreateDataProtector(
                    typeof(YotiAuthenticationMiddleware).FullName,
                    Options.AuthenticationType, "v1");
                Options.StateDataFormat = new PropertiesDataFormat(dataProtecter);
            }

            if (String.IsNullOrEmpty(Options.SignInAsAuthenticationType))
            {
                Options.SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType();
            }

            _yotiClient = new YotiClient(Options.SdkId, privateKeySteam);
        }

        /// <summary>
        /// Provides the <see cref="AuthenticationHandler"/> object for processing authentication-related requests.
        /// </summary>
        /// <returns>An <see cref="AuthenticationHandler"/> configured with the <see cref="YotiAuthenticationOptions"/> supplied to the constructor.</returns>
        protected override AuthenticationHandler<YotiAuthenticationOptions> CreateHandler()
        {
            return new YotiAuthenticationHandler(_yotiClient, _logger);
        }
    }
}
#endif