using System;

namespace Yoti.Auth
{
    /// <summary>
    /// A enum to represent the success state when requesting a <see cref="YotiUserProfile"/> from Yoti.
    /// </summary>
    public enum ActivityOutcome { Success, ProfileNotFound, Failure, SharingFailure }

    /// <summary>
    /// A class to represent the outcome of a request for a <see cref="YotiUserProfile"/> from Yoti.
    /// </summary>
    public class ActivityDetails
    {
        public ActivityDetails(ActivityOutcome activityOutcome)
        {
            RememberMeId = null;
            Timestamp = null;
            Profile = null;
            ApplicationProfile = null;
            ReceiptID = null;
            Outcome = activityOutcome;
        }

        public ActivityDetails(string rememberMeId, DateTime? timestamp, YotiProfile yotiProfile, ApplicationProfile applicationProfile, string receiptID, ActivityOutcome activityOutcome)
        {
            RememberMeId = rememberMeId;
            Timestamp = timestamp;
            Profile = yotiProfile;
            ApplicationProfile = applicationProfile;
            ReceiptID = receiptID;
            Outcome = activityOutcome;
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
        /// The outcome status of the request.
        /// </summary>
        public ActivityOutcome Outcome { get; set; }

        /// <summary>
        /// Receipt ID identifying a completed activity.
        /// </summary>
        /// <returns></returns>
        public string ReceiptID { get; private set; }
    }
}