using Orchard.ContentManagement.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using RealtyShares.UserNotifications.Models;
using Orchard.Validation;
using RealtyShares.UserNotifications.Services;
using Orchard.Security;
using Orchard.Roles.Models;
using Orchard.Data;
using Orchard.Users.Models;
using Orchard.Roles.Services;

namespace RealtyShares.UserNotifications.Handlers
{
    /// <summary>
    /// Content handler for <see cref="RealtyShares.UserNotifications.Models.NotificationBatchPart"/>.
    /// </summary>
    public class NotificationBatchPartHandler : ContentHandler
    {
        public NotificationBatchPartHandler(
            IContentManager contentManager,
            Lazy<IRoleService> roleServiceLazy,
            Lazy<IRepository<UserRolesPartRecord>> userRolesRepositoryLazy)
        {
            OnActivated<NotificationBatchPart>((context, part) =>
            {
                part.RecipientsTypeField.Loader(() =>
                    {
                        var recipientsType = part.Retrieve<string>("RecipientsType");
                        if (string.IsNullOrEmpty(recipientsType)) return NotificationBatchRecipientsType.Role;
                        return (NotificationBatchRecipientsType)Enum.Parse(typeof(NotificationBatchRecipientsType), recipientsType);
                    });

                part.RecipientsTypeField.Setter(recipientsType =>
                    {
                        part.Store("RecipientsType", recipientsType.ToString());
                        return recipientsType;
                    });

                part.AvailableRolesField.Loader(() => roleServiceLazy.Value.GetRoles());

                part.RecipientListField.Loader(() => contentManager.Get(part.RecipientListId));

                part.AvailableRecipientListsField.Loader(() => contentManager.Query(Constants.RecipientListContentType).List());

                part.RecipientListField.Setter(recipientsList =>
                    {
                        part.RecipientListId = recipientsList != null ? recipientsList.ContentItem.Id : 0;
                        return recipientsList;
                    });

                part.ActualRecipientIdsField.Loader(() =>
                    {
                        IEnumerable<int> recipientIds;

                        switch (part.RecipientsType)
                        {
                            case NotificationBatchRecipientsType.Role:
                                recipientIds = userRolesRepositoryLazy.Value.Table
                                    .Where(record => record.Role.Id == part.RecipientsRoleId)
                                    .Select(record => record.UserId);
                                break;
                            case NotificationBatchRecipientsType.RecipientList:
                                recipientIds = part.RecipientList.As<NotificationRecipientsPart>().RecipientIds;
                                break;
                            case NotificationBatchRecipientsType.AdhocRecipients:
                                recipientIds = part.As<NotificationRecipientsPart>().RecipientIds;
                                break;
                            default:
                                throw new InvalidOperationException("Not supported recipient type.");
                        }

                        return recipientIds;
                    });

                part.ActualRecipientsField.Loader(() => contentManager.GetMany<IUser>(part.ActualRecipientIds, VersionOptions.Published, new QueryHints().ExpandParts<UserPart>()));
            });
        }
    }
}