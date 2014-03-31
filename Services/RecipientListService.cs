using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;
using RealtyShares.UserNotifications.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtyShares.UserNotifications.Services
{
    public class RecipientListService : IRecipientListService
    {
        private readonly IContentManager _contentManager;


        public RecipientListService(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }
	
			
        public IEnumerable<ContentItem> GetRecipientLists()
        {
            var notificationBatchItems = _contentManager
                .Query(VersionOptions.Published, Constants.RecipientListContentType)
                .OrderByDescending<TitlePartRecord>(record => record.Title)
                .List();

            return notificationBatchItems;
        }

        public IEnumerable<ContentItem> GetOrderedRecipientLists(RecipientListSortBy sortBy)
        {
            var recipientListItems = GetRecipientLists();

            switch (sortBy)
            {
                case RecipientListSortBy.RecentlyCreatedDate: recipientListItems = recipientListItems.OrderByDescending(item => item.As<CommonPart>().CreatedUtc);
                    break;
                case RecipientListSortBy.RecentlyModifiedDate: recipientListItems = recipientListItems.OrderByDescending(item => item.As<CommonPart>().ModifiedUtc);
                    break;
                case RecipientListSortBy.Title:
                    // It is already ordered by title.
                    break;
                default:
                    break;
            }

            return recipientListItems;
        }
    }
}