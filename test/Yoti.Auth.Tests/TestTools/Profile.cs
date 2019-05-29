using System.Collections.Generic;
using Yoti.Auth.Profile;

namespace Yoti.Auth.Tests.TestTools
{
    internal static class Profile
    {
        public static ApplicationProfile CreateApplicationProfileWithSingleAttribute<T>(YotiAttribute<T> attribute)
        {
            var attributes = new Dictionary<string, BaseAttribute>
            {
                { attribute.GetName(), attribute }
            };

            return new ApplicationProfile(attributes);
        }

        public static YotiProfile CreateUserProfileWithSingleAttribute<T>(YotiAttribute<T> attribute)
        {
            var attributes = new Dictionary<string, BaseAttribute>
            {
                { attribute.GetName(), attribute }
            };

            return new YotiProfile(attributes);
        }
    }
}