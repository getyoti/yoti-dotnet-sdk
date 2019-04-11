using System;

namespace Yoti.Auth.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class StringValueAttribute : System.Attribute
    {
        public readonly string StringValue;

        public StringValueAttribute(string stringValue)
        {
            StringValue = stringValue;
        }
    }
}