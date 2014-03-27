using Orchard.ContentManagement;
using Orchard.Core.Common.Utilities;
using Orchard.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtyShares.UserNotifications.Models
{
    /// <summary>
    /// Content part for handling a set of recipients, who are users, for notifications.
    /// </summary>
    public class NotificationRecipientsPart : ContentPart
    {
        private readonly LazyField<IEnumerable<int>> _recipientIds = new LazyField<IEnumerable<int>>();
        internal LazyField<IEnumerable<int>> RecipientIdsField { get { return _recipientIds; } }
        public IEnumerable<int> RecipientIds
        {
            get { return _recipientIds.Value; }
            set { _recipientIds.Value = value; }
        }

        private readonly LazyField<IEnumerable<IUser>> _recipients = new LazyField<IEnumerable<IUser>>();
        internal LazyField<IEnumerable<IUser>> RecipientsField { get { return _recipients; } }
        public IEnumerable<IUser> Recipients
        {
            get { return _recipients.Value; }
            set { _recipients.Value = value; }
        }

        private readonly LazyField<string> _recipientNames = new LazyField<string>();
        internal LazyField<string> RecipientNamesField { get { return _recipientNames; } }
        public string RecipientNames
        {
            get { return _recipientNames.Value; }
            set { _recipientNames.Value = value; }
        }
    }
}