using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.Data;
using Orchard.Events;
using Orchard.Security;
using RealtyShares.UserNotifications.Services;

namespace RealtyShares.UserNotifications.Events
{
    /// <summary>
    /// Updates a user's notifications when necessary.
    /// </summary>
    public class UserNotificationsUpdaterEventHandler : IUserNotificationsUpdaterEventHandler
    {
        private readonly INotificationsToUserDispatcher _notificationsDispatcher;
        private readonly IContentManager _contentManager;


        public UserNotificationsUpdaterEventHandler(INotificationsToUserDispatcher notificationsDispatcher, IContentManager contentManager)
        {
            _notificationsDispatcher = notificationsDispatcher;
            _contentManager = contentManager;
        }


        void IUserNotificationsUpdaterEventHandler.UpdateNotificationsForUser(int userId)
        {
            var user = _contentManager.Get(userId);
            if (user == null) return;
            _notificationsDispatcher.UpdateNotificationsForUser(user.As<IUser>(), Constants.MaxNotificationBatchCountToCheck);
        }
    }
}