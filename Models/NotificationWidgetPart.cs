using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;
using System.Collections;
using System.Collections.Generic;

namespace RealtyShares.UserNotifications.Models
{
    /// <summary>
    /// Empty part for the Notification Widget.
    /// </summary>
    public class NotificationWidgetPart : ContentPart
    {
        private readonly LazyField<IList<INotification>> _unreadNotifications = new LazyField<IList<INotification>>();
        internal LazyField<IList<INotification>> UnreadNotificationsField { get { return _unreadNotifications; } }
        public IList<INotification> UnreadNotifications
        {
            get { return _unreadNotifications.Value; }
            set { _unreadNotifications.Value = value; }
        }
    }
}