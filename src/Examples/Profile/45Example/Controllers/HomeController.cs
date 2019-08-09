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
        private readonly string _yotiClientSdkId;

        public HomeController()
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            _yotiClientSdkId = ConfigurationManager.AppSettings["YOTI_CLIENT_SDK_ID"];
            logger.Info(string.Format("Yoti Client SDK ID='{0}'", _yotiClientSdkId));
            ViewBag.YotiClientSdkId = _yotiClientSdkId;
        }

        public ActionResult Index()
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            string scenarioId = ConfigurationManager.AppSettings["YOTI_SCENARIO_ID"];
            logger.Info(string.Format("Yoti Scenario ID='{0}'", scenarioId));
            ViewBag.YotiScenarioId = scenarioId;

            return View();
        }

        // GET: Home/DynamicScenario
        public ActionResult DynamicScenario(string token)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                string yotiKeyFilePath = ConfigurationManager.AppSettings["YOTI_KEY_FILE_PATH"];
                logger.Info(
                    string.Format(
                        "yotiKeyFilePath='{0}'",
                        yotiKeyFilePath));

                StreamReader privateKeyStream = System.IO.File.OpenText(yotiKeyFilePath);

                var yotiClient = new YotiClient(_yotiClientSdkId, privateKeyStream);

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

                return View("DynamicScenario", shareUrlResult);
            }
            catch (Exception e)
            {
                logger.Error(e);
                TempData["Error"] = e.Message;
                TempData["InnerException"] = e.InnerException?.Message;

                return RedirectToAction("Error", "Account");
            }
        }
    }
}