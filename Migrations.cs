using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using RealtyShares.UserNotifications.Models;

namespace RealtyShares.UserNotifications
{
    /// <summary>
    /// Migrations for creating the necessary content type for Notifications.
    /// </summary>
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterTypeDefinition(Constants.NotificationBatchContentType,
                cfg => cfg
                    .Draftable()
                    .WithPart("TitlePart")
                    .WithPart("CommonPart")
                    .WithPart("BodyPart")
                    .WithPart(typeof(NotificationBatchPart).Name)
                    .WithPart(typeof(NotificationRecipientsPart).Name)
                    .WithPart("PublishLaterPart")
                );

            ContentDefinitionManager.AlterTypeDefinition(Constants.RecipientListContentType,
                cfg => cfg
                    .WithPart("TitlePart")
                    .WithPart("CommonPart", part => part.WithSetting("OwnerEditorSettings.ShowOwnerEditor", "False"))
                    .WithPart(typeof(NotificationRecipientsPart).Name)
                );


            return 1;
        }
    }
}