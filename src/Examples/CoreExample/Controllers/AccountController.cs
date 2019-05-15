using System;
using System.Collections.Generic;
using System.IO;
using CoreExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yoti.Auth;
using Yoti.Auth.Images;

namespace CoreExample.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _appId = Environment.GetEnvironmentVariable("YOTI_APPLICATION_ID");
        private readonly ILogger _logger;
        private byte[] _photoBytes;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        public byte[] GetPhotoBytes()
        {
            return _photoBytes;
        }

        public void SetPhotoBytes(byte[] value)
        {
            _photoBytes = value;
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Account/Connect?token
        public ActionResult Connect(string token)
        {
            if (token == null)
                return RedirectToAction("Index", "Home");

            try
            {
                string sdkId = Environment.GetEnvironmentVariable("YOTI_CLIENT_SDK_ID");
                _logger.LogInformation(string.Format("sdkId='{0}'", sdkId));

                string yotiKeyFilePath = Environment.GetEnvironmentVariable("YOTI_KEY_FILE_PATH");
                _logger.LogInformation(
                    string.Format(
                        "yotiKeyFilePath='{0}'",
                        yotiKeyFilePath));

                StreamReader privateKeyStream = System.IO.File.OpenText(yotiKeyFilePath);

                var yotiClient = new YotiClient(sdkId, privateKeyStream);

                ActivityDetails activityDetails = yotiClient.GetActivityDetails(token);

                var profile = activityDetails.Profile;

                ViewBag.RememberMeID = activityDetails.RememberMeId;

                DisplayAttributes displayAttributes = CreateDisplayAttributes(profile.Attributes);

                if (profile.FullName != null)
                {
                    displayAttributes.FullName = profile.FullName.GetValue();
                }

                YotiAttribute<Image> selfie = profile.Selfie;
                if (profile.Selfie != null)
                {
                    Image selfieValue = selfie.GetValue();
                    SetPhotoBytes(selfieValue.GetContent());
                    DownloadImageFile();
                    displayAttributes.Base64Selfie = selfieValue.GetBase64URI();
                }

                return View(displayAttributes);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                TempData["Error"] = e.Message;
                TempData["InnerException"] = e.InnerException?.Message;
                return RedirectToAction("Error");
            }
        }

        private static DisplayAttributes CreateDisplayAttributes(Dictionary<string, BaseAttribute> attributes)
        {
            var displayAttributes = new DisplayAttributes();

            foreach (var yotiAttribute in attributes.Values)
            {
                switch (yotiAttribute.GetName())
                {
                    case Yoti.Auth.Constants.UserProfile.FullNameAttribute:
                        // Do nothing - we are displaying this already
                        break;

                    case Yoti.Auth.Constants.UserProfile.GivenNamesAttribute:
                        AddDisplayAttribute<string>("Given name", "yoti-icon-profile", yotiAttribute, displayAttributes);
                        break;

                    case Yoti.Auth.Constants.UserProfile.FamilyNameAttribute:
                        AddDisplayAttribute<string>("Family name", "yoti-icon-profile", yotiAttribute, displayAttributes);
                        break;

                    case Yoti.Auth.Constants.UserProfile.NationalityAttribute:
                        AddDisplayAttribute<string>("Nationality", "yoti-icon-nationality", yotiAttribute, displayAttributes);
                        break;

                    case Yoti.Auth.Constants.UserProfile.PostalAddressAttribute:
                        AddDisplayAttribute<string>("Address", "yoti-icon-address", yotiAttribute, displayAttributes);
                        break;

                    case Yoti.Auth.Constants.UserProfile.StructuredPostalAddressAttribute:
                        // Do nothing - we are handling this with the postalAddress attribute
                        break;

                    case Yoti.Auth.Constants.UserProfile.PhoneNumberAttribute:
                        AddDisplayAttribute<string>("Mobile number", "yoti-icon-phone", yotiAttribute, displayAttributes);
                        break;

                    case Yoti.Auth.Constants.UserProfile.EmailAddressAttribute:
                        AddDisplayAttribute<string>("Email address", "yoti-icon-email", yotiAttribute, displayAttributes);
                        break;

                    case Yoti.Auth.Constants.UserProfile.DateOfBirthAttribute:
                        AddDisplayAttribute<DateTime>("Date of birth", "yoti-icon-calendar", yotiAttribute, displayAttributes);
                        break;

                    case Yoti.Auth.Constants.UserProfile.SelfieAttribute:
                        // Do nothing - we already display the selfie
                        break;

                    case Yoti.Auth.Constants.UserProfile.GenderAttribute:
                        AddDisplayAttribute<string>("Gender", "yoti-icon-gender", yotiAttribute, displayAttributes);
                        break;

                    default:
                        YotiAttribute<string> stringAttribute = yotiAttribute as YotiAttribute<string>;

                        if (stringAttribute != null)
                        {
                            if (stringAttribute.GetName().Contains(":"))
                            {
                                displayAttributes.Add(new DisplayAttribute("Age Verification/", "Age verified", "yoti-icon-verified", stringAttribute.GetAnchors(), stringAttribute.GetValue()));
                                break;
                            }

                            AddDisplayAttribute<string>(stringAttribute.GetName(), "yoti-icon-profile", yotiAttribute, displayAttributes);
                        }
                        break;
                }
            }

            return displayAttributes;
        }

        private static void AddDisplayAttribute<T>(string name, string icon, BaseAttribute baseAttribute, DisplayAttributes displayAttributes)
        {
            if (baseAttribute is YotiAttribute<T> yotiAttribute)
                displayAttributes.Add(name, icon, yotiAttribute.GetAnchors(), yotiAttribute.GetValue());
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }

        public FileContentResult DownloadImageFile()
        {
            if (GetPhotoBytes() == null)
                throw new InvalidOperationException("The 'PhotoBytes' variable has not been set");

            return File(GetPhotoBytes(), System.Net.Mime.MediaTypeNames.Application.Octet, "YotiSelfie.jpg");
        }
    }
}