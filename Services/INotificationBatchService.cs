using Orchard;
using Orchard.ContentManagement;
using RealtyShares.UserNotifications.ViewModels;
using System;
using System.Collections.Generic;

namespace RealtyShares.UserNotifications.Services
{
    public interface INotificationBatchService : IDependency
    {
        IEnumerable<ContentItem> GetNotificationBatches();

        IEnumerable<ContentItem> GetFilteredNotificationBatches(string[] keywords, DateTime fromDate, DateTime toDate, NotificationBatchSortBy sortBy);
    }
}