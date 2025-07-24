using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using DocScanExample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yoti.Auth;
using Yoti.Auth.DocScan;
using Yoti.Auth.DocScan.Session.Create;
using Yoti.Auth.DocScan.Session.Create.Check;
using Yoti.Auth.DocScan.Session.Create.Filter;
using Yoti.Auth.DocScan.Session.Create.Objectives;
using Yoti.Auth.DocScan.Session.Create.Task;

namespace DocScanExample.Controllers
{
    /// <summary>
    /// Example controller demonstrating how to use the suppressed_screens configuration
    /// to customize the document scanning flow for shortened ID verification
    /// </summary>
    public class SuppressedScreensController : Controller
    {
        private readonly DocScanClient _client;
        private readonly string _baseUrl;
        private readonly Uri _apiUrl;

        public SuppressedScreensController(IHttpContextAccessor httpContextAccessor)
        {
            var request = httpContextAccessor.HttpContext.Request;
            _baseUrl = $"{request.Scheme}://{request.Host}";
            _apiUrl = HomeController.GetApiUrl();
            _client = HomeController.GetDocScanClient(_apiUrl);
        }

        /// <summary>
        /// Creates a DocScan session with suppressed screens for a shortened flow
        /// This example suppresses common screens that can be skipped for returning users
        /// </summary>
        public IActionResult ShortenedFlow()
        {
            NotificationConfig notificationConfig =
                new NotificationConfigBuilder()
                .ForClientSessionCompletion()
                .WithEndpoint("https://www.yoti.com/hookurl")
                .Build();

            // List of screens to suppress in the shortened flow
            var suppressedScreens = new List<string>
            {
                "welcome",              // Skip welcome/intro screen
                "privacy_policy",       // Skip privacy policy if already accepted
                "document_selection",   // Skip document type selection for returning users
                "instructions",         // Skip detailed instructions
                "tutorial"              // Skip tutorial/guidance screens
            };

            //Build Session Spec with suppressed screens
            var sessionSpec = new SessionSpecificationBuilder()
                .WithClientSessionTokenTtl(600)
                .WithResourcesTtl(96400)
                .WithUserTrackingId("shortened-flow-user-id")
                
                // Add essential checks for ID verification
                .WithRequestedCheck(
                    new RequestedDocumentAuthenticityCheckBuilder()
                    .WithManualCheckFallback()
                    .Build()
                )
                .WithRequestedCheck(
                    new RequestedLivenessCheckBuilder()
                    .ForStaticLiveness()
                    .Build()
                )
                .WithRequestedCheck(
                    new RequestedFaceMatchCheckBuilder()
                    .WithManualCheckFallback()
                    .Build()
                )
                
                // Add text extraction task
                .WithRequestedTask(
                    new RequestedTextExtractionTaskBuilder()
                    .WithManualCheckFallback()
                    .WithChipDataDesired()
                    .Build()
                )
                
                .WithNotifications(notificationConfig)
                
                // SDK Config with suppressed screens for shortened flow
                .WithSdkConfig(
                    new SdkConfigBuilder()
                    .WithAllowsCameraAndUpload()
                    .WithPrimaryColour("#2d9fff")
                    .WithSecondaryColour("#FFFFFF")
                    .WithFontColour("#FFFFFF")
                    .WithLocale("en-GB")
                    .WithPresetIssuingCountry("GBR")
                    .WithSuccessUrl($"{_baseUrl}/suppressedscreens/success")
                    .WithErrorUrl($"{_baseUrl}/suppressedscreens/error")
                    .WithPrivacyPolicyUrl($"{_baseUrl}/privacy-policy")
                    .WithAllowHandoff(false)
                    .WithSuppressedScreens(suppressedScreens)  // NEW: Suppress specified screens
                    .Build()
                )
                
                // Simplified document requirements for shortened flow
                .WithRequiredDocument(
                    new RequiredIdDocumentBuilder()
                    .WithFilter(
                        (new OrthogonalRestrictionsFilterBuilder())
                        .WithIncludedDocumentTypes(new List<string> { "PASSPORT", "DRIVING_LICENCE" })
                        .Build()
                    )
                    .Build()
                )
                .Build();

            CreateSessionResult createSessionResult = _client.CreateSession(sessionSpec);
            string sessionId = createSessionResult.SessionId;

            string path = $"web/index.html?sessionID={sessionId}&sessionToken={createSessionResult.ClientSessionToken}";
            Uri uri = new Uri(_apiUrl, path);

            ViewBag.iframeUrl = uri.ToString();
            ViewBag.flowType = "Shortened Flow";
            ViewBag.suppressedScreens = suppressedScreens;

            TempData["sessionId"] = sessionId;
            return View("Index");
        }

