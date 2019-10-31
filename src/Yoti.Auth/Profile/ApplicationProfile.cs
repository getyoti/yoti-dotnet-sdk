using System.Collections.Generic;
using Yoti.Auth.Attribute;
using Yoti.Auth.Images;

namespace Yoti.Auth.Profile
{
    /// <summary>
    /// Profile of an application, with convenience methods to access well-known attributes.
    /// </summary>
    public class ApplicationProfile : BaseProfile
    {
        internal ApplicationProfile() : base()
        {
        }

        internal ApplicationProfile(List<BaseAttribute> attributes) : base(attributes)
        {
        }

        /// <summary>
        /// The name of the application.
        /// </summary>
        public YotiAttribute<string> Name
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.ApplicationProfile.ApplicationNameAttribute);
            }
        }

        /// <summary>
        /// The URL where the application is available at.
        /// </summary>
        public YotiAttribute<string> URL
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.ApplicationProfile.ApplicationURLAttribute);
            }
        }

        /// <summary>
        /// The logo of the application that will be displayed to users that perform a share with it.
        /// </summary>
        public YotiAttribute<Image> Logo
        {
            get
            {
                return GetAttributeByName<Image>(name: Constants.ApplicationProfile.ApplicationLogoAttribute);
            }
        }

        /// <summary>
        /// The background color that will be displayed on each receipt the user gets, as a result of
        /// a share with the application.
        /// </summary>
        public YotiAttribute<string> ReceiptBackgroundColor
        {
            get
            {
                return GetAttributeByName<string>(name: Constants.ApplicationProfile.ApplicationReceiptBgColorAttribute);
            }
        }
    }
}