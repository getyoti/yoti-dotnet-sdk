using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yoti.Auth;
using Yoti.Auth.DigitalIdentity;
using Yoti.Auth.DigitalIdentity.Policy;

namespace DigitalIdentityExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _clientSdkId;
        private readonly ILogger _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            _clientSdkId = Environment.GetEnvironmentVariable("YOTI_CLIENT_SDK_ID");
            _logger.LogInformation(string.Format("Yoti Client SDK ID='{0}'", _clientSdkId));
        }
       
        // GET: /generate-share
        [Route("generate-share")]
        public IActionResult DigitalIdentity()
        {
            try
            {
                string yotiKeyFilePath = Environment.GetEnvironmentVariable("YOTI_KEY_FILE_PATH");
                _logger.LogInformation(
                    string.Format(
                        "yotiKeyFilePath='{0}'",
                        yotiKeyFilePath));

                StreamReader privateKeyStream = System.IO.File.OpenText(yotiKeyFilePath);

                var yotiClient = new DigitalIdentityClient(_clientSdkId, privateKeyStream);

                var givenNamesWantedAttribute = new WantedAttributeBuilder()
                    .WithName("given_names")
                    .WithOptional(false)
                    .Build();
                
                var notification = new NotificationBuilder()
                    .WithUrl("https://example.com/webhook")
                    .WithMethod("POST")
                    .WithVerifyTls(true)
                    .Build();
                
                var policy = new PolicyBuilder()
                    .WithWantedAttribute(givenNamesWantedAttribute)
                    .WithFullName()
                    .WithEmail()
                    .WithPhoneNumber()
                    .WithSelfie()
                    .WithAgeOver(18)
                    .WithNationality()
                    .WithGender()
                    .WithDocumentDetails()
                    .WithDocumentImages()           
                    .Build();

                var sessionReq = new ShareSessionRequestBuilder()
                    .WithPolicy(policy)
                    .WithNotification(notification)
                    .WithRedirectUri("https:/www.yoti.com")
                    .WithSubject(new
                    {
                        subject_id = "some_subject_id_string"
                    }).Build();

                var SessionResult = yotiClient.CreateShareSession(sessionReq);

                var sharedReceiptResponse = new SharedReceiptResponse();
                ViewBag.YotiClientSdkId = _clientSdkId;
                ViewBag.sessionID = SessionResult.Id;

                return View("DigitalIdentity", sharedReceiptResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(
                     exception: e,
                     message: e.Message);

                TempData["Error"] = e.Message; 
                TempData["InnerException"] = e.InnerException?.Message;
                return RedirectToAction("Error", "Success");
            }
        }
    } 
}
