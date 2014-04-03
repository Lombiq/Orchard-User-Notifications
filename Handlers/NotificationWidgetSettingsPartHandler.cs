using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;
using RealtyShares.UserNotifications.Models;

namespace RealtyShares.UserNotifications.Handlers
{
    /// <summary>
    /// Content handler for <see cref="RealtyShares.UserNotifications.Models.NotificationWidgetSettingsPart"/>.
    /// </summary>
    public class NotificationWidgetSettingsHandler : ContentHandler
    {
        public Localizer T { get; set; }


        public NotificationWidgetSettingsHandler()
        {
            T = NullLocalizer.Instance;

            Filters.Add(new ActivatingFilter<NotificationWidgetSettingsPart>("Site"));
        }


        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site")
                return;

            base.GetItemMetadata(context);

            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("User Notifications")) { Id = "NotificationWidgetSettings" });
        }
    }
}