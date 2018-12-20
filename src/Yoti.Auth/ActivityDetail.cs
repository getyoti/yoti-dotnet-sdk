using System;

namespace Yoti.Auth
{
    /// <summary>
    /// A class to represent the details of a share between a user and an application.
    /// </summary>
    public class ActivityDetails
    {
        public ActivityDetails(string rememberMeId, DateTime? timestamp, YotiProfile yotiProfile, ApplicationProfile applicationProfile, string receiptID)
        {
            RememberMeId = rememberMeId;
            Timestamp = timestamp;
            Profile = yotiProfile;
            ApplicationProfile = applicationProfile;
            ReceiptID = receiptID;
        }

        /// <summary>
        /// The unique identifier for a particular user.
        /// </summary>
        public string RememberMeId { get; private set; }

        /// <summary>
        /// Time and date of the share.
        /// </summary>
        public DateTime? Timestamp { get; private set; }

        /// <summary>
        /// The <see cref="YotiProfile"/> returned by Yoti if the request was successful.
        /// </summary>
        public YotiProfile Profile { get; set; }

        /// <summary>
        /// The profile associated with the application.
        /// </summary>
        public ApplicationProfile ApplicationProfile { get; private set; }

        /// <summary>
        /// Receipt ID identifying a completed activity.
        /// </summary>
        /// <returns></returns>
        public string ReceiptID { get; private set; }
    }
}