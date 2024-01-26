using Yoti.Auth.ShareUrl;
using Yoti.Auth.DigitalIdentity.Policy;
using Yoti.Auth.Tests.TestData;
using Yoti.Auth.DigitalIdentity;

namespace Yoti.Auth.Tests.TestTools
{
    internal static class CreateQr
    {
        public static QrRequest CreateQrStandard()
        {
            return new QrRequest();
        }
    }
}