        /// <summary>
        /// Creates a DocScan session with a completely customized suppressed screens configuration
        /// This example shows how to create a highly tailored experience
        /// </summary>
        public IActionResult CustomFlow()
        {
            NotificationConfig notificationConfig =
                new NotificationConfigBuilder()
                .ForClientSessionCompletion()
                .WithEndpoint("https://www.yoti.com/hookurl")
                .Build();

            // Custom list of screens to suppress for advanced users
            var customSuppressedScreens = new List<string>
            {
                "welcome",
                "document_selection",
                "instructions",
                "tutorial",
                "preview_screen",
                "confirmation_screen"
            };

            var sessionSpec = new SessionSpecificationBuilder()
                .WithClientSessionTokenTtl(600)
                .WithResourcesTtl(96400)
                .WithUserTrackingId("custom-flow-user-id")
                
                .WithRequestedCheck(
                    new RequestedDocumentAuthenticityCheckBuilder()
                    .WithManualCheckFallback()
                    .Build()
                )
                .WithRequestedCheck(
                    new RequestedLivenessCheckBuilder()
                    .ForStaticLiveness()
                    .Build()
                )
                .WithRequestedCheck(
                    new RequestedFaceMatchCheckBuilder()
                    .WithManualCheckFallback()
                    .Build()
                )
                
                .WithRequestedTask(
                    new RequestedTextExtractionTaskBuilder()
                    .WithManualCheckFallback()
                    .Build()
                )
                
                .WithNotifications(notificationConfig)
                
                .WithSdkConfig(
                    new SdkConfigBuilder()
                    .WithAllowsCameraAndUpload()
                    .WithPrimaryColour("#ff6b35")  // Custom orange color
                    .WithSecondaryColour("#004d40") // Dark teal
                    .WithFontColour("#FFFFFF")
                    .WithLocale("en-US")
                    .WithPresetIssuingCountry("USA")
                    .WithSuccessUrl($"{_baseUrl}/suppressedscreens/success")
                    .WithErrorUrl($"{_baseUrl}/suppressedscreens/error")
                    .WithPrivacyPolicyUrl($"{_baseUrl}/privacy-policy")
                    .WithAllowHandoff(true)
                    .WithSuppressedScreens(customSuppressedScreens)  // Custom suppressed screens
                    .Build()
                )
                
                .WithRequiredDocument(
                    new RequiredIdDocumentBuilder()
                    .WithFilter(
                        (new OrthogonalRestrictionsFilterBuilder())
                        .WithIncludedDocumentTypes(new List<string> { "PASSPORT" })
                        .Build()
                    )
                    .Build()
                )
                .Build();

            CreateSessionResult createSessionResult = _client.CreateSession(sessionSpec);
            string sessionId = createSessionResult.SessionId;

            string path = $"web/index.html?sessionID={sessionId}&sessionToken={createSessionResult.ClientSessionToken}";
            Uri uri = new Uri(_apiUrl, path);

            ViewBag.iframeUrl = uri.ToString();
            ViewBag.flowType = "Custom Flow";
            ViewBag.suppressedScreens = customSuppressedScreens;

            TempData["sessionId"] = sessionId;
            return View("Index");
        }

        /// <summary>
        /// Example of no suppressed screens (standard flow)
        /// </summary>
        public IActionResult StandardFlow()
        {
            NotificationConfig notificationConfig =
                new NotificationConfigBuilder()
                .ForClientSessionCompletion()
                .WithEndpoint("https://www.yoti.com/hookurl")
                .Build();

            var sessionSpec = new SessionSpecificationBuilder()
                .WithClientSessionTokenTtl(600)
                .WithResourcesTtl(96400)
                .WithUserTrackingId("standard-flow-user-id")
                
                .WithRequestedCheck(
                    new RequestedDocumentAuthenticityCheckBuilder()
                    .WithManualCheckFallback()
                    .Build()
                )
                .WithRequestedCheck(
                    new RequestedLivenessCheckBuilder()
                    .ForStaticLiveness()
                    .Build()
                )
                .WithRequestedCheck(
                    new RequestedFaceMatchCheckBuilder()
                    .WithManualCheckFallback()
                    .Build()
                )
                
                .WithRequestedTask(
                    new RequestedTextExtractionTaskBuilder()
                    .WithManualCheckFallback()
                    .Build()
                )
                
                .WithNotifications(notificationConfig)
                
                .WithSdkConfig(
                    new SdkConfigBuilder()
                    .WithAllowsCameraAndUpload()
                    .WithPrimaryColour("#2d9fff")
                    .WithSecondaryColour("#FFFFFF")
                    .WithFontColour("#FFFFFF")
                    .WithLocale("en-GB")
                    .WithPresetIssuingCountry("GBR")
                    .WithSuccessUrl($"{_baseUrl}/suppressedscreens/success")
                    .WithErrorUrl($"{_baseUrl}/suppressedscreens/error")
                    .WithPrivacyPolicyUrl($"{_baseUrl}/privacy-policy")
                    .WithAllowHandoff(false)
                    // Note: No WithSuppressedScreens() call - standard flow with all screens
                    .Build()
                )
                
                .WithRequiredDocument(
                    new RequiredIdDocumentBuilder()
                    .WithFilter(
                        (new OrthogonalRestrictionsFilterBuilder())
                        .WithIncludedDocumentTypes(new List<string> { "PASSPORT", "DRIVING_LICENCE" })
                        .Build()
                    )
                    .Build()
                )
                .Build();

            CreateSessionResult createSessionResult = _client.CreateSession(sessionSpec);
            string sessionId = createSessionResult.SessionId;

            string path = $"web/index.html?sessionID={sessionId}&sessionToken={createSessionResult.ClientSessionToken}";
            Uri uri = new Uri(_apiUrl, path);

            ViewBag.iframeUrl = uri.ToString();
            ViewBag.flowType = "Standard Flow";
            ViewBag.suppressedScreens = new List<string>(); // Empty list

            TempData["sessionId"] = sessionId;
            return View("Index");
        }

        public IActionResult Success()
        {
            string sessionId = TempData["sessionId"].ToString();
            TempData.Keep("sessionId");

            var getSessionResult = _client.GetSession(sessionId);
            
            return View(getSessionResult);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
