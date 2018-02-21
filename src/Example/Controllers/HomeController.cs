using System;
using System.Security.Claims;
using System.Web.Mvc;
using Example.Models;

namespace Example.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private User GetUser()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            int userId = int.Parse(claimsIdentity.FindFirst(ClaimTypes.Name).Value);

            return UserManager.GetUserById(userId);
        }
    }
}