namespace Yoti.Auth.CustomAttributes
{
    internal class StringValueAttribute : System.Attribute
    {
        public readonly string StringValue;

        public StringValueAttribute(string stringValue)
        {
            StringValue = stringValue;
        }
    }
}