using Orchard;
using Orchard.Localization;
using Orchard.UI.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtyShares.UserNotifications
{
    /// <summary>
    /// Menu provider for the module's backend navigation.
    /// </summary>
    public class AdminMenu : Component, INavigationProvider
    {
        public string MenuName { get { return "admin"; } }


        public void GetNavigation(NavigationBuilder builder)
        {
            builder
                .Add(T("Notifications"), "5", item => item.Action("Index", "Admin", new { area = "RealtyShares.UserNotifications" }).Permission(Permissions.ManageNotifications));
        }
    }
}