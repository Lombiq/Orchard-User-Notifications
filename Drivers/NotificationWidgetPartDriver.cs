using Orchard.ContentManagement.Drivers;
using RealtyShares.UserNotifications.Models;

namespace RealtyShares.UserNotifications.Drivers
{
    public class NotificationWidgetPartDriver : ContentPartDriver<NotificationWidgetPart>
    {
        protected override DriverResult Display(NotificationWidgetPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_NotificationWidget", () => shapeHelper.Parts_NotificationWidget());
        }
    }
}