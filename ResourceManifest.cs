using Orchard.UI.Resources;

namespace RealtyShares.UserNotifications
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();


            manifest.DefineStyle("RealtyShares.UserNotifications.NotificationBatchList")
                .SetUrl("realtyshares-usernotifications-notificationbatchlist.css")
                .SetDependencies("jQueryUI_DatePicker");
        }
    }
}