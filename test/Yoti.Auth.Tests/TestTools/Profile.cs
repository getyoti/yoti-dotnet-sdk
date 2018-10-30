using System.Collections.Generic;

namespace Yoti.Auth.Tests.TestTools
{
    internal class Profile
    {
        public static ApplicationProfile CreateApplicationProfileWithSingleAttribute<T>(YotiAttribute<T> attribute)
        {
            var attributes = new Dictionary<string, BaseAttribute>
            {
                { attribute.GetName(), attribute }
            };

            return new ApplicationProfile(attributes);
        }
    }
}