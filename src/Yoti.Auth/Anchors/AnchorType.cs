namespace Yoti.Auth.Anchors
{
    public enum AnchorType
    {
        [StringValue("UNKNOWN")]
        [ExtensionOid("")]
        Unknown,

        [StringValue("SOURCE")]
        [ExtensionOid("1.3.6.1.4.1.47127.1.1.1")]
        Source,

        [StringValue("VERIFIER")]
        [ExtensionOid("1.3.6.1.4.1.47127.1.1.2")]
        Verifier
    }
}