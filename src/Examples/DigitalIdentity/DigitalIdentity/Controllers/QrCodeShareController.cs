using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yoti.Auth;
using Yoti.Auth.DigitalIdentity;
using Yoti.Auth.DigitalIdentity.Policy;

namespace DigitalIdentityExample.Controllers
{
    public class QrCodeShareController : Controller
    {
        private readonly string _clientSdkId;
        private readonly ILogger _logger;

        public QrCodeShareController(ILogger<QrCodeShareController> logger)
        {
            _logger = logger;

            _clientSdkId = Environment.GetEnvironmentVariable("YOTI_CLIENT_SDK_ID");
            _logger.LogInformation(string.Format("Yoti Client SDK ID='{0}'", _clientSdkId));
        }

        private DigitalIdentityClient CreateYotiClient()
        {
            string yotiKeyFilePath = Environment.GetEnvironmentVariable("YOTI_KEY_FILE_PATH");
            _logger.LogInformation(string.Format("yotiKeyFilePath='{0}'", yotiKeyFilePath));
            StreamReader privateKeyStream = System.IO.File.OpenText(yotiKeyFilePath);
            return new DigitalIdentityClient(_clientSdkId, privateKeyStream);
        }

        // GET: /qr-share — renders the interactive page
        [Route("qr-share")]
        public IActionResult QrShare()
        {
            ViewBag.YotiClientSdkId = _clientSdkId;
            return View("QrShare");
        }

        // POST: /qr-share/create-session — Step 1: Create a share session
        [HttpPost]
        [Route("qr-share/create-session")]
        public async Task<IActionResult> CreateSession()
        {
            try
            {
                var yotiClient = CreateYotiClient();

                var givenNamesWantedAttribute = new WantedAttributeBuilder()
                    .WithName("given_names")
                    .WithOptional(false)
                    .Build();

                var policy = new PolicyBuilder()
                    .WithWantedAttribute(givenNamesWantedAttribute)
                    .WithFullName()
                    .WithEmail()
                    .WithPhoneNumber()
                    .WithSelfie()
                    .WithAgeOver(18)
                    .WithNationality()
                    .WithGender()
                    .WithDocumentDetails()
                    .WithDocumentImages()
                    .Build();

                var sessionReq = new ShareSessionRequestBuilder()
                    .WithPolicy(policy)
                    .WithRedirectUri("https://www.yoti.com")
                    .Build();

                var sessionResult = await yotiClient.CreateShareSessionAsync(sessionReq);
                _logger.LogInformation(string.Format(
                    "Session created: ID='{0}', Status='{1}', Expiry='{2}'",
                    sessionResult.Id, sessionResult.Status, sessionResult.Expiry));

                return Json(new
                {
                    sessionId = sessionResult.Id,
                    status = sessionResult.Status,
                    expiry = sessionResult.Expiry
                });
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                return Json(new { error = e.Message, inner = e.InnerException?.Message });
            }
        }

        // POST: /qr-share/create-qr — Step 2: Create QR code for a session
        [HttpPost]
        [Route("qr-share/create-qr")]
        public async Task<IActionResult> CreateQr([FromQuery] string sessionId)
        {
            try
            {
                var yotiClient = CreateYotiClient();

                var qrRequest = new QrRequestBuilder().Build();
                var qrResult = await yotiClient.CreateQrCode(sessionId, qrRequest);
                _logger.LogInformation(string.Format("QR created: ID='{0}', URI='{1}'", qrResult.Id, qrResult.Uri));

                return Json(new
                {
                    qrId = qrResult.Id,
                    qrUri = qrResult.Uri
                });
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                return Json(new { error = e.Message, inner = e.InnerException?.Message });
            }
        }

        // GET: /qr-share/get-session — Step 3: Get session status (poll)
        [HttpGet]
        [Route("qr-share/get-session")]
        public async Task<IActionResult> GetSessionStatus([FromQuery] string sessionId)
        {
            try
            {
                var yotiClient = CreateYotiClient();
                var session = await yotiClient.GetSession(sessionId);

                return Json(new
                {
                    sessionId = session.Id,
                    status = session.Status,
                    expiry = session.Expiry,
                    created = session.Created,
                    updated = session.Updated,
                    qrCodeId = session.QrCode?.Id,
                    receiptId = session.Receipt?.Id
                });
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                return Json(new { error = e.Message, inner = e.InnerException?.Message });
            }
        }

        // GET: /qr-share/get-receipt — Step 4: Get decrypted receipt
        [HttpGet]
        [Route("qr-share/get-receipt")]
        public IActionResult GetReceipt([FromQuery] string receiptId)
        {
            try
            {
                var yotiClient = CreateYotiClient();
                var receipt = yotiClient.GetShareReceipt(receiptId);

                return Json(new
                {
                    id = receipt.ID,
                    sessionId = receipt.SessionID,
                    rememberMeId = receipt.RememberMeID,
                    timestamp = receipt.Timestamp,
                    hasUserProfile = receipt.UserContent?.UserProfile != null,
                    hasApplicationProfile = receipt.ApplicationContent?.ApplicationProfile != null,
                    error = receipt.Error
                });
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: e.Message);
                return Json(new { error = e.Message, inner = e.InnerException?.Message });
            }
        }
    }
}
