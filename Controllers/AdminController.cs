using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.UI.Admin;
using RealtyShares.UserNotifications.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealtyShares.UserNotifications.Controllers
{
    [Admin]
    public class AdminController : Controller, IUpdateModel
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
                .Query(VersionOptions.AllVersions, Constants.NotificationBatchContentType)
                .OrderByDescending<CommonPartRecord>(record => record.Id)
                .List()
                .ToList();

            var viewModel = new NotificationBatchListViewModel();

            var notificationBatchShapes = _contentManager.GetShapesFactory(notificationItems, "SummaryAdmin");

            viewModel.NotificationBatchShapes = notificationBatchShapes;

            return View(viewModel);
        }

        [HttpPost, ActionName("Index")]
        [FormValueRequired("submit.Filter")]
        public ActionResult Filter(NotificationBatchListViewModel viewModel)
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageNotifications))
            {
                return new HttpUnauthorizedResult(T("You are not allowed to manage notifications.").Text);
            }

            var notificationItems = _contentManager
                .Query(VersionOptions.AllVersions, Constants.NotificationBatchContentType)
                .OrderByDescending<CommonPartRecord>(record => record.Id)
                .List()
                .Where(item => item.As<CommonPart>().CreatedUtc >= (viewModel.FromDate ?? DateTime.MinValue)
                    && item.As<CommonPart>().CreatedUtc <= (viewModel.ToDate ?? DateTime.MaxValue));

            switch (viewModel.NotificationBatchSortBy)
            {
                case NotificationBatchSortBy.DateSent: notificationItems = notificationItems.OrderBy(item => item.As<CommonPart>().PublishedUtc);
                    break;
                case NotificationBatchSortBy.Title: notificationItems = notificationItems.OrderBy(item => item.As<TitlePart>().Title);
                    break;
                default: notificationItems = notificationItems.OrderBy(item => item.As<CommonPart>().CreatedUtc);
                    break;
            }

            var notificationBatchShapes = _contentManager.GetShapesFactory(notificationItems, "SummaryAdmin");

            viewModel.NotificationBatchShapes = notificationBatchShapes;

            return View(viewModel);
        }

        public ActionResult ManageRecipientLists()
        {
            throw new NotImplementedException();
        }


        #region IUpdateModel Members

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties,
            string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }

        #endregion
    }
}