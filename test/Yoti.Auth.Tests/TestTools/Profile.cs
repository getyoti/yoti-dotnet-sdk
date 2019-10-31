using System.Collections.Generic;
using Yoti.Auth.Attribute;
using Yoti.Auth.Profile;

namespace Yoti.Auth.Tests.TestTools
{
    internal static class Profile
    {
        public static ApplicationProfile CreateApplicationProfileWithSingleAttribute<T>(YotiAttribute<T> attribute)
        {
            var attributes = new List<BaseAttribute>
            {
                { attribute }
            };

            return new ApplicationProfile(attributes);
        }

        public static YotiProfile CreateUserProfileWithSingleAttribute<T>(YotiAttribute<T> attribute)
        {
            var attributes = new List<BaseAttribute>
            {
                { attribute }
            };

            return new YotiProfile(attributes);
        }

        public static YotiProfile CreateUserProfileWithSingleAttribute<T>(ProtoBuf.Attribute.Attribute attribute)
        {
            return AddAttributeToProfile<T>(new YotiProfile(), attribute);
        }

        public static YotiProfile AddAttributeToProfile<T>(YotiProfile yotiProfile, ProtoBuf.Attribute.Attribute attribute)
        {
            BaseAttribute parsedAttribute = AttributeConverter.ConvertToBaseAttribute(attribute);

            if (parsedAttribute != null)
                yotiProfile.Add((YotiAttribute<T>)parsedAttribute);

            return yotiProfile;
        }
    }
}