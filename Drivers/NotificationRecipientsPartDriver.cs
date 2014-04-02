using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using RealtyShares.UserNotifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Security;
using Orchard.Users.Models;
using Orchard.Localization;

namespace RealtyShares.UserNotifications.Drivers
{
    public class NotificationRecipientsPartDriver : ContentPartDriver<NotificationRecipientsPart>
    {
        private readonly IContentManager _contentManager;

        public Localizer T { get; set; }


        public NotificationRecipientsPartDriver(IContentManager contentManager)
        {
            _contentManager = contentManager;

            T = NullLocalizer.Instance;
        }


        protected override DriverResult Editor(NotificationRecipientsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_NotificationRecipients_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts.NotificationRecipients",
                    Model: part,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor(NotificationRecipientsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
                if (!string.IsNullOrEmpty(part.RecipientNames))
                {
                    var recipientNames = part.RecipientNames
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (recipientNames.Any())
                    {
                        var normalizedRecipientNames = recipientNames.Select(name => name.Trim().ToLowerInvariant()).ToArray();

                        var users = _contentManager
                            .Query("User")
                            .Where<UserPartRecord>(record => normalizedRecipientNames.Contains(record.NormalizedUserName))
                            .List();

                        if (users.Count() == normalizedRecipientNames.Length)
                        {
                            part.Recipients = users.Select(user => user.As<IUser>());
                        }
                        else
                        {
                            var usersPerName = users.ToDictionary(user => user.As<UserPart>().NormalizedUserName);
                            for (int i = 0; i < normalizedRecipientNames.Length; i++)
                            {
                                if (!usersPerName.ContainsKey(normalizedRecipientNames[i]))
                                {
                                    updater.AddModelError("RecipientNameInvalid:" + normalizedRecipientNames[i], T("There is no user with the name {0}.", recipientNames[i]));
                                }
                            }
                        }
                    }
                    else
                    {
                        part.Recipients = Enumerable.Empty<IUser>();
                    }
                }
                else
                {
                    part.Recipients = Enumerable.Empty<IUser>();
                }
            }

            return Editor(part, shapeHelper);
        }
    }
}