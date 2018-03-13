#if NETSTANDARD1_6
#else
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Yoti.Auth;

namespace Yoti.Auth.Owin
{
    /// <summary>
    /// Contains information about the login session as well as the user <see cref="System.Security.Claims.ClaimsIdentity"/>.
    /// </summary>
    public class YotiAuthenticatedContext : BaseContext
    {
        /// <summary>
        /// Initializes a <see cref="YotiAuthenticatedContext"/>
        /// </summary>
        /// <param name="context">The OWIN environment</param>
        /// <param name="user">The Yoti user info</param>
        public YotiAuthenticatedContext(IOwinContext context, YotiUserProfile user)
                : base(context)
            {
            User = user;
        }

        /// <summary>
        /// Gets the Yoti user
        /// </summary>
        /// <remarks>
        /// Contains the Yoti user profile
        /// </remarks>
        public YotiUserProfile User { get; private set; }

        /// <summary>
        /// Gets the <see cref="ClaimsIdentity"/> representing the user
        /// </summary>
        public ClaimsIdentity Identity { get; set; }

        /// <summary>
        /// Gets or sets a property bag for common authentication properties
        /// </summary>
        public AuthenticationProperties Properties { get; set; }
    }
}
#endif