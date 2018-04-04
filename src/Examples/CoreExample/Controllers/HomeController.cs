using System;
using System.Diagnostics;
using CoreExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreExample.Controllers
{
    public class HomeController : Controller
    {
        private string _appId = Environment.GetEnvironmentVariable("YOTI_APPLICATION_ID");

        public IActionResult Index()
        {
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