using Example.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Example.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var user = GetUser();

            ViewBag.Photo = "data:image/jpeg;base64," + Convert.ToBase64String(user.Photo);

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