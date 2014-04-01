using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Logging;
using RealtyShares.UserNotifications.Models;
using System.Web.Mvc;
using System.Linq;

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

            // Commented out for testing purposes.
            //var notificationUserPart = _wca.GetContext().CurrentUser.As<NotificationsUserPart>();
            //var relevantNotificationEntry = notificationUserPart.RecentNotificationEntries.FirstOrDefault(entry => entry.NotificationId == id);

            //if (relevantNotificationEntry == null)
            //{
            //    return false;
            //}

            //relevantNotificationEntry.IsRead = true;

            return true;
        }
    }
}