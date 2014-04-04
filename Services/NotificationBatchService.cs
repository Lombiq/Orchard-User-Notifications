using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;
using Orchard.Validation;
using RealtyShares.UserNotifications.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtyShares.UserNotifications.Services
{
    public class NotificationBatchService : INotificationBatchService
    {
        private readonly IContentManager _contentManager;


        public NotificationBatchService(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }
	
			
        public IEnumerable<ContentItem> GetNotificationBatches()
        {
            var notificationBatchItems = _contentManager
                .Query(VersionOptions.Latest, Constants.NotificationBatchContentType)
                .OrderByDescending<CommonPartRecord>(record => record.Id)
                .List();

            return notificationBatchItems;
        }

        public IEnumerable<ContentItem> GetFilteredNotificationBatches(string[] keywords, DateTime? fromDate, DateTime? toDate, NotificationBatchSortBy sortBy)
        {
            var notificationBatchItems = _contentManager.Query(VersionOptions.Latest, Constants.NotificationBatchContentType);

            if (fromDate != null && toDate != null)
            {
                notificationBatchItems = notificationBatchItems.Where<CommonPartRecord>(record => record.CreatedUtc >= fromDate.Value && record.CreatedUtc <= toDate.Value);
            }

            switch (sortBy)
            {
                case NotificationBatchSortBy.DateSent: notificationBatchItems = notificationBatchItems
                    .OrderByDescending<CommonPartRecord>(record => record.PublishedUtc);
                    break;
                case NotificationBatchSortBy.Title: notificationBatchItems = notificationBatchItems
                    .OrderBy<TitlePartRecord>(record => record.Title);
                    break;
                default: 
                    throw new ArgumentOutOfRangeException("sortBy");
            }

            return notificationBatchItems.List();
        }
    }
}