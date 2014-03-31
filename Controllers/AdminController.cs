using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using RealtyShares.UserNotifications.Services;
using RealtyShares.UserNotifications.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RealtyShares.UserNotifications.Controllers
{
    [Admin]
    public class AdminController : Controller, IUpdateModel
    {
        private const int DefaultPageSizeForNotificationBatchList = 10;
        private const int DefaultPageSizeForRecipientLists = 10;


        private readonly IContentManager _contentManager;
        private readonly IOrchardServices _orchardServices;
        private readonly INotificationBatchService _notificationBatchService;
        private readonly IRecipientListService _recipientListService;
        

        public Localizer T { get; set; }

        public ILogger Logger { get; set; }


        public AdminController(IOrchardServices orchardServices, INotificationBatchService notificationBatchService, 
            IRecipientListService recipientListService)
        {
            _orchardServices = orchardServices;
            _contentManager = orchardServices.ContentManager;
            _notificationBatchService = notificationBatchService;
            _recipientListService = recipientListService;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }


        public ActionResult Index(int page = 1, int pageSize = DefaultPageSizeForNotificationBatchList)
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageNotifications))
            {
                return new HttpUnauthorizedResult(T("You are not allowed to manage notifications.").Text);
            }

            var notificationBatchItems = _notificationBatchService.GetNotificationBatches();

            return GetNotificationBatchListViewResult(notificationBatchItems, page, pageSize);
        }

        [HttpPost, ActionName("Index")]
        [FormValueRequired("submit.Filter")]
        public ActionResult FilterNotificationBatches(NotificationBatchListViewModel viewModel)
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageNotifications))
            {
                return new HttpUnauthorizedResult(T("You are not allowed to manage notifications.").Text);
            }

            var keywords = (viewModel.Keywords ?? String.Empty).Split(' ', ',');

            var notificationBatchItems = _notificationBatchService.GetFilteredNotificationBatches(keywords, 
                viewModel.FromDate ?? DateTime.MinValue, viewModel.ToDate ?? DateTime.MaxValue, viewModel.NotificationBatchSortBy);

            return GetNotificationBatchListViewResult(notificationBatchItems);
        }

        public ActionResult ManageRecipientLists(int page = 1, int pageSize = DefaultPageSizeForRecipientLists)
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageNotifications))
            {
                return new HttpUnauthorizedResult(T("You are not allowed to manage notifications.").Text);
            }

            var recipientLists = _recipientListService.GetRecipientLists();

            return GetRecipientListViewResult(recipientLists, page, pageSize);
        }

        [HttpPost, ActionName("ManageRecipientLists")]
        [FormValueRequired("submit.BulkAction")]
        public ActionResult ManageRecepientListsBulkAction(RecipientListsViewModel viewModel)
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageNotifications))
            {
                return new HttpUnauthorizedResult(T("You are not allowed to manage notifications.").Text);
            }

            var selectedEntries = viewModel.RecipientListEntries.Where(entry => entry.IsChecked);

            if (selectedEntries.Any())
            {
                switch (viewModel.RecipientListBulkAction)
                {
                    case RecipientListBulkAction.None:
                        break;
                    case RecipientListBulkAction.Delete:
                        {
                            foreach (var entryItem in selectedEntries)
                            {
                                _contentManager.Remove(_contentManager.Get(entryItem.Id));
                            }

                            _orchardServices.Notifier.Add(NotifyType.Information, T("Items were successfully removed."));
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                _orchardServices.Notifier.Add(NotifyType.Warning, T("Select a recipient list first."));
            }

            return RedirectToAction("ManageRecipientLists");
        }

        [HttpPost, ActionName("ManageRecipientLists")]
        [FormValueRequired("submit.Filter")]
        public ActionResult FilterRecipientLists(RecipientListsViewModel viewModel)
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageNotifications))
            {
                return new HttpUnauthorizedResult(T("You are not allowed to manage notifications.").Text);
            }

            var recipientListItems = _recipientListService.GetOrderedRecipientLists(viewModel.RecipientListSortBy);

            return GetRecipientListViewResult(recipientListItems, 1, viewModel.PageSize);
        }


        private ActionResult GetNotificationBatchListViewResult(IEnumerable<ContentItem> notificationBatchItems, int page = 1, int pageSize = DefaultPageSizeForNotificationBatchList)
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
                                                   .Select(item => _contentManager.BuildDisplay(item, "SummaryAdmin")));

                viewModel.NotificationBatchShapes = notificationBatchShapeList;
                viewModel.Pager = pagerShape;
            }

            return View("Index", viewModel);
        }

        private ActionResult GetRecipientListViewResult(IEnumerable<ContentItem> recipientLists, int page = 1, int pageSize = DefaultPageSizeForRecipientLists)
        {
            var viewModel = new RecipientListsViewModel();

            if (recipientLists.Any())
            {
                var pagerParameters = pageSize == 0 ? new PagerParameters { Page = page, PageSize = null } : new PagerParameters { Page = page, PageSize = pageSize };
                var pager = new Pager(_orchardServices.WorkContext.CurrentSite, pagerParameters);

                dynamic pagerShape = _orchardServices.New.Pager(pager).TotalItemCount(recipientLists.Count());

                var recipientListEntryItems = new List<RecipientListEntry>();
                recipientListEntryItems.AddRange(recipientLists
                                                   .Skip(pager.GetStartIndex())
                                                   .Take(pager.PageSize)
                                                   .Select(item => new RecipientListEntry 
                                                        { 
                                                            Title = item.As<TitlePart>().Title, 
                                                            IsChecked = false,
                                                            Id = item.Id,
                                                            ContentItem = item
                                                        }));

                viewModel.RecipientListEntries = recipientListEntryItems;
                viewModel.Pager = pagerShape;
            }

            return View("ManageRecipientLists", viewModel);
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