using System.IO;
using Google.Protobuf;

namespace Yoti.Auth.Tests.TestTools
{
    internal static class Protobuf
    {
        public static byte[] SerializeProtobuf<T>(T protobufObject) where T : IMessage
        {
            byte[] byteValue;
            using (MemoryStream stream = new MemoryStream())
            {
                protobufObject.WriteTo(stream);
                byteValue = stream.ToArray();
            }

            return byteValue;
        }
    }
}