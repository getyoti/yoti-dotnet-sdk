using System;
using System.Configuration;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
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

                ViewBag.RememberMeID = activityDetails.RememberMeId;

                var selfie = profile.Selfie.GetValue();

                if (profile.Selfie != null)
                {
                    ViewBag.Base64Photo = selfie.GetBase64URI();
                    SetPhotoBytes(selfie.GetContent());
                }
                else
                {
                    ViewBag.Message = "No photo provided, change the application settings to request a photo from the user for this demo";
                }

                var identity = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, activityDetails.RememberMeId.ToString()),
                        },
                    "ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                authManager.SignIn(identity);

                return View(profile);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return RedirectToAction("LoginFailure", "Home");
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