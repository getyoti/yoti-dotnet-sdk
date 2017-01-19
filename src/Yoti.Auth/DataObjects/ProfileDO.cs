using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yoti.Auth.DataObjects
{
    internal class ProfileDO
    {
        public string session_data { get; set; }

        public ReceiptDO receipt { get; set; }
    }
}
