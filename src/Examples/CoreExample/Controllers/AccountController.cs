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

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        public static byte[] PhotoBytes { get; set; }

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
                if (activityDetails.Outcome == ActivityOutcome.Success)
                {
                    _logger.LogInformation("ActivityOutcome=Success");

                    ViewBag.RememberMeID = activityDetails.RememberMeId;

                    YotiProfile yotiProfile = activityDetails.Profile;

                    if (yotiProfile.Selfie != null)
                    {
                        PhotoBytes = yotiProfile.Selfie.GetValue().Content;
                    }

                    return View(yotiProfile);
                }
                else
                {
                    _logger.LogWarning(
                        string.Format(
                            "ActivityOutcome='{0}'",
                            activityDetails.Outcome));
                    return RedirectToAction("LoginFailure", "Home");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(
                    exception: e,
                    message: "An error occurred");

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