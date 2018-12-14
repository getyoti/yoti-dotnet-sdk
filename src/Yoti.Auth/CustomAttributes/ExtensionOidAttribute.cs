namespace Yoti.Auth.CustomAttributes
{
    internal sealed class ExtensionOidAttribute : System.Attribute
    {
        public readonly string ExtensionOid;

        public ExtensionOidAttribute(string extensionOid)
        {
            ExtensionOid = extensionOid;
        }
    }
}