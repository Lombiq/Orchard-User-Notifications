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
        /// <summary>
        /// Gets the recipient list content items from database.
        /// </summary>
        /// <returns>List of recipient list content items</returns>
        IEnumerable<ContentItem> GetRecipientLists();

        /// <summary>
        /// Gets the filtered recipient list content items from database.
        /// </summary>
        /// <param name="sortBy">Sorting mode of the recipient list content items</param>
        /// <returns>List of filtered recipient list content items</returns>
        IEnumerable<ContentItem> GetOrderedRecipientLists(RecipientListSortBy sortBy);
    }
}