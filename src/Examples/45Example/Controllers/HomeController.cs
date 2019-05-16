using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using Yoti.Auth;
using Yoti.Auth.ShareUrl;
using Yoti.Auth.ShareUrl.Extensions;
using Yoti.Auth.ShareUrl.Policy;

namespace Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _appId;
        private readonly string _scenarioId;

        public ActionResult Index()
        {
            ViewBag.YotiAppId = _appId;
            ViewBag.ScenarioId = _scenarioId;
            return View();
        }

        public HomeController()
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            _appId = ConfigurationManager.AppSettings["YOTI_APPLICATION_ID"];
            logger.Info(string.Format("appId='{0}'", _appId));

            _scenarioId = ConfigurationManager.AppSettings["SCENARIO_ID"];
            logger.Info(string.Format("scenarioId='{0}'", _scenarioId));
        }

        // GET: Home/DynamicScenario
        public ActionResult DynamicScenario(string token)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                string sdkId = ConfigurationManager.AppSettings["YOTI_CLIENT_SDK_ID"];
                logger.Info(string.Format("sdkId='{0}'", sdkId));

                string yotiKeyFilePath = ConfigurationManager.AppSettings["YOTI_KEY_FILE_PATH"];
                logger.Info(
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
                   .WithSelfie(optional: true)
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
                logger.Error(
                    e,
                    "An error occurred");
                ViewBag.Error = e.Message;

                return RedirectToAction("LoginFailure", "Home");
            }
        }
    }
}