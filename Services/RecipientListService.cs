using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;
using RealtyShares.UserNotifications.ViewModels;
using System;
using System.Collections.Generic;

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
                .Query(VersionOptions.Latest, Constants.RecipientListContentType)
                .OrderBy<TitlePartRecord>(record => record.Title)
                .List();

            return notificationBatchItems;
        }

        public IEnumerable<ContentItem> GetOrderedRecipientLists(RecipientListSortBy sortBy)
        {
            var recipientListItems = _contentManager.Query(VersionOptions.Latest, Constants.RecipientListContentType);

            switch (sortBy)
            {
                case RecipientListSortBy.RecentlyCreatedDate: recipientListItems = recipientListItems
                    .OrderByDescending<CommonPartRecord>(record => record.CreatedUtc);
                    break;
                case RecipientListSortBy.RecentlyModifiedDate: recipientListItems = recipientListItems
                    .OrderByDescending<CommonPartRecord>(record => record.ModifiedUtc);
                    break;
                case RecipientListSortBy.Title: recipientListItems = recipientListItems
                    .OrderBy<TitlePartRecord>(record => record.Title);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("sortBy");
            }

            return recipientListItems.List();
        }
    }
}