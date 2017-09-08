using System;
using System.Globalization;

namespace Yoti.Auth
{
    public class YotiAttributeValue
    {
        public enum TypeEnum { Text, Date, Jpeg, Png }

        private readonly TypeEnum _type;
        private readonly byte[] _data;

        public YotiAttributeValue(TypeEnum type, byte[] data)
        {
            _type = type;
            _data = data;
        }

        public YotiAttributeValue(TypeEnum type, string data)
        {
            _type = type;

            switch (_type)
            {
                case TypeEnum.Jpeg:
                case TypeEnum.Png:
                    _data = Conversion.Base64ToBytes(data);
                    break;

                default:
                    _data = Conversion.UtfToBytes(data);
                    break;
            }
        }

        public TypeEnum Type
        {
            get
            {
                return _type;
            }
        }

        public byte[] ToBytes()
        {
            return _data;
        }

        public override string ToString()
        {
            switch (_type)
            {
                case TypeEnum.Jpeg:
                case TypeEnum.Png:
                    return Conversion.BytesToBase64(_data);

                default:
                    return Conversion.BytesToUtf8(_data);
            }
        }

        public DateTime? ToDate()
        {
            switch (_type)
            {
                case TypeEnum.Date:
                    DateTime date;
                    if (DateTime.TryParseExact(ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                    {
                        return date;
                    }
                    else
                    {
                        return null;
                    }
                default:
                    return null;
            }
        }

        public string ToDataUrlString()
        {
            switch (_type)
            {
                case TypeEnum.Jpeg:
                    return "data:image/jpeg;base64," + ToString();
                case TypeEnum.Png:
                    return "data:image/png;base64," + ToString();
                default:
                    return null;
            }
        }
    }
}