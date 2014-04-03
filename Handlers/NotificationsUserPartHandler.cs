using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment;
using Orchard.Services;
using RealtyShares.UserNotifications.Models;
using Orchard.ContentManagement;
using Orchard.Validation;
using RealtyShares.UserNotifications.Services;
using Orchard.Security;

namespace RealtyShares.UserNotifications.Handlers
{
    /// <summary>
    /// Content handler for <see cref="RealtyShares.UserNotifications.Models.NotificationsUserPart"/>.
    /// </summary>
    public class NotificationsUserPartHandler : ContentHandler
    {
        public NotificationsUserPartHandler(IJsonConverter jsonConverter, Lazy<INotificationsToUserDispatcher> notificationDispatcherLazy)
        {
            Filters.Add(new ActivatingFilter<NotificationsUserPart>("User"));

            OnActivated<NotificationsUserPart>((context, part) =>
            {
                part.RecentNotificationEntriesField.Loader(() =>
                    {
                        var serializedEntries = part.Retrieve<string>("RecentNotificationEntriesSerialized");
                        if (string.IsNullOrEmpty(serializedEntries)) return Enumerable.Empty<NotificationUserEntry>();
                        return jsonConverter.Deserialize<IEnumerable<NotificationUserEntry>>(serializedEntries);
                    });

                part.RecentNotificationEntriesField.Setter(entries =>
                    {
                        Argument.ThrowIfNull(entries, "entries");
                        part.Store<string>("RecentNotificationEntriesSerialized", jsonConverter.Serialize(entries));
                        return entries;
                    });
            });
        }
    }
}