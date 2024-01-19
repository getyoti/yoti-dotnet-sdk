//using Yoti.Auth.DigitalIdentity;
using Yoti.Auth.Profile;
using Yoti.Auth.Share;
namespace Yoti.Auth.DigitalIdentity
{
    public class SharedReceiptResponse
    {
        public string ID { get; set; }
        public string SessionID { get; set; }
        public string RememberMeID { get; set; }
        public string ParentRememberMeID { get; set; }
        public string Timestamp { get; set; }
        public string Error { get; set; }
        public UserContent UserContent { get; set; }
        public ApplicationContent ApplicationContent { get; set; }
    }

    public class ApplicationContent
    {
        public ApplicationProfile ApplicationProfile { get; set; }
        public ExtraData ExtraData { get; set; }
    }

    public class UserContent
    {
        public YotiProfile UserProfile { get; set; }
        public ExtraData ExtraData { get; set; }
    }

    /*public class ApplicationProfile
    {
        // Implement properties and methods for ApplicationProfile
    }

    public class UserProfile
    {
        // Implement properties and methods for UserProfile
    }*/
}