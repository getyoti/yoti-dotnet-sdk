using Newtonsoft.Json;

namespace Yoti.Auth.Sandbox.Profile.Response
{
    public class YotiTokenResponse
    {
        public string Token { private set; get; }

        public YotiTokenResponse([JsonProperty(PropertyName = "token")]string token)
        {
            Token = token;
        }
    }
}