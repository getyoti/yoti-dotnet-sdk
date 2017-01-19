using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yoti.Auth
{
    internal class Response
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Content { get; set; }
    }
}
