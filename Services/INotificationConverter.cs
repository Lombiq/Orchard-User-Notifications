using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Security;
using RealtyShares.UserNotifications.Models;

namespace RealtyShares.UserNotifications.Services
{
    /// <summary>
    /// Converts Notification Batches to notifications.
    /// </summary>
    public interface INotificationConverter : IDependency
    {
        /// <summary>
        /// Converts a notification batch content item to an INotification object.
        /// </summary>
        /// <param name="notificationBatch">The notification batch to convert.</param>
        /// <param name="recipient">The user who is the recipient of the notification.</param>
        /// <returns>The INotification object.</returns>
        INotification ConvertBatchToNotification(IContent notificationBatch, IUser recipient);
    }
}
