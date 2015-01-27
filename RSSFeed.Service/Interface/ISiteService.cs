using RSSFeed.Domain;

namespace RSSFeed.Service.Interface
{
    public interface ISiteService : IRepository<Site>, IExtraSiteService
    {}
}