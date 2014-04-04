using Orchard;
using Orchard.Collections;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;
using Orchard.Indexing;
using Orchard.Search.Models;
using Orchard.Search.Services;
using Orchard.Settings;
using Orchard.UI.Navigation;
using RealtyShares.UserNotifications.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RealtyShares.UserNotifications.Services
{
    public class NotificationBatchService : INotificationBatchService
    {
        private readonly IContentManager _contentManager;
        private readonly ISearchService _searchService;
        private readonly IOrchardServices _orchardServices;
        private readonly ISiteService _siteService;


        public NotificationBatchService(IOrchardServices orchardServices, IContentManager contentManager, 
            ISearchService searchService, ISiteService siteService)
        {
            _contentManager = contentManager;
            _searchService = searchService;
            _orchardServices = orchardServices;
            _siteService = siteService;
        }
	
			
        public IEnumerable<ContentItem> GetNotificationBatches()
        {
            var notificationBatchItems = _contentManager
                .Query(VersionOptions.Latest, Constants.NotificationBatchContentType)
                .OrderByDescending<CommonPartRecord>(record => record.Id)
                .List();

            return notificationBatchItems;
        }

        public IEnumerable<ContentItem> GetFilteredNotificationBatches(string keywords, DateTime? fromDate, DateTime? toDate, NotificationBatchSortBy sortBy)
        {
            var pager = new Pager(_siteService.GetSiteSettings(), new PagerParameters());
            var searchSettingsPart = _orchardServices.WorkContext.CurrentSite.As<SearchSettingsPart>();

            IEnumerable<ISearchHit> searchHits = new PageOfItems<ISearchHit>(new ISearchHit[] { });

            searchHits = _searchService.Query(keywords, pager.Page, pager.PageSize,
                                              _orchardServices.WorkContext.CurrentSite.As<SearchSettingsPart>().Record.FilterCulture,
                                              searchSettingsPart.SearchIndex,
                                              searchSettingsPart.SearchedFields,
                                              searchHit => searchHit);

            var notificationBatchItems = _contentManager
                .Query(VersionOptions.Latest, Constants.NotificationBatchContentType)
                .ForContentItems(searchHits.Select(hit => hit.ContentItemId));

            if (fromDate != null && toDate != null)
            {
                notificationBatchItems = notificationBatchItems
                    .Where<CommonPartRecord>(record => record.CreatedUtc >= fromDate.Value && record.CreatedUtc <= toDate.Value);
            }

            switch (sortBy)
            {
                case NotificationBatchSortBy.DateSent: notificationBatchItems = notificationBatchItems
                    .OrderByDescending<CommonPartRecord>(record => record.PublishedUtc);
                    break;
                case NotificationBatchSortBy.Title: notificationBatchItems = notificationBatchItems
                    .OrderBy<TitlePartRecord>(record => record.Title);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("sortBy");
            }

            return notificationBatchItems.List();
        }
    }
}