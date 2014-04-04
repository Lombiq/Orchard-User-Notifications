using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;
using Orchard.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtyShares.UserNotifications.Drivers
{
    public class BodyPartDriver : ContentPartDriver<BodyPart>
    {
        protected override string Prefix { get { return "Body"; } }


        protected override DriverResult Editor(BodyPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Common_Body_Edit_Tokenized", () => 
                shapeHelper.EditorTemplate(TemplateName: "Parts.Common.Body.Tokenized", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(BodyPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            return Editor(part, shapeHelper);
        }
    }
}