using System;
using System.Diagnostics;
using System.IO;
using CoreExample.Models;
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
        private readonly string _appId;
        private readonly string _scenarioId;
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            _appId = Environment.GetEnvironmentVariable("YOTI_APPLICATION_ID");
            _logger.LogInformation(string.Format("appId='{0}'", _appId));

            _scenarioId = Environment.GetEnvironmentVariable("SCENARIO_ID");
            _logger.LogInformation(string.Format("scenarioId='{0}'", _scenarioId));
        }

        public IActionResult Index()
        {
            ViewBag.YotiAppId = _appId;
            ViewBag.ScenarioId = _scenarioId;

            return View();
        }

        // GET: Account/DynamicScenario
        public IActionResult DynamicScenario(string token)
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

                var givenNamesWantedAttribute = new WantedAttributeBuilder()
                    .WithName("given_names")
                    .WithOptional(true)
                    .Build();

                DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                   .WithWantedAttribute(givenNamesWantedAttribute)
                   .WithWantedAttribute("nationality", optional: true)
                   .WithFullName()
                   .WithPhoneNumber(optional: true)
                   .WithAgeOver(18)
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

                ViewBag.YotiAppId = _appId;
                ViewBag.ScenarioId = _scenarioId;

                return View("DynamicScenario", shareUrlResult);
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult LoginFailure()
        {
            ViewBag.YotiAppId = _appId;
            return View();
        }
    }
}