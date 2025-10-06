using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Yoti.Auth;
using Yoti.Auth.DigitalIdentity;
using Yoti.Auth.DigitalIdentity.Policy;

namespace HeaderTestConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Yoti SDK Header Test ===\n");

            // Mock HTTP Handler to simulate response
            var mockHandler = new MockHttpMessageHandler();
            var httpClient = new HttpClient(mockHandler);

            // Create a test key pair (you'd normally load this from file)
            string testKeyPem = @"-----BEGIN RSA PRIVATE KEY-----
MIIEowIBAAKCAQEAx3dJSSlIMNKFHGLdqOqNk6fYNZ3hXxZ8WHPIp1fxqPEr3qKF
+LNLR5vqVFvNkQ8vq7y6uGPq5z3MJN4hXzHBM2nGv6W6ybJLEZZQEqSI4+qLnH5u
+H5qEq4C6v3qKwZJGq9ZXX8pKW0h8ZX6W0PqCMV7Pnz8W6yLbKL5q3hV2bE9v7RG
WPLQqOqEbXqJVbp5V8JqKQ7eXVL5XqEL3qKF+LNLR5vqVFvNkQ8vq7y6uGPq5z3M
JN4hXzHBM2nGv6W6ybJLEZZQEqSI4+qLnH5u+H5qEq4C6v3qKwZJGq9ZXX8pKW0h
8ZX6W0PqCMV7Pnz8W6yLbKL5q3hV2bE9v7RGWPLQ+QIDAQABAoIBAH4+V3qZhM4h
jZNfVL5C9FvL4kC3D7qQNXqGCJqK5rJ5L9qQNXqGCJqK5rJ5L9qQNXqGCJqK5rJ5
L9qQNXqGCJqK5rJ5L9qQNXqGCJqK5rJ5L9qQNXqGCJqK5rJ5L9qQNXqGCJqK5rJ5
L9qQNXqGCJqK5rJ5L9qQNXqGCJqK5rJ5L9qQNXqGCJqK5rJ5L9qQNXqGCJqK5rJ5
L9qQNXqGCJqK5rJ5L9qQNXqGCJqK5rJ5L9qQNXqGCJqK5rJ5L9qQNXqGCJqK5rJ5
L9qQNXqGCJqK5rJ5L9qQNXqGCJqK5rJ5L9qQNXqGCJqK5rJ5LAECgYEA5qKF+LNL
R5vqVFvNkQ8vq7y6uGPq5z3MJN4hXzHBM2nGv6W6ybJLEZZQEqSI4+qLnH5u+H5q
Eq4C6v3qKwZJGq9ZXX8pKW0h8ZX6W0PqCMV7Pnz8W6yLbKL5q3hV2bE9v7RGWPLQ
qOqEbXqJVbp5V8JqKQ7eXVL5XqEL3qKF+LNLRAECgYEA3qKF+LNLR5vqVFvNkQ8v
q7y6uGPq5z3MJN4hXzHBM2nGv6W6ybJLEZZQEqSI4+qLnH5u+H5qEq4C6v3qKwZJ
Gq9ZXX8pKW0h8ZX6W0PqCMV7Pnz8W6yLbKL5q3hV2bE9v7RGWPLQqOqEbXqJVbp5
V8JqKQ7eXVL5XqEL3qKF+LNLRQkCgYBGq9ZXX8pKW0h8ZX6W0PqCMV7Pnz8W6yLb
KL5q3hV2bE9v7RGWPLQqOqEbXqJVbp5V8JqKQ7eXVL5XqEL3qKF+LNLR5vqVFvN
kQ8vq7y6uGPq5z3MJN4hXzHBM2nGv6W6ybJLEZZQEqSI4+qLnH5u+H5qEq4C6v3q
KwZJGq9ZXX8pKW0h8ZX6W0PqCMV7PnwBAoGBAOqEbXqJVbp5V8JqKQ7eXVL5XqEL
3qKF+LNLR5vqVFvNkQ8vq7y6uGPq5z3MJN4hXzHBM2nGv6W6ybJLEZZQEqSI4+qL
nH5u+H5qEq4C6v3qKwZJGq9ZXX8pKW0h8ZX6W0PqCMV7Pnz8W6yLbKL5q3hV2bE9
v7RGWPLQqOqEbXqJVbp5V8JqKQ7eXVL5XqECgYBKL5q3hV2bE9v7RGWPLQqOqEbX
qJVbp5V8JqKQ7eXVL5XqEL3qKF+LNLR5vqVFvNkQ8vq7y6uGPq5z3MJN4hXzHBM2
nGv6W6ybJLEZZQEqSI4+qLnH5u+H5qEq4C6v3qKwZJGq9ZXX8pKW0h8ZX6W0PqCM
V7Pnz8W6yLbKL5q3hV2bE9v7RGWPLQqOqA==
-----END RSA PRIVATE KEY-----";

            try
            {
                // Create a DigitalIdentityClient
                Console.WriteLine("Creating DigitalIdentityClient...");
                var stringReader = new StringReader(testKeyPem);
                var streamReader = new StreamReader(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(testKeyPem)));
                
                var client = new DigitalIdentityClient(
                    httpClient,
                    "test-sdk-id",
                    streamReader
                );

                // Create a simple share session request
                Console.WriteLine("Creating ShareSessionRequest...");
                var policy = new PolicyBuilder()
                    .WithFullName()
                    .WithEmail()
                    .Build();

                var sessionRequest = new ShareSessionRequestBuilder()
                    .WithPolicy(policy)
                    .WithRedirectUri("https://example.com/callback")
                    .Build();

                // Call CreateShareSession
                Console.WriteLine("Calling CreateShareSession...\n");
                var result = client.CreateShareSession(sessionRequest);

                // Display headers
                Console.WriteLine("=== RESPONSE HEADERS ===");
                foreach (var header in result.Headers)
                {
                    Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }
                
                Console.WriteLine($"\n=== SPECIAL HEADERS ===");
                Console.WriteLine($"X-Request-ID (via RequestId property): {result.RequestId}");
                Console.WriteLine($"X-Request-ID (via GetHeaderValue): {result.GetHeaderValue("X-Request-ID")}");
                Console.WriteLine($"Content-Type: {result.GetHeaderValue("Content-Type")}");
                
                Console.WriteLine($"\n=== RESPONSE DATA ===");
                Console.WriteLine($"Session ID: {result.Data.Id}");
                Console.WriteLine($"Status: {result.Data.Status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                Console.WriteLine($"Stack: {ex.StackTrace}");
            }

            Console.WriteLine("\n=== Test Complete ===");
        }
    }

    // Mock HTTP handler for testing
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            Console.WriteLine($"\n--- HTTP Request ---");
            Console.WriteLine($"Method: {request.Method}");
            Console.WriteLine($"URI: {request.RequestUri}");
            Console.WriteLine($"Request Headers:");
            foreach (var header in request.Headers)
            {
                Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
            }
            Console.WriteLine($"--- End Request ---\n");

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            
            // Add various headers
            response.Headers.Add("X-Request-ID", "mock-request-id-12345");
            response.Headers.Add("X-Yoti-Request-Trace", "trace-value-abc");
            response.Headers.Add("X-Custom-Header", "custom-value");
            response.Headers.Add("Date", DateTime.UtcNow.ToString("R"));
            
            // Add content with content headers
            var jsonContent = @"{
                ""id"": ""mock-session-123"",
                ""status"": ""CREATED"",
                ""qr_code"": ""https://example.com/qr""
            }";
            
            response.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            response.Content.Headers.Add("X-Content-Custom", "content-header-value");
            
            return Task.FromResult(response);
        }
    }
}
