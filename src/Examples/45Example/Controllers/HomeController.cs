using System.Configuration;
using System.Security.Claims;
using System.Web.Mvc;
using Example.Models;

namespace Example.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly string _appId = ConfigurationManager.AppSettings["YOTI_APPLICATION_ID"];

        public ActionResult Index()
        {
            ViewBag.YotiAppId = _appId;
            return View();
        }

        public ActionResult LoginFailure()
        {
            ViewBag.YotiAppId = _appId;
            return View();
        }
    }
}