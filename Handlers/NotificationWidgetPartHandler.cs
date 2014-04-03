using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Settings;
using RealtyShares.UserNotifications.Models;
using RealtyShares.UserNotifications.Services;
using System.Linq;

namespace RealtyShares.UserNotifications.Handlers
{
    /// <summary>
    /// Content handler for <see cref="RealtyShares.UserNotifications.Models.NotificationWidgetPart"/>.
    /// </summary>
    public class NotificationWidgetPartHandler : ContentHandler
    {
        public NotificationWidgetPartHandler(INotificationsToUserDispatcher notificationDispatcher, IWorkContextAccessor wca, ISiteService siteService)
        {
            OnActivated<NotificationWidgetPart>((context, part) =>
            {
                part.UnreadNotificationsField.Loader(() =>
                    {
                        var currentUser = wca.GetContext().CurrentUser;

                        if (currentUser == null)
                        {
                            return Enumerable.Empty<INotification>().ToList();
                        }

                        var notificationCount = siteService.GetSiteSettings().As<NotificationWidgetSettingsPart>().NotificationCount;
                        var unreadNotifications = notificationDispatcher
                            .GetRecentNotificationsForUser(currentUser, notificationCount, true)
                            .ToList();

                        return unreadNotifications;
                    });
            });
        }
    }
}