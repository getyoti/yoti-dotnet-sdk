namespace Yoti.Auth.Anchors
{
    public enum AnchorType
    {
        [ExtensionOid("")]
        UNKNOWN,

        [ExtensionOid("1.3.6.1.4.1.47127.1.1.1")]
        SOURCE,

        [ExtensionOid("1.3.6.1.4.1.47127.1.1.2")]
        VERIFIER
    }
}