using System;
using System.Diagnostics;
using CoreExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreExample.Controllers
{
    public class HomeController : Controller
    {
        private string _appId;
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string scenarioId = Environment.GetEnvironmentVariable("SCENARIO_ID");
            _logger.LogInformation(string.Format("scenarioID='{0}'", scenarioId));
            ViewBag.ScenarioId = scenarioId;

            _appId = Environment.GetEnvironmentVariable("YOTI_APPLICATION_ID");
            _logger.LogInformation(string.Format("appId='{0}'", _appId));
            ViewBag.YotiAppId = _appId;
            return View();
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