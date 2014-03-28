using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Localization;
using Orchard.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealtyShares.UserNotifications.Controllers
{
    public class AdminController : Controller
    {
        private readonly IContentManager _contentManager;
        private readonly IOrchardServices _orchardServices;


        public Localizer T { get; set; }

        public ILogger Logger { get; set; }


        public AdminController(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
            _contentManager = orchardServices.ContentManager;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }


        public ActionResult Index()
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageNotifications))
            {
                return new HttpUnauthorizedResult(T("You are not allowed to manage notifications.").Text);
            }

            var notificationItems = _contentManager
                .Query(VersionOptions.Published, Constants.NotificationBatchContentType)
                .OrderByDescending<CommonPartRecord>(record => record.Id)
                .List();

            var notificationBatchShapes = _contentManager.GetShapesFactory(notificationItems, "SummaryAdmin");

            var viewModel = _orchardServices.New.ViewModel(NotificationBatchShapes: notificationBatchShapes);

            return View(viewModel);
        }
    }
}