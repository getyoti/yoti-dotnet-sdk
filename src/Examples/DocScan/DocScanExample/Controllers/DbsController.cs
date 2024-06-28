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
 
namespace DocScanExample.Controllers
{
    public class DbsController : Controller
    {
        private readonly DocScanClient _client;

        private readonly string _baseUrl;
        private readonly Uri _apiUrl;

        public DbsController(IHttpContextAccessor httpContextAccessor)
        {
            var request = httpContextAccessor.HttpContext.Request;
            
            _baseUrl = $"{request.Scheme}://{request.Host}"; ;
            _apiUrl = GetApiUrl();
            _client = GetDocScanClient(_apiUrl);
        }

        public IActionResult Index()
        {
            //Build Session Spec
            var sessionSpec = new SessionSpecificationBuilder()
                .WithClientSessionTokenTtl(600)
                .WithResourcesTtl(86400)
                .WithUserTrackingId("some-user-tracking-id")
                //Add Sdk Config (with builder)
                .WithSdkConfig(
                    new SdkConfigBuilder()
                    .WithAllowsCamera()
                    .WithPrimaryColour("#2d9fff")
                    .WithSecondaryColour("#FFFFFF")
                    .WithFontColour("#FFFFFF")
                    .WithLocale("en-GB")
                    .WithPresetIssuingCountry("GBR")
                    .WithSuccessUrl($"{_baseUrl}/idverify/success")
                    .WithErrorUrl($"{_baseUrl}/idverify/error")
                    .WithPrivacyPolicyUrl($"{_baseUrl}/privacy-policy")
                    .Build()
                    )
                .WithCreateIdentityProfilePreview(true)
                .WithIdentityProfileRequirements(new
                 {
                     trust_framework = "UK_TFIDA",
                     scheme = new
                     {
                         type = "DBS",
                         objective = "BASIC"
                     }
                 })
                .WithSubject(new
                {
                    subject_id = "some_subject_id_string"
                })    
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
