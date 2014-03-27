using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using Orchard.Security;
using RealtyShares.UserNotifications.Models;

namespace RealtyShares.UserNotifications.Services
{
    /// <summary>
    /// Service for dealing with dispatching notifications from their batches to users.
    /// </summary>
    public interface INotificationsToUserDispatcher : IDependency
    {
        void UpdateNotificationsForUser(IUser user, int maxNotificationBatchCountToCheck);
        IEnumerable<INotification> GetRecentNotificationsForUser(IUser user, int count);
    }
}
