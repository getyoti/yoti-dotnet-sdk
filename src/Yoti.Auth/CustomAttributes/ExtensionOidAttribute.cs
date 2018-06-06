namespace Yoti.Auth
{
    internal class ExtensionOidAttribute : System.Attribute
    {
        public readonly string ExtensionOid;

        public ExtensionOidAttribute(string extensionOid)
        {
            ExtensionOid = extensionOid;
        }
    }
}