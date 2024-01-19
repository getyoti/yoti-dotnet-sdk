using System.Collections.Generic;
using Yoti.Auth.Attribute;

namespace Yoti.Auth.DigitalIdentity
{
    public interface IBaseProfile
    {
        YotiAttribute<T> GetAttributeByName<T>(string name);

        List<YotiAttribute<T>> FindAttributesStartingWith<T>(string prefix);
    }
}