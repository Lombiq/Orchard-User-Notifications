using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Logging;
using RealtyShares.UserNotifications.Models;
using System.Web.Mvc;

namespace RealtyShares.UserNotifications.Controllers
{
    public class NotificationWidgetController : Controller
    {
        private readonly IContentManager _contentManager;
        private readonly IWorkContextAccessor _wca;
        

        public Localizer T { get; set; }

        public ILogger Logger { get; set; }


        public NotificationWidgetController(IContentManager contentManager, IWorkContextAccessor wca)
        {
            _contentManager = contentManager;
            _wca = wca;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }


        [HttpPost]
        public bool RemoveNotification(int id)
        {
            if (_wca.GetContext().CurrentUser == null)
            {
                return false;
            }

            var notificationBatch = _contentManager.Get(id);

            if (notificationBatch == null)
            {
                return false;
            }

            if (notificationBatch.ContentType != Constants.NotificationBatchContentType)
            {
                return false;
            }

            var notificationUserPart = _wca.GetContext().CurrentUser.As<NotificationsUserPart>();

            // Commented out for testing purposes.
            //notificationUserPart.LastProcessedNotificationId = id;

            return true;
        }
    }
}