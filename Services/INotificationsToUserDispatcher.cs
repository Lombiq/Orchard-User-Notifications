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
        /// <summary>
        /// Updates the notifications that are accessible for a user, processing the given amount of notifications.
        /// </summary>
        /// <param name="user">The user to update the notifications for.</param>
        /// <param name="maxNotificationBatchCountToCheck">The amount of notification batches to check for retrieving the ones where the user is a recipient.</param>
        void UpdateNotificationsForUser(IUser user, int maxNotificationBatchCountToCheck);

        /// <summary>
        /// Retrieves the recent notifications of the users, as persisted in the user object.
        /// </summary>
        /// <param name="user">The user to retrieve the notifications for.</param>
        /// <param name="maxCount">The maximal number of notifications to retrieve.</param>
        /// <param name="justUnread">If true, will only return unread notifications.</param>
        /// <returns>The fetched notifications.</returns>
        IEnumerable<INotification> GetRecentNotificationsForUser(IUser user, int maxCount, bool justUnread);
    }
}
