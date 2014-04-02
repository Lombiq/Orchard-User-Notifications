using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Common.Utilities;
using Orchard.Core.Title.Models;
using Orchard.Roles.Models;
using Orchard.Security;

namespace RealtyShares.UserNotifications.Models
{
    /// <summary>
    /// Various types of defining recipients for a notification batch.
    /// </summary>
    public enum NotificationBatchRecipientsType
    {
        /// <summary>
        /// Recipients are all users of a certain role.
        /// </summary>
        Role,
        /// <summary>
        /// Recipients are the users defined in a recipient list.
        /// </summary>
        RecipientList,
        /// <summary>
        /// Recipients are given through ad-hoc configurtaion when sending the batch.
        /// </summary>
        AdhocRecipients
    }


    /// <summary>
    /// Stores custom data of batches notifications.
    /// </summary>
    public class NotificationBatchPart : ContentPart
    {
        private readonly LazyField<NotificationBatchRecipientsType> _recipientsType = new LazyField<NotificationBatchRecipientsType>();
        internal LazyField<NotificationBatchRecipientsType> RecipientsTypeField { get { return _recipientsType; } }
        public NotificationBatchRecipientsType RecipientsType
        {
            get { return _recipientsType.Value; }
            set { _recipientsType.Value = value; }
        }

        private readonly LazyField<IEnumerable<RoleRecord>> _availableRoles = new LazyField<IEnumerable<RoleRecord>>();
        internal LazyField<IEnumerable<RoleRecord>> AvailableRolesField { get { return _availableRoles; } }
        public IEnumerable<RoleRecord> AvailableRoles { get { return _availableRoles.Value; } }

        public int RecipientsRoleId
        {
            get { return this.Retrieve(x => x.RecipientsRoleId); }
            set { this.Store(x => x.RecipientsRoleId, value); }
        }

        private readonly LazyField<IEnumerable<IContent>> _availableRecipientLists = new LazyField<IEnumerable<IContent>>();
        internal LazyField<IEnumerable<IContent>> AvailableRecipientListsField { get { return _availableRecipientLists; } }
        public IEnumerable<IContent> AvailableRecipientLists { get { return _availableRecipientLists.Value; } }

        public int RecipientListId
        {
            get { return this.Retrieve(x => x.RecipientListId); }
            set { this.Store(x => x.RecipientListId, value); }
        }

        private readonly LazyField<IContent> _recipientList = new LazyField<IContent>();
        internal LazyField<IContent> RecipientListField { get { return _recipientList; } }
        public IContent RecipientList
        {
            get { return _recipientList.Value; }
            set { _recipientList.Value = value; }
        }

        public string RecipientNames
        {
            get { return this.Retrieve(x => x.RecipientNames); }
            set { this.Store(x => x.RecipientNames, value); }
        }

        private readonly LazyField<IEnumerable<int>> _actualRecipientIds = new LazyField<IEnumerable<int>>();
        internal LazyField<IEnumerable<int>> ActualRecipientIdsField { get { return _actualRecipientIds; } }
        /// <summary>
        /// Gets the ids of the users who are the actual recipients of this notification batch, regardless of how recipients where selected.
        /// </summary>
        public IEnumerable<int> ActualRecipientIds { get { return _actualRecipientIds.Value; } }

        private readonly LazyField<IEnumerable<IUser>> _actualRecipients = new LazyField<IEnumerable<IUser>>();
        internal LazyField<IEnumerable<IUser>> ActualRecipientsField { get { return _actualRecipients; } }
        /// <summary>
        /// Gets the users who are the actual recipients of this notification batch, regardless of how recipients where selected.
        /// </summary>
        public IEnumerable<IUser> ActualRecipients { get { return _actualRecipients.Value; } }
    }
}