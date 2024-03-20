﻿using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yoti.Auth;
using Yoti.Auth.DigitalIdentity;
using Yoti.Auth.DigitalIdentity.Policy;

namespace CoreExample.Controllers
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

                var policy = new PolicyBuilder()
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

                var sessionReq = new ShareSessionRequestBuilder().WithPolicy(policy)
                    .WithRedirectUri("https:/www.yoti.com").WithSubject(new
                    {
                        subject_id = "some_subject_id_string"
                    }).Build();

                var SessionResult = yotiClient.CreateShareSession(sessionReq);

                ViewBag.YotiClientSdkId = _clientSdkId;
                ViewBag.sessionID = SessionResult.Id;

                return View("DigitalIdentity", SessionResult);
            }
            catch (Exception e)
            {
                _logger.LogError(
                     exception: e,
                     message: e.Message);

                TempData["Error"] = e.Message; 
                TempData["InnerException"] = e.InnerException?.Message;
                return RedirectToAction("Error", "Account");
            }
        }

        [Route("receipt-info")]
        // GET: receipt-info?ReceiptID
        public IActionResult ReceiptInfo(string ReceiptID)
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
                
                var ReceiptResult = yotiClient.GetShareReceipt(ReceiptID);

                ViewBag.YotiClientSdkId = _clientSdkId;
                
                return View("DigitalIdentity", ReceiptResult);
            }
            catch (Exception e)
            {
                _logger.LogError(
                     exception: e,
                     message: e.Message);

                TempData["Error"] = e.Message;
                TempData["InnerException"] = e.InnerException?.Message;
                return RedirectToAction("Error", "Account");
            }
        }
    } 
}
