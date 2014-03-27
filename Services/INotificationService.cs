using Orchard;
using Orchard.Security;
using RealtyShares.UserNotifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealtyShares.UserNotifications.Services
{
    /// <summary>
    /// Service for sending out notifications to users.
    /// </summary>
    public interface INotificationService : IDependency
    {
        /// <summary>
        /// Sends a new notification to the designated recipients.
        /// </summary>
        /// <param name="recipients">The users who will receive the notification</param>
        /// <param name="tokenizedTitle">The tokenized notification title to send.</param>
        /// <param name="tokenizedBody">The tokenized notification body to send.</param>
        void SendNotification(IEnumerable<IUser> recipients, string tokenizedTitle, string tokenizedBody);

        /// <summary>
        /// Fetches the recent notifications where the given user is one of the recipients.
        /// </summary>
        /// <param name="recipient">The user who received the notifications.</param>
        /// <param name="maxNotificationBatchCountToCheck">How many of the most recent notification batches to check at max when filtering 
        /// for the user's notifications.</param>
        /// <returns>Matching notifications.</returns>
        IEnumerable<INotification> FetchNotifications(IUser recipient, int maxNotificationBatchCountToCheck);
    }


    /// <summary>
    /// Extensions for <see cref="INotificationService"/>.
    /// </summary>
    public static class NotificationServiceExtensions
    {
        /// <summary>
        /// Sends a new notification to the designated recipient.
        /// </summary>
        /// <param name="recipient">The user who will receive the notification</param>
        /// <param name="tokenizedTitle">The tokenized notification title to send.</param>
        /// <param name="tokenizedBody">The tokenized notification body to send.</param>
        public static void SendNotification(this INotificationService notificationService, IUser recipient, string tokenizedTitle, string tokenizedBody)
        {
            notificationService.SendNotification(new[] { recipient }, tokenizedTitle, tokenizedBody);
        }
    }
}
