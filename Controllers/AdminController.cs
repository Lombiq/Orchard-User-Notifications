using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
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


        public ActionResult Index(int page = 1, int pageSize = 3)
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageNotifications))
            {
                return new HttpUnauthorizedResult(T("You are not allowed to manage notifications.").Text);
            }

            var notificationBatchItems = _contentManager
                .Query(VersionOptions.AllVersions, Constants.NotificationBatchContentType)
                .OrderByDescending<CommonPartRecord>(record => record.Id)
                .List();

            return GetNotificationBatchListViewResult(notificationBatchItems, page, pageSize);
        }

        [HttpPost, ActionName("Index")]
        [FormValueRequired("submit.Filter")]
        public ActionResult Filter(NotificationBatchListViewModel viewModel)
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageNotifications))
            {
                return new HttpUnauthorizedResult(T("You are not allowed to manage notifications.").Text);
            }

            var notificationBatchItems = _contentManager
                .Query(VersionOptions.AllVersions, Constants.NotificationBatchContentType)
                .OrderByDescending<CommonPartRecord>(record => record.Id)
                .List()
                .Where(item => item.As<CommonPart>().CreatedUtc >= (viewModel.FromDate ?? DateTime.MinValue)
                    && item.As<CommonPart>().CreatedUtc <= (viewModel.ToDate ?? DateTime.MaxValue));

            switch (viewModel.NotificationBatchSortBy)
            {
                case NotificationBatchSortBy.DateSent: notificationBatchItems = notificationBatchItems.OrderBy(item => item.As<CommonPart>().PublishedUtc);
                    break;
                case NotificationBatchSortBy.Title: notificationBatchItems = notificationBatchItems.OrderBy(item => item.As<TitlePart>().Title);
                    break;
                default: notificationBatchItems = notificationBatchItems.OrderBy(item => item.As<CommonPart>().CreatedUtc);
                    break;
            }

            return GetNotificationBatchListViewResult(notificationBatchItems);
        }

        public ActionResult ManageRecipientLists()
        {
            throw new NotImplementedException();
        }


        private ActionResult GetNotificationBatchListViewResult(IEnumerable<ContentItem> notificationBatchItems, int page = 1, int pageSize = 3)
        {
            var viewModel = new NotificationBatchListViewModel();

            if (notificationBatchItems.Any())
            {
                var pager = new Pager(_orchardServices.WorkContext.CurrentSite, new PagerParameters { Page = page, PageSize = pageSize });

                dynamic pagerShape = _orchardServices.New.Pager(pager).TotalItemCount(notificationBatchItems.Count());

                var notificationBatchShapeList = _orchardServices.New.List();
                notificationBatchShapeList.AddRange(notificationBatchItems
                                                   .Skip(pager.GetStartIndex())
                                                   .Take(pager.PageSize)
                                                   .Select(plan => _contentManager.BuildDisplay(plan, "SummaryAdmin")));

                viewModel.NotificationBatchShapes = notificationBatchShapeList;
                viewModel.Pager = pagerShape;
            }

            return View("Index", viewModel);
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