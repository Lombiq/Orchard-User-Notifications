using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Core.Common.Utilities;

namespace RealtyShares.UserNotifications.Models
{
    /// <summary>
    /// Notification widget settings content part to set notification count to display on the UI.
    /// </summary>
    public class NotificationWidgetSettingsPart : ContentPart
    {
        public int NotificationCount
        {
            get { return this.Retrieve(x => x.NotificationCount); }
            set { this.Store(x => x.NotificationCount, value); }
        }
    }
}