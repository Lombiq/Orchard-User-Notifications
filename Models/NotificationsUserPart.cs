using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace RealtyShares.UserNotifications.Models
{
    /// <summary>
    /// Represents a notification that was received by a user.
    /// </summary>
    public class NotificationUserEntry
    {
        public int NotificationId { get; set; }
        public bool IsRead { get; set; }
    }


    /// <summary>
    /// Content part to be attached to the User content type to handle notifications sent to a user.
    /// </summary>
    /// <remarks>
    /// This part doesn't need a driver as it's attached to the User content type through an ActivatingFilter
    /// from <see cref="RealtyShares.UserNotifications.Handlers.NotificationsUserPartHandler"/>.
    /// </remarks>
    public class NotificationsUserPart : ContentPart
    {
        public DateTime LastCheckedUtc
        {
            get { return this.Retrieve(x => x.LastCheckedUtc); }
            set { this.Store(x => x.LastCheckedUtc, value); }
        }

        public int LastProcessedNotificationId
        {
            get { return this.Retrieve(x => x.LastProcessedNotificationId); }
            set { this.Store(x => x.LastProcessedNotificationId, value); }
        }

        private readonly LazyField<IEnumerable<NotificationUserEntry>> _recentNotificationEntries = new LazyField<IEnumerable<NotificationUserEntry>>();
        internal LazyField<IEnumerable<NotificationUserEntry>> RecentNotificationEntriesField { get { return _recentNotificationEntries; } }
        public IEnumerable<NotificationUserEntry> RecentNotificationEntries
        {
            get { return _recentNotificationEntries.Value; }
            set { _recentNotificationEntries.Value = value; }
        }
    }
}