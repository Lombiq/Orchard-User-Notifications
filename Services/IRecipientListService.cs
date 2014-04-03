using Orchard;
using Orchard.ContentManagement;
using RealtyShares.UserNotifications.ViewModels;
using System.Collections.Generic;

namespace RealtyShares.UserNotifications.Services
{
    /// <summary>
    /// Queries Recipient Lists with the given parameters.
    /// </summary>
    public interface IRecipientListService : IDependency
    {
        IEnumerable<ContentItem> GetRecipientLists();

        IEnumerable<ContentItem> GetOrderedRecipientLists(RecipientListSortBy sortBy);
    }
}