namespace Yoti.Auth.CustomAttributes
{
    internal sealed class ProtobufNameAttribute : System.Attribute
    {
        public readonly string ProtobufName;

        public ProtobufNameAttribute(string name)
        {
            ProtobufName = name;
        }
    }
}