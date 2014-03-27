using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using RealtyShares.UserNotifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtyShares.UserNotifications.Drivers
{
    public class NotificationRecipientsPartDriver : ContentPartDriver<NotificationRecipientsPart>
    {
        protected override DriverResult Editor(NotificationRecipientsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_UserRecipients_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts.UserRecipients",
                    Model: part,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor(NotificationRecipientsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}