using System;
using System.IO;
using DigitalIdentity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yoti.Auth;
using Yoti.Auth.Attribute;
using Yoti.Auth.Document;
using Yoti.Auth.Images;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;

namespace DigitalIdentityExample.Controllers
{
    public class SuccessController : Controller
    {
        private readonly string _clientSdkId;
        private readonly ILogger _logger;
        public SuccessController(ILogger<SuccessController> logger)
        {
            _logger = logger;

            _clientSdkId = Environment.GetEnvironmentVariable("YOTI_CLIENT_SDK_ID");
            _logger.LogInformation(string.Format("Yoti Client SDK ID='{0}'", _clientSdkId));
        }
        public ActionResult Error()
        {
            return View();
        }
        [Route("receipt-info")]
        // GET: receipt-info?ReceiptID
        public IActionResult ReceiptInfo(string ReceiptID)
        {
            try
            {
                string yotiKeyFilePath = Environment.GetEnvironmentVariable("YOTI_KEY_FILE_PATH");
                _logger.LogInformation(
                    string.Format(
                        "yotiKeyFilePath='{0}'",
                        yotiKeyFilePath));

                StreamReader privateKeyStream = System.IO.File.OpenText(yotiKeyFilePath);

                var yotiClient = new DigitalIdentityClient(_clientSdkId, privateKeyStream);
                
                var ReceiptResult = yotiClient.GetShareReceipt(ReceiptID);
                
                DisplayAttributes displayAttributes = CreateDisplayAttributes(ReceiptResult.Data.UserContent.UserProfile.AttributeCollection);
                if (ReceiptResult.Data.UserContent.UserProfile.FullName != null)
                {
                    displayAttributes.FullName = ReceiptResult.Data.UserContent.UserProfile.FullName.GetValue();
                }

                YotiAttribute<Image> selfie = ReceiptResult.Data.UserContent.UserProfile.Selfie;
                if (ReceiptResult.Data.UserContent.UserProfile.Selfie != null)
                {
                    displayAttributes.Base64Selfie = selfie.GetValue().GetBase64URI();
                }

                displayAttributes.ErrorDetails = ReceiptResult.Data.ErrorDetails;
                
                ViewBag.YotiClientSdkId = _clientSdkId;
                
                return View("SuccessResult", displayAttributes);
            }
            catch (Exception e)
            {
                _logger.LogError(
                     exception: e,
                     message: e.Message);

                TempData["Error"] = e.Message;
                TempData["InnerException"] = e.InnerException?.Message;
                return RedirectToAction("Error", "Success");
            }
        }
        
        private static DisplayAttributes CreateDisplayAttributes(ReadOnlyCollection<BaseAttribute> attributes)
        {
            var displayAttributes = new DisplayAttributes();

            foreach (var yotiAttribute in attributes)
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
                        AddDisplayAttribute<string>("Postal Address", "yoti-icon-address", yotiAttribute, displayAttributes);
                        break;

                    case Yoti.Auth.Constants.UserProfile.StructuredPostalAddressAttribute:
                        AddDisplayAttribute<Dictionary<string, JToken>>("Structured Postal Address", "yoti-icon-address", yotiAttribute, displayAttributes);
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

                    case Yoti.Auth.Constants.UserProfile.DocumentDetailsAttribute:
                        AddDisplayAttribute<DocumentDetails>("Document Details", "yoti-icon-profile", yotiAttribute, displayAttributes);
                        break;

                    case Yoti.Auth.Constants.UserProfile.DocumentImagesAttribute:
                        AddDisplayAttribute<List<Image>>("Document Images", "yoti-icon-profile", yotiAttribute, displayAttributes);
                        break;

                    case Yoti.Auth.Constants.UserProfile.IdentityProfileReportAttribute:
                        AddDisplayAttribute<Dictionary<string, JToken>>("Identity Profile Report", "yoti-icon-profile", yotiAttribute, displayAttributes);
                        break;

                    default:
                        if (yotiAttribute is YotiAttribute<string> stringAttribute)
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
    } 
}
