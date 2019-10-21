using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Images;

namespace Yoti.Auth.Tests.TestTools
{
    internal static class AssertImages
    {
        public static void ContainsExpectedImage(List<Image> images, string mimeType, string expectedBase64UrlLast10)
        {
            foreach (var image in images)
            {
                string base64Url = image.GetBase64URI();
                string last10 = base64Url.Substring(base64Url.Length - 10);

                if (mimeType == image.GetMIMEType()
                    && expectedBase64UrlLast10 == last10)
                {
                    return;
                }
            }

            Assert.Fail();
        }
    }
}