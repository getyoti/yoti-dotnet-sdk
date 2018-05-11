using System;
using System.Globalization;

namespace Yoti.Auth
{
    internal static class ExtensionMethods
    {
        public static T ConvertType<T>(this byte[] bytes)
        {
            var type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.String:
                    return (T)(object)Conversion.BytesToUtf8(bytes);

                case TypeCode.DateTime:
                    DateTime date;
                    if (DateTime.TryParseExact(
                        s: Conversion.BytesToUtf8(bytes),
                        format: "yyyy-MM-dd",
                        provider: CultureInfo.InvariantCulture,
                        style: DateTimeStyles.None,
                        result: out date))
                    {
                        return (T)(object)date;
                    }
                    else throw new InvalidCastException("Unable to cast to DateTime");

                case TypeCode.Boolean:
                    bool parsed = Boolean.TryParse(Conversion.BytesToUtf8(bytes), out bool output);

                    if (!parsed)
                        throw new FormatException(
                            String.Format(
                                "'{0}' value was unable to be parsed into a bool",
                                bytes));

                    return (T)(object)output;

                case TypeCode.Object:
                    return (T)(object)bytes;

                default:
                    return (T)Convert.ChangeType(bytes, typeof(T));
            }
        }
    }
}