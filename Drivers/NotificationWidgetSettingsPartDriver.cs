using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using RealtyShares.UserNotifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtyShares.UserNotifications.Drivers
{
    public class NotificationWidgetSettingsPartDriver : ContentPartDriver<NotificationWidgetSettingsPart>
    {
        protected override DriverResult Editor(NotificationWidgetSettingsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_NotificationWidgetSettings_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts.NotificationWidgetSettings",
                    Model: part,
                    Prefix: Prefix))
                    .OnGroup("NotificationWidgetSettings");
        }

        protected override DriverResult Editor(NotificationWidgetSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}