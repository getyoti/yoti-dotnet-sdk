using System.Collections.Generic;
using Yoti.Auth.Images;

namespace Yoti.Auth.Tests.TestTools
{
    internal class ImageComparer : IEqualityComparer<Image>
    {
        public bool Equals(Image x, Image y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return (x.Content() == y.Content())
                && (x.GetMIMEType() == y.GetMIMEType());
        }

        public int GetHashCode(Image obj)
        {
            throw new System.NotImplementedException();
        }
    }
}