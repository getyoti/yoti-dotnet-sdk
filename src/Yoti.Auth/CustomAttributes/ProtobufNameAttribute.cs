namespace Yoti.Auth
{
    internal class ProtobufNameAttribute : System.Attribute
    {
        public readonly string ProtobufName;

        public ProtobufNameAttribute(string name)
        {
            ProtobufName = name;
        }
    }
}