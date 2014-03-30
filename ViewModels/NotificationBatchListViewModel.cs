using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtyShares.UserNotifications.ViewModels
{
    public enum NotificationBatchSortBy
    {
        DateSent = 0,
        Title = 1
    }


    public class NotificationBatchListViewModel
    {
        public Func<IEnumerable<dynamic>> NotificationBatchShapes { get; set; }

        public string Keywords { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public NotificationBatchSortBy NotificationBatchSortBy { get; set; }
    }
}