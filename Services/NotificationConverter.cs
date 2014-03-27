using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;
using Orchard.Security;
using Orchard.Tokens;
using RealtyShares.UserNotifications.Models;

namespace RealtyShares.UserNotifications.Services
{
    public class NotificationConverter : INotificationConverter
    {
        private readonly ITokenizer _tokenizer;


        public NotificationConverter(ITokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }
        
    
        public INotification ConvertBatchToNotification(IContent notificationBatch, IUser recipient)
        {
            return new Notification
            {
                Id = notificationBatch.ContentItem.Id,
                ContentItem = notificationBatch.ContentItem,
                TitleLazy = new Lazy<string>(() => _tokenizer.Replace(notificationBatch.As<TitlePart>().Title, new { User = recipient })),
                BodyLazy = new Lazy<string>(() => _tokenizer.Replace(notificationBatch.As<BodyPart>().Text, new { User = recipient }))
            };
        }


        private class Notification : INotification
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