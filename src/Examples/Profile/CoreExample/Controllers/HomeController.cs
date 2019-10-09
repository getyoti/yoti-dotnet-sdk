using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yoti.Auth;
using Yoti.Auth.ShareUrl;
using Yoti.Auth.ShareUrl.Extensions;
using Yoti.Auth.ShareUrl.Policy;

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

        public IActionResult Index()
        {
            ViewBag.YotiClientSdKId = _clientSdkId;

            string _scenarioId = Environment.GetEnvironmentVariable("YOTI_SCENARIO_ID");
            _logger.LogInformation(string.Format("Yoti Scenario ID='{0}'", _scenarioId));
            ViewBag.YotiScenarioId = _scenarioId;

            return View();
        }

        // GET: Home/DynamicScenario
        public IActionResult DynamicScenario(string token)
        {
            try
            {
                string yotiKeyFilePath = Environment.GetEnvironmentVariable("YOTI_KEY_FILE_PATH");
                _logger.LogInformation(
                    string.Format(
                        "yotiKeyFilePath='{0}'",
                        yotiKeyFilePath));

                StreamReader privateKeyStream = System.IO.File.OpenText(yotiKeyFilePath);

                var yotiClient = new YotiClient(_clientSdkId, privateKeyStream);

                var givenNamesWantedAttribute = new WantedAttributeBuilder()
                    .WithName("given_names")
                    .Build();

                DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                   .WithWantedAttribute(givenNamesWantedAttribute)
                   .WithFullName()
                   .WithSelfie()
                   .WithPhoneNumber()
                   .WithAgeOver(18)
                   .WithRememberMeId(true)
                   .Build();

                var locationExtension = new LocationConstraintExtensionBuilder()
                    .WithLatitude(51.5044772)
                    .WithLongitude(-0.082161)
                    .WithMaxUncertainty(300)
                    .WithRadius(1500)
                    .Build();

                var dynamicScenario = new DynamicScenarioBuilder()
                    .WithCallbackEndpoint("/account/connect")
                    .WithPolicy(dynamicPolicy)
                    .WithExtension(locationExtension)
                    .Build();
                ShareUrlResult shareUrlResult = yotiClient.CreateShareUrl(dynamicScenario);

                ViewBag.YotiClientSdkId = _clientSdkId;

                return View("DynamicScenario", shareUrlResult);
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