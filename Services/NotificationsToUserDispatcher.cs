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

            var notifications = _notificationService.FetchNotifications(user, Math.Min(uncheckedNotificationCount, maxNotificationBatchCountToCheck));

            var existingUserNotificationEntriesLookup = notificationsUserPart.RecentNotificationEntries.ToDictionary(entry => entry.NotificationId);
            var existingUserNotificationEntries = new Stack<NotificationUserEntry>(notificationsUserPart.RecentNotificationEntries.Reverse());
            foreach (var notification in notifications)
            {
                var notificationId = notification.ContentItem.Id;
                if (!existingUserNotificationEntriesLookup.ContainsKey(notificationId))
                {
                    existingUserNotificationEntries.Push(new NotificationUserEntry { NotificationId = notificationId });
                }
            }
            notificationsUserPart.RecentNotificationEntries = existingUserNotificationEntries.Take(maxNotificationBatchCountToCheck);

            if (notifications.Any())
            {
                notificationsUserPart.LastProcessedNotificationId = notifications.First().ContentItem.Id;
            }
        }

        public IEnumerable<INotification> GetRecentNotificationsForUser(IUser user, int maxCount, bool justUnread)
        {
            var notificationIds = GetNotificationsUserPartOrThrow(user).RecentNotificationEntries;
            if (justUnread) notificationIds = notificationIds.Where(notification => !notification.IsRead);
            return _contentManager
                .GetMany<IContent>(notificationIds.Take(maxCount).Select(entry => entry.NotificationId), VersionOptions.Published, QueryHints.Empty)
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