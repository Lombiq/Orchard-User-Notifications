using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;

namespace RealtyShares.UserNotifications.Models
{
    /// <summary>
    /// Represents a notification that can be sent out to users.
    /// </summary>
    public interface INotification : IContent
    {
        /// <summary>
        /// Title of the notification.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Body text of the notification.
        /// </summary>
        string Body { get; }
    }
}
