using System.Collections.Generic;
using RSSFeed.Domain;

namespace RSSFeed.Service.Interface
{
    public interface ICache
    {
        void Clear();
        void Remove(Channel channel);       
        void Remove(Site site);
        void Remove(Group group);
        void Add(IList<Group> feedEntity);
        void Add(Site site);
        void Add(Group group);
        void Add(IList<Site> sites);
        Site GetSiteById(int id);
        Site GetSiteByItemId(string uniqId);
        Channel GetChannelById(int id);
        Group GetGroupById(int id);
        IList<Site> GetAllSites();
        IList<Group> GetAllGroups();
    }
}