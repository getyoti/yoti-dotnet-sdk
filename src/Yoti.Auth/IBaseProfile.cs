using System.Collections.Generic;

namespace Yoti.Auth
{
    public interface IBaseProfile
    {
        YotiAttribute<T> GetAttributeByName<T>(string name);

        List<YotiAttribute<T>> FindAttributesStartingWith<T>(string prefix);
    }
}