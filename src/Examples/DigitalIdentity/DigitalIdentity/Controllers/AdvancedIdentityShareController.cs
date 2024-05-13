using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Yoti.Auth;
using Yoti.Auth.DigitalIdentity;
using Yoti.Auth.DigitalIdentity.Policy;

namespace DigitalIdentityExample.Controllers
{
    public class AdvancedIdentityShareController : Controller
    {
        private readonly string _clientSdkId;
        private readonly ILogger _logger;
        public AdvancedIdentityShareController(ILogger<AdvancedIdentityShareController> logger)
        {
            _logger = logger;

            _clientSdkId = Environment.GetEnvironmentVariable("YOTI_CLIENT_SDK_ID");
            _logger.LogInformation(string.Format("Yoti Client SDK ID='{0}'", _clientSdkId));
        }
       
        // GET: /advanced-identity-share
        [Route("advanced-identity-share")]
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
                
                string advancedIdentityProfileJson = @"
            {
                ""profiles"": [
                    {
                        ""trust_framework"": ""YOTI_GLOBAL"",
                        ""schemes"": [
                            {
                                ""label"": ""identity-AL-L1"",
                                ""type"": ""IDENTITY"",
                                ""objective"": ""AL_L1"",
                        
                            },
                            {
                                ""label"": ""identity-AL-M1"",
                                ""type"": ""IDENTITY"",
                                ""objective"": ""AL_M1"",
                        
                            }
                        ]
                    }
                ]

            }";
                
                var advancedIdentityProfile = JsonConvert.DeserializeObject<AdvancedIdentityProfile>(advancedIdentityProfileJson);
                
                var policy = new PolicyBuilder()
                    .WithAdvancedIdentityProfileRequirements(advancedIdentityProfile)
                    .Build();

                var sessionReq = new ShareSessionRequestBuilder().WithPolicy(policy)
                    .WithRedirectUri("https:/www.yoti.com")
                    .Build();

                var SessionResult = yotiClient.CreateShareSession(sessionReq);

                var sharedReceiptResponse = new SharedReceiptResponse();
                ViewBag.YotiClientSdkId = _clientSdkId;
                ViewBag.sessionID = SessionResult.Id;

                return View("AdvancedIdentityShare", sharedReceiptResponse);
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
