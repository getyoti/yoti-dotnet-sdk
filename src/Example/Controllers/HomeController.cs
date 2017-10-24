using System;
using System.Security.Claims;
using System.Web.Mvc;
using Example.Models;

namespace Example.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public static byte[] PhotoBytes { get; set; }

        public ActionResult Index()
        {
            var user = GetUser();

            if (user.Base64Photo == null)
                ViewBag.Message = "No photo provided, change the application settings to request a photo from the user for this demo";
            else
            {
                ViewBag.Photo = user.Base64Photo;
                PhotoBytes = user.Photo;
            }
            return View();
        }

        private User GetUser()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            int userId = int.Parse(claimsIdentity.FindFirst(ClaimTypes.Name).Value);

            return UserManager.GetUserById(userId);
        }

        public FileContentResult DownloadImageFile()
        {
            if (PhotoBytes == null)
                throw new InvalidOperationException("The 'PhotoBytes' variable has not been set");

            return File(PhotoBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "YotiSelfie.jpg");
        }
    }
}