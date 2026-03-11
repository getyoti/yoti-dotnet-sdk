using System;
using System.IO;
using System.Threading.Tasks;
using Yoti.Auth;
using Yoti.Auth.DigitalIdentity;
using Yoti.Auth.DigitalIdentity.Policy;
using Yoti.Auth.DocScan;
using Yoti.Auth.DocScan.Session.Create;

namespace Yoti.Auth.Examples
{
    /// <summary>
    /// Example demonstrating how to access HTTP response headers from Yoti API calls
    /// All existing methods now return headers - no need for separate WithHeaders methods!
    /// </summary>
    public class RequestHeaderExample
    {
        public async Task DemonstrateHeaderAccess()
        {
            // Initialize client (same as before)
            string sdkId = "your-sdk-id";
            var keyStream = new StreamReader(File.OpenRead("path-to-your-private-key.pem"));
            var client = new DigitalIdentityClient(sdkId, keyStream);

            // Create a share session request with correct method names
            var shareSessionRequest = new ShareSessionRequestBuilder()
                .WithPolicy(new PolicyBuilder()
                    .WithDateOfBirth()
                    .Build())
                .WithRedirectUri("https://your-callback-endpoint.com")
                .Build();

            try
            {
                // Existing methods now return headers automatically!
                var sessionResultWithHeaders = await client.CreateShareSessionAsync(shareSessionRequest);
                
                // Access the actual data (implicit conversion from YotiHttpResponse<T> to T)
                ShareSessionResult sessionResult = sessionResultWithHeaders;
                Console.WriteLine($"Session ID: {sessionResult.Id}");
                Console.WriteLine($"Session Status: {sessionResult.Status}");
                
                // Access headers directly
                string requestId = sessionResultWithHeaders.RequestId; // Shortcut for X-Request-ID
                Console.WriteLine($"Request ID for troubleshooting: {requestId}");
                
                // Access other headers
                string serverHeader = sessionResultWithHeaders.GetHeaderValue("Server");
                Console.WriteLine($"Server: {serverHeader}");
                
                // Get a receipt - this method also returns headers now!
                var receiptWithHeaders = await client.GetShareReceiptAsync("some-receipt-id");
                
                // Both the receipt data and headers are available
                var receipt = receiptWithHeaders.Data; // or implicit: SharedReceiptResponse receipt = receiptWithHeaders;
                var receiptHeaders = receiptWithHeaders.Headers;
                
                if (receiptWithHeaders.RequestId != null)
                {
                    Console.WriteLine($"Receipt Request ID: {receiptWithHeaders.RequestId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task DemonstrateDocScanHeaderAccess()
        {
            // DocScan example - all methods now return headers too!
            string sdkId = "your-sdk-id";
            var keyStream = new StreamReader(File.OpenRead("path-to-your-private-key.pem"));
            var docScanClient = new DocScanClient(sdkId, keyStream);
            
            try
            {
                // Create session specification
                var sessionSpec = new SessionSpecificationBuilder()
                    .WithClientSessionTokenTtl(600)
                    .Build();

                // Create session - method now returns headers automatically
                var createSessionResultWithHeaders = await docScanClient.CreateSessionAsync(sessionSpec);
                
                // Access data and headers
                var createResult = createSessionResultWithHeaders.Data;
                string createRequestId = createSessionResultWithHeaders.RequestId;
                
                Console.WriteLine($"Created Session ID: {createResult.SessionId}");
                Console.WriteLine($"Create Session Request ID: {createRequestId}");
                
                // Get session - this method also returns headers now
                var getSessionResultWithHeaders = await docScanClient.GetSessionAsync(createResult.SessionId);
                
                var sessionData = getSessionResultWithHeaders.Data;
                string getRequestId = getSessionResultWithHeaders.RequestId;
                
                Console.WriteLine($"Session State: {sessionData.State}");
                Console.WriteLine($"Get Session Request ID: {getRequestId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DocScan Error: {ex.Message}");
            }
        }

        public void DemonstrateSyncHeaderAccess()
        {
            // Synchronous methods also return headers now!
            string sdkId = "your-sdk-id";
            var keyStream = new StreamReader(File.OpenRead("path-to-your-private-key.pem"));
            var client = new DigitalIdentityClient(sdkId, keyStream);

            var shareSessionRequest = new ShareSessionRequestBuilder()
                .WithPolicy(new PolicyBuilder()
                    .WithDateOfBirth()
                    .Build())
                .WithRedirectUri("https://your-callback-endpoint.com")
                .Build();

            try
            {
                // Sync method - returns headers too
                var sessionResultWithHeaders = client.CreateShareSession(shareSessionRequest);
                
                Console.WriteLine($"Sync Session ID: {sessionResultWithHeaders.Data.Id}");
                Console.WriteLine($"Sync Session Status: {sessionResultWithHeaders.Data.Status}");
                
                if (sessionResultWithHeaders.RequestId != null)
                {
                    Console.WriteLine($"Sync Request ID: {sessionResultWithHeaders.RequestId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sync Error: {ex.Message}");
            }
        }
    }
}
