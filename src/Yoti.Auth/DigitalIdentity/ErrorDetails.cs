using Newtonsoft.Json;
using System.Collections.Generic;
using Yoti.Auth.DigitalIdentity;

namespace Yoti.Auth.DigitalIdentity
{
    public class ErrorDetails
    {
        public ErrorReason ErrorReason { get; private set; }

        public ErrorReason GetErrorReason()
        {
            return ErrorReason;
        }
    }
    
}
