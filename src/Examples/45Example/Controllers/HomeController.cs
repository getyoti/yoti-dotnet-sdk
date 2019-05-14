using System.Configuration;
using System.Web.Mvc;

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
    }
}