namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedThirdPartyIdentityCheckBuilder
    {
        private int? _handledCheckLimit;

        /// <summary>
        /// Sets the number of times a check response should return a handled result before switching to success (for sandbox testing)
        /// </summary>
        /// <param name="handledCheckLimit">The number of handled check responses before success</param>
        /// <returns>The <see cref="RequestedThirdPartyIdentityCheckBuilder"/></returns>
        public RequestedThirdPartyIdentityCheckBuilder WithHandledCheckLimit(int handledCheckLimit)
        {
            Validation.NotLessThan(handledCheckLimit, 0, nameof(handledCheckLimit));
            _handledCheckLimit = handledCheckLimit;
            return this;
        }

        public RequestedThirdPartyIdentityCheck Build()
        {
            RequestedThirdPartyIdentityCheckConfig config = new RequestedThirdPartyIdentityCheckConfig(_handledCheckLimit);

            return new RequestedThirdPartyIdentityCheck(config);
        }
    }
}