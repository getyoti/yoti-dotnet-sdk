#if NETSTANDARD1_6
#else
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using System.IO;

namespace Yoti.Auth.Owin
{
    /// <summary>
    /// Configuration options for <see cref="YotiAuthenticationMiddleware"/>
    /// </summary>

    public class YotiAuthenticationOptions : AuthenticationOptions
    {
        /// <summary>
        /// Initializes a new <see cref="YotiAuthenticationOptions"/>
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
            MessageId = "Yoti.Owin.Security.YotiAuthenticationOptions.set_Caption(System.String)",
            Justification = "Not localizable.")]
        public YotiAuthenticationOptions()
            : base(Constants.DefaultAuthenticationType)
        {
            Caption = Constants.DefaultAuthenticationType;
            CallbackPath = new PathString("/signin-yoti");
            AuthenticationMode = AuthenticationMode.Passive;
        }

        /// <summary>
        /// Gets or sets the application-id provided by Yoti
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets the sdk-id provided by Yoti
        /// </summary>
        public string SdkId { get; set; }

        /// <summary>
        /// The request path within the application's base path where the user-agent will be returned.
        /// The middleware will process this request when it arrives.
        /// Default value is "/signin-yoti".
        /// </summary>
        public PathString CallbackPath { get; set; }

        /// <summary>
        /// Get or sets the text that the user can display on a sign in user interface.
        /// </summary>
        public string Caption
        {
            get { return Description.Caption; }
            set { Description.Caption = value; }
        }

        /// <summary>
        /// Gets or sets the name of another authentication middleware which will be responsible for actually issuing a user <see cref="System.Security.Claims.ClaimsIdentity"/>.
        /// </summary>
        public string SignInAsAuthenticationType { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IYotiAuthenticationProvider"/> used to handle authentication events.
        /// </summary>
        public IYotiAuthenticationProvider Provider { get; set; }

        /// <summary>
        /// Gets or sets the type used to secure data handled by the middleware.
        /// </summary>
        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }
    }
}
#endif