using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using RealtyShares.UserNotifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtyShares.UserNotifications.Drivers
{
    public class NotificationBatchPartDriver : ContentPartDriver<NotificationBatchPart>
    {
        protected override DriverResult Editor(NotificationBatchPart part, dynamic shapeHelper)
        {
            var sdf = part.ActualRecipientIds;
            return ContentShape("Parts_NotificationBatch_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts.NotificationBatch",
                    Model: part,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor(NotificationBatchPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}