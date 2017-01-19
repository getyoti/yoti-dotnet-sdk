using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yoti.Auth
{
    public enum ImageType { Jpeg, Png }
    public class Image
    {
        public ImageType Type { get; set; }
        public byte[] Data { get; set; }
    }
}
