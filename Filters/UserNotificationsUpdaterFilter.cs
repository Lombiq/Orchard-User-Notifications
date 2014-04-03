using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard;
using Orchard.Mvc.Filters;
using Orchard.UI.Admin;
using RealtyShares.UserNotifications.Services;
using Orchard.ContentManagement;
using RealtyShares.UserNotifications.Models;
using Orchard.Services;
using Orchard.Environment.State;
using Orchard.Environment.Configuration;
using Orchard.Environment.Descriptor;

namespace RealtyShares.UserNotifications.Filters
{
    /// <summary>
    /// Filter for updating the currently authenticated user's (if any) notifications periodically, after requests.
    /// </summary>
    public class UserNotificationsUpdaterFilter : FilterProvider, IActionFilter
    {
        private readonly IWorkContextAccessor _wca;
        private readonly IClock _clock;
        private readonly IProcessingEngine _processingEngine;
        private readonly ShellSettings _shellSettings;
        private readonly IShellDescriptorManager _shellDescriptorManager;


        public UserNotificationsUpdaterFilter(
            IWorkContextAccessor wca,
            IClock clock,
            IProcessingEngine processingEngine,
            ShellSettings shellSettings,
            IShellDescriptorManager shellDescriptorManager,
            INotificationsToUserDispatcher notificationsDispatcher)
        {
            _wca = wca;
            _clock = clock;
            _processingEngine = processingEngine;
            _shellSettings = shellSettings;
            _shellDescriptorManager = shellDescriptorManager;
        }


        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var user = _wca.GetContext().CurrentUser;

            // Not doing anything on the admin or for anonymous users, and also 
            if (AdminFilter.IsApplied(filterContext.RequestContext) || user == null)
            {
                return;
            }

            var notificationsUserPart = user.As<NotificationsUserPart>();
            // Not checking if CheckIntervalMinutes hasn't passed yet since the last update.
            //if (notificationsUserPart.LastCheckedUtc >= _clock.UtcNow.AddMinutes(-1 * Constants.NewNotificationCheckIntervalMinutes))
            //{
            //    return;
            //}

            notificationsUserPart.LastCheckedUtc = _clock.UtcNow;

            // Using the processing engine to do the update after the request has executed so the user experience is not impacted.
            var shellDescriptor = _shellDescriptorManager.GetShellDescriptor();
            _processingEngine.AddTask(
                _shellSettings,
                shellDescriptor,
                "IUserNotificationsUpdaterEventHandler.UpdateNotificationsForUser",
                new Dictionary<string, object> { { "userId", user.ContentItem.Id } }
            );
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }
    }
}