using Orchard.ContentManagement;
using System.Collections.Generic;

namespace RealtyShares.UserNotifications.ViewModels
{
    public enum RecipientListSortBy
    {
        Title = 0,
        RecentlyCreatedDate = 1,
        RecentlyModifiedDate = 2
    }


    public enum RecipientListBulkAction
    {
        None = 0,
        Delete = 1
    }


    public class RecipientListsViewModel
    {
        public IList<RecipientListEntry> RecipientListEntries { get; set; }

        public dynamic Pager { get; set; }

        public RecipientListSortBy RecipientListSortBy { get; set; }

        public RecipientListBulkAction RecipientListBulkAction { get; set; }

        public int PageSize { get; set; }


        public RecipientListsViewModel()
        {
            RecipientListEntries = new List<RecipientListEntry>();
        }
    }


    public class RecipientListEntry
    {
        public string Title { get; set; }

        public int Id { get; set; }

        public ContentItem ContentItem { get; set; }

        public bool IsChecked { get; set; }
    }
}