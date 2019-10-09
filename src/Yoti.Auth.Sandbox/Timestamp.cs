using System;

namespace Yoti.Auth.Sandbox
{
    public static class Timestamp
    {
        public static long GetUnixTimeMicroseconds(DateTime utcDateTime)
        {
            DateTimeOffset dto = new DateTimeOffset(utcDateTime);
            return dto.ToUnixTimeMilliseconds() * 1000;
        }
    }
}