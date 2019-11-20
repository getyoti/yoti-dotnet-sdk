using System.Collections.Generic;
using Yoti.Auth.Share.ThirdParty;

namespace Yoti.Auth.Tests.Share.ThirdParty.Issuing
{
    internal class IssuingAttributeComparer : IEqualityComparer<IssuingAttribute>
    {
        public bool Equals(IssuingAttribute x, IssuingAttribute y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return (x.Name == y.Name)
                && (x.Value == y.Value);
        }

        public int GetHashCode(IssuingAttribute obj)
        {
            throw new System.NotImplementedException();
        }
    }
}