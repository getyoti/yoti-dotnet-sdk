using System;
using System.Configuration;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Example.Models;
using Yoti.Auth;

namespace Example.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _appId = ConfigurationManager.AppSettings["YOTI_APPLICATION_ID"];
        private byte[] _photoBytes;

        public byte[] GetPhotoBytes()
        {
            return _photoBytes;
        }

        public void SetPhotoBytes(byte[] value)
        {
            _photoBytes = value;
        }

        public ActionResult LogIn()
        {
            ViewBag.YotiAppId = _appId;
            return View();
        }

        // GET: Account/Connect?token
        public ActionResult Connect(string token)
        {
            if (token == null)
                return RedirectToAction("Index", "Home");

            try
            {
                string sdkId = ConfigurationManager.AppSettings["YOTI_CLIENT_SDK_ID"];
                var privateKeyStream = System.IO.File.OpenText(ConfigurationManager.AppSettings["YOTI_KEY_FILE_PATH"]);
                var yotiClient = new YotiClient(sdkId, privateKeyStream);

                var activityDetails = yotiClient.GetActivityDetails(token);
                var profile = activityDetails.Profile;

                var user = new User
                {
                    RememberMeID = activityDetails.RememberMeId
                };

                if (profile.Selfie != null)
                {
                    user.Base64Photo = profile.Selfie.GetValue().GetBase64URI();
                    user.Photo = profile.Selfie.GetValue().GetContent();
                    SetPhotoBytes(user.Photo);
                }
                else
                {
                    ViewBag.Message = "No photo provided, change the application settings to request a photo from the user for this demo";
                }

                UpdateAttributesIfPresent(profile, user);

                var identity = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, user.RememberMeID.ToString()),
                        },
                    "ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                authManager.SignIn(identity);

                return View(user);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.ToString();
                return RedirectToAction("LoginFailure", "Home");
            }
        }

        private static void UpdateAttributesIfPresent(YotiProfile yotiProfile, User user)
        {
            Type userType = user.GetType();
            foreach (PropertyInfo yotiProfileProperty in yotiProfile.GetType().GetProperties())
            {
                if (!yotiProfileProperty.CanRead || (yotiProfileProperty.GetIndexParameters().Length > 0))
                    continue;

                PropertyInfo userProperty = userType.GetProperty(yotiProfileProperty.Name);
                if ((userProperty != null) && (userProperty.CanWrite) && yotiProfileProperty.Name != "Id")
                    userProperty.SetValue(user, yotiProfileProperty.GetValue(yotiProfile, null), null);
            }
        }

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");

            ViewBag.YotiAppId = _appId;
            return View();
        }

        public FileContentResult DownloadImageFile()
        {
            if (GetPhotoBytes() == null)
                throw new InvalidOperationException("The 'PhotoBytes' variable has not been set");

            return File(GetPhotoBytes(), System.Net.Mime.MediaTypeNames.Application.Octet, "YotiSelfie.jpg");
        }
    }
}