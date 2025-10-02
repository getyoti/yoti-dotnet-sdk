using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Yoti.Auth.Tests.DigitalIdentity
{
    [TestClass]
    public class HomeControllerCreateQrCodeTests
    {
        private Mock<ILogger<DigitalIdentityExample.Controllers.HomeController>> _mockLogger;
        private DigitalIdentityExample.Controllers.HomeController _homeController;
        private const string ValidSessionId = "ss.v2.abc123def456ghi789";
        private const string InvalidSessionId = "0";
        private const string ClientSdkId = "test-sdk-id";

        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<DigitalIdentityExample.Controllers.HomeController>>();
            
            // Set up environment variables for the controller
            Environment.SetEnvironmentVariable("YOTI_CLIENT_SDK_ID", ClientSdkId);
            Environment.SetEnvironmentVariable("YOTI_KEY_FILE_PATH", GetTestKeyFilePath());
            
            _homeController = new DigitalIdentityExample.Controllers.HomeController(_mockLogger.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Clean up environment variables
            Environment.SetEnvironmentVariable("YOTI_CLIENT_SDK_ID", null);
            Environment.SetEnvironmentVariable("YOTI_KEY_FILE_PATH", null);
        }

        [TestMethod]
        public async Task CreateQrCode_WithValidSessionId_AttemptsApiCall()
        {
            // Act
            var result = await _homeController.CreateQrCode(ValidSessionId);

            // Assert
            // Since we're using test credentials, the API call will fail with an authentication error
            // But this verifies that validation passed and the method attempted the API call
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            
            // Verify the error response structure (should be API error, not validation error)
            dynamic response = badRequestResult.Value;
            Assert.AreEqual(ValidSessionId, response.GetType().GetProperty("sessionId").GetValue(response));
            Assert.IsFalse((bool)response.GetType().GetProperty("success").GetValue(response));
            
            var error = response.GetType().GetProperty("error").GetValue(response)?.ToString();
            // Should NOT be validation errors since session ID format is correct
            Assert.IsFalse(error != null && error.Contains("Invalid session ID format"));
            Assert.IsFalse(error != null && error.Contains("Session ID is required"));
        }

        [TestMethod]
        public async Task CreateQrCode_WithNullSessionId_ReturnsBadRequest()
        {
            // Act
            var result = await _homeController.CreateQrCode(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            
            // Verify the error response
            dynamic response = badRequestResult.Value;
            Assert.IsFalse((bool)response.GetType().GetProperty("success").GetValue(response));
            Assert.AreEqual("Session ID is required", response.GetType().GetProperty("error").GetValue(response));
        }

        [TestMethod]
        public async Task CreateQrCode_WithEmptySessionId_ReturnsBadRequest()
        {
            // Act
            var result = await _homeController.CreateQrCode("");

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            
            // Verify the error response
            dynamic response = badRequestResult.Value;
            Assert.IsFalse((bool)response.GetType().GetProperty("success").GetValue(response));
            Assert.AreEqual("Session ID is required", response.GetType().GetProperty("error").GetValue(response));
        }

        [TestMethod]
        public async Task CreateQrCode_WithWhitespaceSessionId_ReturnsBadRequest()
        {
            // Act
            var result = await _homeController.CreateQrCode("   ");

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            
            // Verify the error response
            dynamic response = badRequestResult.Value;
            Assert.IsFalse((bool)response.GetType().GetProperty("success").GetValue(response));
            Assert.AreEqual("Session ID is required", response.GetType().GetProperty("error").GetValue(response));
        }

        [TestMethod]
        public async Task CreateQrCode_WithInvalidSessionIdFormat_ReturnsBadRequest()
        {
            // Act
            var result = await _homeController.CreateQrCode(InvalidSessionId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            
            // Verify the error response
            dynamic response = badRequestResult.Value;
            Assert.AreEqual(InvalidSessionId, response.GetType().GetProperty("sessionId").GetValue(response));
            Assert.IsFalse((bool)response.GetType().GetProperty("success").GetValue(response));
            Assert.AreEqual("Invalid session ID format", response.GetType().GetProperty("error").GetValue(response));
            Assert.IsTrue(response.GetType().GetProperty("message").GetValue(response).ToString().Contains("ss.v2."));
            Assert.AreEqual("ss.v2.xxxxx...", response.GetType().GetProperty("expectedFormat").GetValue(response));
        }

        [TestMethod]
        [DataRow("abc123")]
        [DataRow("session123")]
        [DataRow("ss.v1.abc123")]
        [DataRow("invalid-format")]
        public async Task CreateQrCode_WithVariousInvalidFormats_ReturnsBadRequest(string invalidSessionId)
        {
            // Act
            var result = await _homeController.CreateQrCode(invalidSessionId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            
            // Verify the error response
            dynamic response = badRequestResult.Value;
            Assert.AreEqual(invalidSessionId, response.GetType().GetProperty("sessionId").GetValue(response));
            Assert.IsFalse((bool)response.GetType().GetProperty("success").GetValue(response));
            Assert.AreEqual("Invalid session ID format", response.GetType().GetProperty("error").GetValue(response));
        }

        [TestMethod]
        [DataRow("ss.v2.abc123")]
        [DataRow("ss.v2.1234567890abcdef")]
        [DataRow("ss.v2.very-long-session-id-with-multiple-parts")]
        public async Task CreateQrCode_WithValidSessionIdFormats_AttemptsApiCall(string validSessionId)
        {
            // Note: This test will attempt actual API call and may fail with authentication errors,
            // but it verifies that the validation passes and the method proceeds to the API call
            
            // Act
            var result = await _homeController.CreateQrCode(validSessionId);

            // Assert
            // The result should be either OkObjectResult (if API call succeeds) 
            // or BadRequestObjectResult (if API call fails due to auth/network issues)
            // But it should NOT be validation error about session ID format
            Assert.IsTrue(result is OkObjectResult || result is BadRequestObjectResult);
            
            if (result is BadRequestObjectResult badRequest)
            {
                dynamic response = badRequest.Value;
                var error = response.GetType().GetProperty("error").GetValue(response)?.ToString();
                
                // Should not be a validation error about session ID format
                Assert.IsFalse(error != null && error.Contains("Invalid session ID format"));
                Assert.IsFalse(error != null && error.Contains("Session ID is required"));
            }
        }

        [TestMethod]
        public async Task CreateQrCode_WithMissingEnvironmentVariables_HandlesGracefully()
        {
            // Arrange - Remove environment variables
            Environment.SetEnvironmentVariable("YOTI_KEY_FILE_PATH", null);
            
            // Act
            var result = await _homeController.CreateQrCode(ValidSessionId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            
            // Verify it's an error related to missing configuration, not validation
            dynamic response = badRequestResult.Value;
            Assert.IsFalse((bool)response.GetType().GetProperty("success").GetValue(response));
            Assert.IsNotNull(response.GetType().GetProperty("error").GetValue(response));
        }

        [TestMethod]
        public void CreateQrCode_MethodHasCorrectAttributes()
        {
            // Get the method info
            var methodInfo = typeof(DigitalIdentityExample.Controllers.HomeController)
                .GetMethod("CreateQrCode");
            
            // Verify method exists
            Assert.IsNotNull(methodInfo);
            
            // Verify it's async
            Assert.IsTrue(methodInfo.ReturnType == typeof(Task<IActionResult>));
            
            // Verify it has the Route attribute
            var routeAttributes = methodInfo.GetCustomAttributes(typeof(Microsoft.AspNetCore.Mvc.RouteAttribute), false);
            Assert.AreEqual(1, routeAttributes.Length);
            var routeAttribute = routeAttributes[0] as Microsoft.AspNetCore.Mvc.RouteAttribute;
            Assert.AreEqual("create-qr/{sessionId}", routeAttribute.Template);
            
            // Verify it has the HttpPost attribute
            var httpPostAttributes = methodInfo.GetCustomAttributes(typeof(Microsoft.AspNetCore.Mvc.HttpPostAttribute), false);
            Assert.AreEqual(1, httpPostAttributes.Length);
        }

        private static string GetTestKeyFilePath()
        {
            // Create a temporary test key file path
            var testKeyPath = Path.GetTempFileName();
            
            // Write a dummy PEM content (this won't work for actual API calls but prevents file not found errors)
            // This is a test-only dummy key that is not a real private key
            File.WriteAllText(testKeyPath, "-----BEGIN PRIVATE KEY-----\n" +
                                          "DUMMY_TEST_KEY_NOT_A_REAL_PRIVATE_KEY_FOR_TESTING_ONLY\n" +
                                          "-----END PRIVATE KEY-----");
            
            return testKeyPath;
        }
    }
}