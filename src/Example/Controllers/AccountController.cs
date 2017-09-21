using System;
using System.Configuration;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Example.Models;
using Yoti.Auth;

namespace Example.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LogIn()
        {
            ViewBag.YotiAppId = ConfigurationManager.AppSettings["Yoti.AppId"];
            return View();
        }

        // GET: Account/Connect?token
        public ActionResult Connect(string token)
        {
            try
            {
                string sdkId = ConfigurationManager.AppSettings["Yoti.SdkId"];
                var privateKeyStream = System.IO.File.OpenText(Server.MapPath("~/application-key.pem"));
                var yotiClient = new YotiClient(sdkId, privateKeyStream);

                var activityDetails = yotiClient.GetActivityDetails(token);
                if (activityDetails.Outcome == ActivityOutcome.Success)
                {
                    var yotiProfile = activityDetails.UserProfile;

                    User user = UserManager.GetUserByYotiId(yotiProfile.Id);

                    // create new user if none exists
                    if (user == null)
                    {
                        user = new User
                        {
                            YotiId = yotiProfile.Id
                        };
                    }

                    // update user information
                    if (yotiProfile.Selfie != null)
                    {
                        user.Photo = yotiProfile.Selfie.Data;
                    }

                    if (!string.IsNullOrEmpty(yotiProfile.MobileNumber))
                    {
                        user.PhoneNumber = yotiProfile.MobileNumber;
                    }

                    UserManager.SaveUser(user);

                    var identity = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                        },
                        "ApplicationCookie");

                    var ctx = Request.GetOwinContext();
                    var authManager = ctx.Authentication;

                    authManager.SignIn(identity);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("LoginFailure");
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.ToString();
                return View();
            }
        }

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");

            ViewBag.YotiAppId = ConfigurationManager.AppSettings["Yoti.AppId"];
            return View();
        }

        public ActionResult LoginFailure()
        {
            ViewBag.YotiAppId = ConfigurationManager.AppSettings["Yoti.AppId"];
            return View();
        }
    }
}