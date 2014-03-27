using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Security;
using RealtyShares.UserNotifications.Models;

namespace RealtyShares.UserNotifications.Services
{
    public class NotificationsToUserDispatcher : INotificationsToUserDispatcher
    {
        private readonly INotificationService _notificationService;
        private readonly IContentManager _contentManager;
        private readonly INotificationConverter _notificationConverter;

        public NotificationsToUserDispatcher(
            INotificationService notificationService,
            IContentManager contentManager,
            INotificationConverter notificationConverter)
        {
            _notificationService = notificationService;
            _contentManager = contentManager;
            _notificationConverter = notificationConverter;
        }
        
    
        public void UpdateNotificationsForUser(IUser user, int maxNotificationBatchCountToCheck)
        {
            var notificationsUserPart = GetNotificationsUserPartOrThrow(user);

            var uncheckedNotificationCount = _contentManager
                .Query(VersionOptions.Published, Constants.NotificationBatchContentType)
                .Where<CommonPartRecord>(record => record.Id > notificationsUserPart.LastProcessedNotificationId)
                .Count();

            maxNotificationBatchCountToCheck = Math.Min(uncheckedNotificationCount, maxNotificationBatchCountToCheck);

            var notifications = _notificationService.FetchNotifications(user, maxNotificationBatchCountToCheck);

            var existingUserNotificationEntries = notificationsUserPart.RecentNotificationEntries.ToDictionary(entry => entry.NotificationId);
            var newUserNotificationEntries = new List<NotificationUserEntry>(maxNotificationBatchCountToCheck);
            foreach (var notification in notifications)
            {
                var notificationId = notification.ContentItem.Id;
                if (existingUserNotificationEntries.ContainsKey(notificationId))
                {
                    newUserNotificationEntries.Add(existingUserNotificationEntries[notificationId]);
                }
                else
                {
                    newUserNotificationEntries.Add(new NotificationUserEntry { NotificationId = notificationId });
                }
            }
            notificationsUserPart.RecentNotificationEntries = newUserNotificationEntries;

            if (notifications.Any())
            {
                notificationsUserPart.LastProcessedNotificationId = notifications.First().ContentItem.Id;
            }
        }

        public IEnumerable<INotification> GetRecentNotificationsForUser(IUser user, int count)
        {
            return _contentManager
                .GetMany<IContent>(GetNotificationsUserPartOrThrow(user).RecentNotificationEntries.Take(count).Select(entry => entry.NotificationId), VersionOptions.Published, QueryHints.Empty)
                .Select(notificationBatch => _notificationConverter.ConvertBatchToNotification(notificationBatch, user));
        }


        private static NotificationsUserPart GetNotificationsUserPartOrThrow(IUser user)
        {
            var notificationsUserPart = user.As<NotificationsUserPart>();

            if (notificationsUserPart == null)
            {
                throw new ArgumentException("The supplied user object should have NotificationsUserPart attached.");
            }

            return notificationsUserPart;
        }
    }
}