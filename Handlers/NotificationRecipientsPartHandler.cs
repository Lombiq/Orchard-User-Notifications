using Orchard.ContentManagement.Handlers;
using Orchard.Services;
using RealtyShares.UserNotifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Validation;
using Orchard.Security;
using Orchard.Users.Models;

namespace RealtyShares.UserNotifications.Handlers
{
    /// <summary>
    /// Content handler for <see cref="RealtyShares.UserNotifications.Models.NotificationRecipientsPart"/>.
    /// </summary>
    public class NotificationRecipientsPartHandler : ContentHandler
    {
        public NotificationRecipientsPartHandler(Lazy<IJsonConverter> jsonConverterLazy, IContentManager contentManager)
        {
            OnActivated<NotificationRecipientsPart>((context, part) =>
            {
                part.RecipientIdsField.Loader(() =>
                {
                    var serializedEntries = part.Retrieve<string>("RecipientsIdsSerialized");
                    if (string.IsNullOrEmpty(serializedEntries)) return Enumerable.Empty<int>();
                    return jsonConverterLazy.Value.Deserialize<IEnumerable<int>>(serializedEntries);
                });

                part.RecipientIdsField.Setter(recipients =>
                {
                    Argument.ThrowIfNull(recipients, "recipients");
                    part.Store<string>("RecipientsIdsSerialized", jsonConverterLazy.Value.Serialize(recipients));
                    return recipients;
                });

                part.RecipientsField.Loader(() => contentManager.GetMany<IUser>(part.RecipientIds, VersionOptions.Published, new QueryHints().ExpandParts<UserPart>()));

                part.RecipientsField.Setter(recipients =>
                    {
                        Argument.ThrowIfNull(recipients, "recipients");
                        part.RecipientIds = recipients.Select(user => user.ContentItem.Id);
                        return recipients;
                    });

                part.RecipientNamesField.Loader(() => string.Join(", ", part.Recipients.Select(user => user.UserName)));
                
                part.RecipientNamesField.Setter(names =>
                    {
                        if (string.IsNullOrEmpty(names)) return names;

                        var namesArray = names
                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(name => name.Trim().ToLowerInvariant())
                            .ToArray();

                        // To be very thoughtful this could use IMembershipService instead, but that would mean n queries instead of 1.
                        part.Recipients = contentManager
                            .Query("User")
                            .Where<UserPartRecord>(record => namesArray.Contains(record.NormalizedUserName))
                            .List<IUser>();

                        return names;
                    });
            });
        }
    }
}