using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using RealtyShares.UserNotifications.Models;
using RealtyShares.UserNotifications.Services;
using System;
using System.Collections.Generic;

namespace RealtyShares.UserNotifications.Handlers
{
    /// <summary>
    /// Content handler for <see cref="RealtyShares.UserNotifications.Models.NotificationWidgetPart"/>.
    /// </summary>
    public class NotificationWidgetPartHandler : ContentHandler
    {
        public NotificationWidgetPartHandler(INotificationsToUserDispatcher notificationDispatcher, IWorkContextAccessor wca)
        {
            OnActivated<NotificationWidgetPart>((context, part) =>
            {
                part.UnreadNotificationsField.Loader(() =>
                    {
                        //var unreadNotifications = notificationDispatcher.GetRecentNotificationsForUser(wca.GetContext().CurrentUser, 50).ToList();

                        // For testing purposes.
                        var unreadNotifications = new List<INotification>()
                        {
                            new TestNotification
                            {
                                Id = 0,
                                ContentItem = null,
                                TitleLazy = new Lazy<string>(() => "Important Notification"),
                                BodyLazy = new Lazy<string>(() => "You have to go to the bathroom soon...")
                            },
                            new TestNotification
                            {
                                Id = 1,
                                ContentItem = null,
                                TitleLazy = new Lazy<string>(() => "Less Important Notification"),
                                BodyLazy = new Lazy<string>(() => "You run out of toilet paper...")
                            }
                        };

                        return unreadNotifications;
                    });
            });
        }


        // For testing purposes.
        private class TestNotification : INotification
        {
            public int Id { get; set; }
            public ContentItem ContentItem { get; set; }
            public Lazy<string> TitleLazy { get; set; }
            public string Title { get { return TitleLazy.Value; } }
            public Lazy<string> BodyLazy { get; set; }
            public string Body { get { return BodyLazy.Value; } }
        }
    }
}