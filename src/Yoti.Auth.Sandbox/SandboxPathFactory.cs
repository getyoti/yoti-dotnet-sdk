using Yoti.Auth.Web;

namespace Yoti.Auth.Sandbox
{
    public static class SandboxPathFactory
    {
        public static string CreateSandboxPath(string appId)
        {
            return $"/apps/{appId}/tokens?timestamp={EndpointFactory.GetTimestamp()}&nonce={CryptoEngine.GenerateNonce()}";
        }
    }
}