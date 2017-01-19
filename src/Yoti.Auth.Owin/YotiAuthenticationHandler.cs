#if NETSTANDARD1_6
#else
using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Yoti.Auth;

namespace Yoti.Auth.Owin
{
    internal class YotiAuthenticationHandler : AuthenticationHandler<YotiAuthenticationOptions>
    {
        private const string AuthorizeEndpoint = "https://www.yoti.com/connect/";

        private readonly ILogger _logger;
        private readonly YotiClient _yotiClient;

        public YotiAuthenticationHandler(YotiClient yotiClient, ILogger logger)
        {
            _yotiClient = yotiClient;
            _logger = logger;
        }

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            AuthenticationProperties properties = null;

            try
            {
                string token = null;

                IReadableStringCollection query = Request.Query;
                IList<string> values = query.GetValues("token");
                if (values != null && values.Count == 1)
                {
                    token = values[0];
                }

                string stateCookieKey = Constants.StatePrefix + Options.AuthenticationType;
                string stateCookie = Request.Cookies[stateCookieKey];
                if (string.IsNullOrWhiteSpace(stateCookie))
                {
                    _logger.WriteWarning("{0} cookie not found.", stateCookie);
                    return null;
                }

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsSecure
                };

                Response.Cookies.Delete(stateCookieKey, cookieOptions);

                properties = Options.StateDataFormat.Unprotect(stateCookie);
                if (properties == null)
                {
                    return null;
                }

                // Request the token
                ActivityDetails activityDetails = await _yotiClient.GetActivityDetailsAsync(token);

                if (activityDetails.Outcome != ActivityOutcome.Success)
                {
                    // TODO: Check how this is handled
                    throw new HttpRequestException();
                }

                var context = new YotiAuthenticatedContext(Context, activityDetails.UserProfile);

                context.Identity = new ClaimsIdentity(
                    Options.AuthenticationType,
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                if (!string.IsNullOrEmpty(context.User.Id))
                {
                    context.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, context.User.Id, ClaimValueTypes.String, Options.AuthenticationType));
                }

                if (context.User.Selfie != null)
                {
                    context.Identity.AddClaim(new Claim("selfie", Convert.ToBase64String(context.User.Selfie.Data), context.User.Selfie.Type.ToString(), Options.AuthenticationType));
                }

                if (!string.IsNullOrEmpty(context.User.GivenNames))
                {
                    context.Identity.AddClaim(new Claim("given_names", context.User.GivenNames, ClaimValueTypes.String, Options.AuthenticationType));
                }

                if (!string.IsNullOrEmpty(context.User.FamilyName))
                {
                    context.Identity.AddClaim(new Claim("family_name", context.User.FamilyName, ClaimValueTypes.String, Options.AuthenticationType));
                }

                if (!string.IsNullOrEmpty(context.User.MobileNumber))
                {
                    context.Identity.AddClaim(new Claim("phone_number", context.User.MobileNumber, ClaimValueTypes.String, Options.AuthenticationType));
                }

                if (context.User.DateOfBirth != null)
                {
                    context.Identity.AddClaim(new Claim("date_of_birth", context.User.DateOfBirth.Value.ToString("yyyy-MM-dd"), ClaimValueTypes.String, Options.AuthenticationType));
                }

                if (!string.IsNullOrEmpty(context.User.Address))
                {
                    context.Identity.AddClaim(new Claim("post_code", context.User.Address, ClaimValueTypes.String, Options.AuthenticationType));
                }

                if (!string.IsNullOrEmpty(context.User.Gender))
                {
                    context.Identity.AddClaim(new Claim("gender", context.User.Gender, ClaimValueTypes.String, Options.AuthenticationType));
                }

                if (!string.IsNullOrEmpty(context.User.Nationality))
                {
                    context.Identity.AddClaim(new Claim("nationality", context.User.Nationality, ClaimValueTypes.String, Options.AuthenticationType));
                }

