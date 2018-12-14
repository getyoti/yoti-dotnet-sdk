using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yoti.Auth;

namespace CoreExample.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        private byte[] _photoBytes;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        public byte[] GetPhotoBytes()
        {
            return _photoBytes;
        }

        public void SetPhotoBytes(byte[] value)
        {
            _photoBytes = value;
        }

        // GET: Account/Connect?token
        public ActionResult Connect(string token)
        {
            try
            {
                string sdkId = Environment.GetEnvironmentVariable("YOTI_CLIENT_SDK_ID");
                _logger.LogInformation(string.Format("sdkId='{0}'", sdkId));

                string yotiKeyFilePath = Environment.GetEnvironmentVariable("YOTI_KEY_FILE_PATH");
                _logger.LogInformation(
                    string.Format(
                        "yotiKeyFilePath='{0}'",
                        yotiKeyFilePath));

                StreamReader privateKeyStream = System.IO.File.OpenText(yotiKeyFilePath);

                var yotiClient = new YotiClient(sdkId, privateKeyStream);

                ActivityDetails activityDetails = yotiClient.GetActivityDetails(token);

                _logger.LogInformation("ActivityOutcome=Success");

                ViewBag.RememberMeID = activityDetails.RememberMeId;

                YotiProfile yotiProfile = activityDetails.Profile;

                if (yotiProfile.Selfie != null)
                {
                    SetPhotoBytes(yotiProfile.Selfie.GetValue().GetContent());
                }

                return View(yotiProfile);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    exception: e,
                    message: "An error occurred");
                ViewBag.Error = e.Message;

                return RedirectToAction("LoginFailure", "Home");
            }
        }

        public FileContentResult DownloadImageFile()
        {
            if (GetPhotoBytes() == null)
                throw new InvalidOperationException("The 'PhotoBytes' variable has not been set");

            return File(GetPhotoBytes(), System.Net.Mime.MediaTypeNames.Application.Octet, "YotiSelfie.jpg");
        }
    }
}