using RSSFeed.Domain;

namespace RSSFeed.Service.Interface
{
    public interface IExtraSiteService
    {
        bool AddNewUrl(string uri);
        Site MakeNewSite(string uri);
    }
}