                foreach (var attributeName in context.User.OtherAttributes.Keys)
                {
                    var attributeValue = context.User.OtherAttributes[attributeName];
                    context.Identity.AddClaim(new Claim(attributeName, attributeValue.ToString(), attributeValue.Type.ToString(), Options.AuthenticationType));
                }
                
                context.Properties = properties;

                await Options.Provider.Authenticated(context);

                return new AuthenticationTicket(context.Identity, context.Properties);
            }
            catch (Exception ex)
            {
                _logger.WriteError("Authentication failed", ex);
                return new AuthenticationTicket(null, properties);
            }
        }

        protected override Task ApplyResponseChallengeAsync()
        {
            if (Response.StatusCode != 401)
            {
                return Task.FromResult<object>(null);
            }

            AuthenticationResponseChallenge challenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);

            if (challenge != null)
            {
                string baseUri =
                    Request.Scheme +
                    Uri.SchemeDelimiter +
                    Request.Host +
                    Request.PathBase;

                string currentUri =
                    baseUri +
                    Request.Path +
                    Request.QueryString;

                AuthenticationProperties properties = challenge.Properties;
                if (string.IsNullOrEmpty(properties.RedirectUri))
                {
                    properties.RedirectUri = currentUri;
                }
                
                string authorizationEndpoint = AuthorizeEndpoint + Options.AppId;

                string state = Options.StateDataFormat.Protect(properties);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsSecure
                };
                
                string stateCookieKey = Constants.StatePrefix + Options.AuthenticationType;
                Response.Cookies.Append(stateCookieKey, state, cookieOptions);

                var redirectContext = new YotiApplyRedirectContext(
                    Context, Options,
                    properties, authorizationEndpoint);

                Options.Provider.ApplyRedirect(redirectContext);
            }

            return Task.FromResult<object>(null);
        }

        public override async Task<bool> InvokeAsync()
        {
            return await InvokeReplyPathAsync();
        }

        private async Task<bool> InvokeReplyPathAsync()
        {
            if (Options.CallbackPath.HasValue && Options.CallbackPath == Request.Path)
            {
                AuthenticationTicket ticket = await AuthenticateAsync();

                if (ticket == null)
                {
                    _logger.WriteWarning("Invalid return state, unable to redirect.");
                    Response.StatusCode = 500;
                    return true;
                }



                var context = new YotiReturnEndpointContext(Context, ticket);
                context.SignInAsAuthenticationType = Options.SignInAsAuthenticationType;
                context.RedirectUri = ticket.Properties.RedirectUri;

                await Options.Provider.ReturnEndpoint(context);

                if (context.SignInAsAuthenticationType != null &&
                    context.Identity != null)
                {
                    ClaimsIdentity grantIdentity = context.Identity;
                    if (!string.Equals(grantIdentity.AuthenticationType, context.SignInAsAuthenticationType, StringComparison.Ordinal))
                    {
                        grantIdentity = new ClaimsIdentity(grantIdentity.Claims, context.SignInAsAuthenticationType, grantIdentity.NameClaimType, grantIdentity.RoleClaimType);
                    }
                    
                    Context.Authentication.SignIn(context.Properties, grantIdentity);
                }

                if (!context.IsRequestCompleted && context.RedirectUri != null)
                {
                    string redirectUri = context.RedirectUri;
                    if (context.Identity == null)
                    {
                        // add a redirect hint that sign-in failed in some way
                        redirectUri += "&error=access_denied";
                    }
                    Response.Redirect(redirectUri);
                    context.RequestCompleted();
                }

                return context.IsRequestCompleted;
            }
            return false;
        }

        private static void AddQueryString(IDictionary<string, string> queryStrings, AuthenticationProperties properties,
            string name, string defaultValue = null)
        {
            string value;
            if (!properties.Dictionary.TryGetValue(name, out value))
            {
                value = defaultValue;
            }
            else
            {
                // Remove the parameter from AuthenticationProperties so it won't be serialized to state parameter
                properties.Dictionary.Remove(name);
            }

            if (value == null)
            {
                return;
            }

            queryStrings[name] = value;
        }
    }
}
#endif