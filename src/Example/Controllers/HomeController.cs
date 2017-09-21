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
            var user = GetUser();

            if (user.Photo == null)
                ViewBag.Message = "No photo provided, change the application settings to request a photo from the user for this demo";
            else
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