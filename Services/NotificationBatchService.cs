using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;
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
                .Query(VersionOptions.AllVersions, Constants.NotificationBatchContentType)
                .OrderByDescending<CommonPartRecord>(record => record.Id)
                .List();

            return notificationBatchItems;
        }

        public IEnumerable<ContentItem> GetFilteredNotificationBatches(string[] keywords, DateTime fromDate, DateTime toDate, NotificationBatchSortBy sortBy)
        {
            var notificationBatchItems = GetNotificationBatches()
                .Where(item => item.As<CommonPart>().CreatedUtc >= fromDate
                    && item.As<CommonPart>().CreatedUtc <= toDate);

            switch (sortBy)
            {
                case NotificationBatchSortBy.DateSent: notificationBatchItems = notificationBatchItems.OrderBy(item => item.As<CommonPart>().PublishedUtc);
                    break;
                case NotificationBatchSortBy.Title: notificationBatchItems = notificationBatchItems.OrderBy(item => item.As<TitlePart>().Title);
                    break;
                default: notificationBatchItems = notificationBatchItems.OrderBy(item => item.As<CommonPart>().CreatedUtc);
                    break;
            }

            return notificationBatchItems;
        }
    }
}