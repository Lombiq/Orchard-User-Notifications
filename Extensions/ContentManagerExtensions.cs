using System;
using System.Collections.Generic;
using System.Linq;

namespace Orchard.ContentManagement
{
    internal static class ContentManagerExtensions
    {
        public static Func<IEnumerable<dynamic>> GetShapesFactory(this IContentManager contentManager, IEnumerable<ContentItem> contentItems, string displayType = "", string groupId = "")
        {
            return () => contentItems.Select(item => contentManager.BuildDisplay(item, displayType, groupId));
        }
    }
}