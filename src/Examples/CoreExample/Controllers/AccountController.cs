using System;
using Microsoft.AspNetCore.Mvc;
using Yoti.Auth;

namespace Example.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _appId = Environment.GetEnvironmentVariable("YOTI_APPLICATION_ID");
        public static byte[] PhotoBytes { get; set; }

        // GET: Account/Connect?token
        public ActionResult Connect(string token)
        {
            try
            {
                ViewBag.YotiAppId = _appId;
                string sdkId = Environment.GetEnvironmentVariable("YOTI_CLIENT_SDK_ID");
                var yotiKeyFilePath = Environment.GetEnvironmentVariable("YOTI_KEY_FILE_PATH");
                var privateKeyStream = System.IO.File.OpenText(yotiKeyFilePath);

                var yotiClient = new YotiClient(sdkId, privateKeyStream);

                var activityDetails = yotiClient.GetActivityDetails(token);
                if (activityDetails.Outcome == ActivityOutcome.Success)
                {
                    var yotiProfile = activityDetails.Profile;

                    if (yotiProfile.Selfie != null)
                    {
                        PhotoBytes = yotiProfile.Selfie.GetImage().Data;
                    }

                    return View(yotiProfile);
                }
                else
                {
                    return RedirectToAction("LoginFailure", "Home");
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.ToString();
                return RedirectToAction("LoginFailure", "Home");
            }
        }

        public FileContentResult DownloadImageFile()
        {
            if (PhotoBytes == null)
                throw new InvalidOperationException("The 'PhotoBytes' variable has not been set");

            return File(PhotoBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "YotiSelfie.jpg");
        }
    }
}