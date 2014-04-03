using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Events;
using Orchard.Security;

namespace RealtyShares.UserNotifications.Events
{
    /// <summary>
    /// Handles requests for updating a user's notifications.
    /// </summary>
    public interface IUserNotificationsUpdaterEventHandler : IEventHandler
    {
        /// <summary>
        /// Updates the user's notifications through the relevant services.
        /// </summary>
        /// <remarks>
        /// This event handler is used through <see cref="Orchard.Environment.State.IProcessingEngine"/>. As such it's only safe to pass
        /// primitive types to its methods and never objects that have references to the DB (like content items).
        /// </remarks>
        /// <param name="userId">ID of the user.</param>
        void UpdateNotificationsForUser(int userId);
    }
}
