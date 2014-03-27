using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtyShares.UserNotifications
{
    /// <summary>
    /// Custom permissions for Notification management.
    /// </summary>
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageNotifications = new Permission { Category = "User Notifications", Description = "Manage User Notifications", Name = "RealtyShares.UserNotifications.ManageNotifications" };

        public virtual Feature Feature { get; set; }


        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ManageNotifications
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] { ManageNotifications }
                }
            };
        }
    }
}