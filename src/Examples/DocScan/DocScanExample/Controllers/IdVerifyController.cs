using System.Diagnostics;
using DocScanExample.Models;
using Microsoft.AspNetCore.Mvc;
using Yoti.Auth.DocScan;
using Yoti.Auth.DocScan.Session.Retrieve;

namespace DocScanExample.Controllers
{
    public class IdVerifyController : Controller
    {
        private readonly DocScanClient _client;

        public IdVerifyController()
        {
            _client = HomeController.GetDocScanClient();
        }

        public IActionResult Updates()
        {
            return View();
        }

        public IActionResult Success()
        {
            string sessionId = TempData["sessionId"].ToString();
            TempData.Keep("sessionId");
            //sessionId = "f5f4c67a-af57-4501-89ab-428d0aaa1b55";
            GetSessionResult getSessionResult = _client.GetSession(sessionId);
            
            return View(getSessionResult);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
