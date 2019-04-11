using System;

namespace Yoti.Auth.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class ExtensionOidAttribute : System.Attribute
    {
        public readonly string ExtensionOid;

        public ExtensionOidAttribute(string extensionOid)
        {
            ExtensionOid = extensionOid;
        }
    }
}