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
        /// <summary>
        /// The <see cref="YotiUserProfile"/> returned by Yoti if the request was successful.
        /// </summary>
        [Obsolete("Please use Profile instead")]
        public YotiUserProfile UserProfile { get; set; }

        /// <summary>
        /// The <see cref="YotiProfile"/> returned by Yoti if the request was successful.
        /// </summary>
        public YotiProfile Profile { get; set; }

        /// <summary>
        /// The outcome status of the request.
        /// </summary>
        public ActivityOutcome Outcome { get; set; }
    }
}