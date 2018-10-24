using System.Collections.Generic;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth.Tests.TestTools
{
    internal class Profile
    {
        public static ApplicationProfile CreateApplicationProfileWithSingleAttribute(byte[] value, string name, TypeEnum type)
        {
            var yotiAttributeValue = new YotiAttributeValue(
                type,
                 value);
            var yotiAttribute = new YotiAttribute<object>(name, yotiAttributeValue);

            var attributes = new Dictionary<string, YotiAttribute<object>>
            {
                { name, yotiAttribute }
            };

            var profile = new ApplicationProfile(attributes);
            return profile;
        }
    }
}