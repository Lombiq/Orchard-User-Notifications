using Orchard.Security;
using RealtyShares.UserNotifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Tokens;
using Orchard.Core.Title.Models;

namespace RealtyShares.UserNotifications.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IContentManager _contentManager;
        private readonly INotificationConverter _notificationConverter;


        public NotificationService(IContentManager contentManager, INotificationConverter notificationConverter)
        {
            _contentManager = contentManager;
            _notificationConverter = notificationConverter;
        }


        public void SendNotification(IEnumerable<IUser> recipients, string tokenizedTitle, string tokenizedBody)
        {
            var notificationBatch = _contentManager.New(Constants.NotificationBatchContentType);

            notificationBatch.As<NotificationRecipientsPart>().Recipients = recipients;
            notificationBatch.As<TitlePart>().Title = tokenizedTitle;
            notificationBatch.As<BodyPart>().Text = tokenizedBody;

            _contentManager.Create(notificationBatch);
            _contentManager.Publish(notificationBatch);
        }

        public IEnumerable<INotification> FetchNotifications(IUser recipient, int maxNotificationBatchCountToCheck)
        {
            // This is the drawback of using this setup: we gain by not having to do operations in the magnitude of the count of recipients
            // when sending notifications but we have to do a linear search for notifications per a given user.
            return _contentManager
                .Query(VersionOptions.Published, Constants.NotificationBatchContentType)
                .OrderByDescending<CommonPartRecord>(record => record.Id)
                .Slice(maxNotificationBatchCountToCheck)
                .Select(notification => notification.As<NotificationBatchPart>())
                .Where(notification => notification.ActualRecipientIds.Contains(recipient.ContentItem.Id))
                .Select(notification => _notificationConverter.ConvertBatchToNotification(notification, recipient));
        }
    }
}