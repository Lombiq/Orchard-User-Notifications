using Orchard;
using Orchard.ContentManagement;
using RealtyShares.UserNotifications.ViewModels;
using System;
using System.Collections.Generic;

namespace RealtyShares.UserNotifications.Services
{
    /// <summary>
    /// Queries Notification Batches with the given parameters.
    /// </summary>
    public interface INotificationBatchService : IDependency
    {
        /// <summary>
        /// Gets the notification batch content items from database.
        /// </summary>
        /// <returns>List of notification batch content items</returns>
        IEnumerable<ContentItem> GetNotificationBatches();

        /// <summary>
        /// Gets the filtered notification batch content items from database.
        /// </summary>
        /// <param name="keywords">Keywords to search in the item fields</param>
        /// <param name="fromDate">Lowest date of the sent notifications</param>
        /// <param name="toDate">Highest date of the sent notifications</param>
        /// <param name="sortBy">Sorting mode of the notification batch content items</param>
        /// <returns>List of filtered notification batch content items</returns>
        IEnumerable<ContentItem> GetFilteredNotificationBatches(string keywords, DateTime? fromDate, DateTime? toDate, NotificationBatchSortBy sortBy);
    }
}