using System;
using Yoti.Auth.Profile;

namespace Yoti.Auth
{
    /// <summary>
    /// A class to represent the details of a share between a user and an application.
    /// </summary>
    public class ActivityDetails
    {
        internal ActivityDetails(string rememberMeId, string parentRememberMeId, DateTime? timestamp, YotiProfile yotiProfile, ApplicationProfile applicationProfile, string receiptId)
        {
            RememberMeId = rememberMeId;
            ParentRememberMeId = parentRememberMeId;
            Timestamp = timestamp;
            Profile = yotiProfile;
            ApplicationProfile = applicationProfile;
            ReceiptId = receiptId;
        }

        /// <summary>
        /// Return the rememberMeId, which is a unique, stable identifier for a user in the context
        /// of an application. You can use it to identify returning users. This value will be
        /// different for the same user in different applications.
        /// </summary>
        public string RememberMeId { get; private set; }

        /// <summary>
        /// Return the parentRememberMeId, which is a unique, stable identifier for a user in the
        /// context of an organisation. You can use it to identify returning users. This value is
        /// consistent for a given user across different applications belonging to a single organisation.
        /// </summary>
        public string ParentRememberMeId { get; private set; }

        /// <summary>
        /// Time (UTC) and date of the sharing activity.
        /// </summary>
        public DateTime? Timestamp { get; private set; }

        /// <summary>
        /// The <see cref="YotiProfile"/> returned by Yoti if the request was successful.
        /// </summary>
        public YotiProfile Profile { get; private set; }

        /// <summary>
        /// The profile associated with the application.
        /// </summary>
        public ApplicationProfile ApplicationProfile { get; private set; }

        /// <summary>
        /// Receipt ID identifying a completed sharing activity.
        /// </summary>
        /// <returns></returns>
        public string ReceiptId { get; private set; }
    }
}