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
        INotification ConvertBatchToNotification(IContent notificationBatch, IUser recipient);
    }
}
