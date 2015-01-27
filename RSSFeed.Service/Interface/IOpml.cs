using System.Collections.Generic;
using System.Web;
using RSSFeed.Domain.opml;

namespace RSSFeed.Service.Interface
{
    public interface IOpml
    {
        void ImportOpml(HttpPostedFileBase fileBase);
        string ExportOpml();
    }
}