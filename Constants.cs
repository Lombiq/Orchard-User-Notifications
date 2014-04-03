using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtyShares.UserNotifications
{
    /// <summary>
    /// Stores well-known constants used throughout the module.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Technical name of the Notification  Batch content type.
        /// </summary>
        public const string NotificationBatchContentType = "NotificationBatch";

        /// <summary>
        /// Technical name of the Recipient List content type.
        /// </summary>
        public const string RecipientListContentType = "RecipientList";

        /// <summary>
        /// Technical name of the Notification Widget content type.
        /// </summary>
        public const string NotificationWidgetContentType = "NotificationWidget";

        /// <summary>
        /// How many notification batches to check at max when trying to retrieve the latest notifications for a user.
        /// </summary>
        public const int MaxNotificationBatchCountToCheck = 100;

        /// <summary>
        /// What the interval should be (in minutes) between checking for new notifications for a logged in user.
        /// </summary>
        public const int NewNotificationCheckIntervalMinutes = 10;
    }
}