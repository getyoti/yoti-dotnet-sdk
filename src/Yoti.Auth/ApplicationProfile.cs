using System.Collections.Generic;

namespace Yoti.Auth
{
    /// <summary>
    /// Profile of an application, with convenience methods to access well-known attributes.
    /// </summary>
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile() : base()
        {
        }

        public ApplicationProfile(Dictionary<string, BaseAttribute> attributes) : base(attributes)
        {
        }

        /// <summary>
        /// The name of the application.
        /// </summary>
        public YotiAttribute<string> Name
        {
            get
            {
                return GetAttributeByName<string>(name: YotiConstants.ApplicationNameAttribute);
            }
        }

        /// <summary>
        /// The URL where the application is available at.
        /// </summary>
        public YotiAttribute<string> URL
        {
            get
            {
                return GetAttributeByName<string>(name: YotiConstants.ApplicationURLAttribute);
            }
        }

        /// <summary>
        /// The logo of the application that will be displayed to users that perform a share with it.
        /// </summary>
        public YotiAttribute<Image> Logo
        {
            get
            {
                return GetAttributeByName<Image>(name: YotiConstants.ApplicationLogoAttribute);
            }
        }

        /// <summary>
        /// The background colour that will be displayed on each receipt the user gets, as a result of a share with the application.
        /// </summary>
        public YotiAttribute<string> ReceiptBackgroundColour
        {
            get
            {
                return GetAttributeByName<string>(name: YotiConstants.ApplicationReceiptBgColourAttribute);
            }
        }
    }
}