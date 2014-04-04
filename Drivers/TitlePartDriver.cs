using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Title.Models;
using Orchard.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtyShares.UserNotifications.Drivers
{
    public class TitlePartDriver : ContentPartDriver<TitlePart>
    {
        protected override string Prefix { get { return "Title"; } }


        protected override DriverResult Editor(TitlePart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Title_Edit_Tokenized", () => 
                shapeHelper.EditorTemplate(TemplateName: "Parts.Title.Tokenized", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(TitlePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            return Editor(part, shapeHelper);
        }
    }
}