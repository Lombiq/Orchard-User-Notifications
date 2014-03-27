using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Events;
using Orchard.Security;

namespace RealtyShares.UserNotifications.Services
{
    /// <summary>
    /// Only here instead of <see cref="Orchard.Users.Events.IUserEventHandler"/> so <see cref="UserNotificationsUpdater"/> is not forced to
    /// implement all the event handling methods.
    /// </summary>
    public interface IUserEventHandler : IEventHandler
    {
        void LoggedIn(IUser user);
    }


    /// <summary>
    /// Updates a user's notifications when necessary.
    /// </summary>
    public class UserNotificationsUpdater : IUserEventHandler
    {
        private readonly INotificationsToUserDispatcher _notificationsDispatcher;


        public UserNotificationsUpdater(INotificationsToUserDispatcher notificationsDispatcher)
        {
            _notificationsDispatcher = notificationsDispatcher;
        }
        
    
        public void LoggedIn(IUser user)
        {
            _notificationsDispatcher.UpdateNotificationsForUser(user, 100);
        }
    }
}