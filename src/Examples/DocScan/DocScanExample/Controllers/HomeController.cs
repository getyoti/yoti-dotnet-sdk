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
    public class HomeController : Controller
    {
        private readonly DocScanClient _client;

        private readonly string _baseUrl;
        private readonly Uri _apiUrl;

        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            var request = httpContextAccessor.HttpContext.Request;
            
            _baseUrl = $"{request.Scheme}://{request.Host}"; ;
            _apiUrl = GetApiUrl();
            _client = GetDocScanClient(_apiUrl);
        }

        public IActionResult Index()
        {
            NotificationConfig notificationConfig =
            new NotificationConfigBuilder()
            .ForClientSessionCompletion()
            .WithEndpoint("https://www.yoti.com/hookurl")
            .Build();

            //Build Session Spec
            var sessionSpec = new SessionSpecificationBuilder()
                .WithClientSessionTokenTtl(600)
                .WithResourcesTtl(96400)
                .WithUserTrackingId("some-user-tracking-id")
                //Add Checks (using builders)
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
                //.WithRequestedCheck(
                //    new RequestedFaceComparisonCheckBuilder()
                //    .WithManualCheckNever()
                //    .Build()
                // )
                .WithRequestedCheck(
                    new RequestedFaceMatchCheckBuilder()
                    .WithManualCheckFallback()
                    .Build()
                )
                .WithRequestedCheck(
                    new RequestedIdDocumentComparisonCheckBuilder()
                    .Build())
                .WithRequestedCheck(
                    new RequestedThirdPartyIdentityCheckBuilder()
                    .Build())
                .WithRequestedCheck(
                    new RequestedWatchlistScreeningCheckBuilder()
                    .ForAdverseMedia()
                    .ForSanctions()
                    .Build()
                )
                //Add Tasks (using builders)
                .WithRequestedTask(
                    new RequestedTextExtractionTaskBuilder()
                    .WithManualCheckFallback()
                    .WithChipDataDesired()
                    .Build()
                )
                .WithRequestedTask(
                    new RequestedSupplementaryDocTextExtractionTaskBuilder()
                    .WithManualCheckFallback()
                    .Build()
                )
                .WithNotifications(notificationConfig)
                //Add Sdk Config (with builder)
                .WithSdkConfig(
                    new SdkConfigBuilder()
                    .WithAllowsCameraAndUpload()
                    .WithPrimaryColour("#2d9fff")
                    .WithSecondaryColour("#FFFFFF")
                    .WithFontColour("#FFFFFF")
                    .WithLocale("en-GB")
                    .WithPresetIssuingCountry("GBR")
                    .WithSuccessUrl($"{_baseUrl}/idverify/success")
                    .WithErrorUrl($"{_baseUrl}/idverify/error")
                    .WithPrivacyPolicyUrl($"{_baseUrl}/privacy-policy")
                    .WithAllowHandoff(false)
                    .Build()
                    )
                //Add Required Documents (with builders)
                .WithRequiredDocument(
                    new RequiredIdDocumentBuilder()
                    .WithFilter(
                        (new OrthogonalRestrictionsFilterBuilder())
                        .WithIncludedDocumentTypes(new List<string> { "PASSPORT" })
                        .Build()
                    )
                    .Build()
                )
                .WithRequiredDocument(
                    new RequiredIdDocumentBuilder()
                    .WithFilter(
                        (new OrthogonalRestrictionsFilterBuilder())
                        .WithIncludedDocumentTypes(new List<string> { "DRIVING_LICENCE" })
                        .Build()
                    )
                    .Build()
                )
                .WithRequiredDocument(
                    new RequiredSupplementaryDocumentBuilder()  
                    .WithObjective(
                        new ProofOfAddressObjectiveBuilder().Build())
                    .Build()
                )          
            .Build();

            CreateSessionResult createSessionResult = _client.CreateSession(sessionSpec);
            string sessionId = createSessionResult.SessionId;
           
            string path = $"web/index.html?sessionID={sessionId}&sessionToken={createSessionResult.ClientSessionToken}";
            Uri uri = new Uri(_apiUrl, path);

            ViewBag.iframeUrl = uri.ToString();

            TempData["sessionId"] = sessionId;
            return View();
        }

        public IActionResult Media(string mediaId, string sessionId)
        {
            MediaValue media = _client.GetMediaContent(sessionId, mediaId);

            return File(media.GetContent(), media.GetMIMEType());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        internal static DocScanClient GetDocScanClient(Uri apiUrl = null)
        {
            if (apiUrl == null)
                apiUrl = GetApiUrl();

            StreamReader privateKeyStream = System.IO.File.OpenText(Environment.GetEnvironmentVariable("YOTI_KEY_FILE_PATH"));
            var key = CryptoEngine.LoadRsaKey(privateKeyStream);

            string clientSdkId = Environment.GetEnvironmentVariable("YOTI_CLIENT_SDK_ID");

            return new DocScanClient(clientSdkId, key, new HttpClient(), apiUrl);
        }

        internal static Uri GetApiUrl()
        {
            string apiUrl = Environment.GetEnvironmentVariable("YOTI_DOC_SCAN_API_URL");
            if (string.IsNullOrEmpty(apiUrl))
            {
                return Yoti.Auth.Constants.Api.DefaultYotiDocsUrl;
            }

            if (!apiUrl.EndsWith("/", StringComparison.Ordinal))
                apiUrl += "/";

            return new Uri(apiUrl);
        }

        public IActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}
