using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using RSSFeed.Domain;

namespace RSSFeed.Service.Interface
{
    public class CacheService : ICache
    {
        private IList<Site> _sites;
        private const string Sitelist = "siteList";
        public CacheService()
        {
            if (HttpContext.Current.Cache[Sitelist] == null)
            {
                _sites = new List<Site>();
                HttpContext.Current.Cache.Insert(Sitelist, _sites, null, DateTime.Now.AddMinutes(120), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }

        }
        public void Remove(Site site)
        {
            if (HttpContext.Current.Cache[Sitelist] != null)
            {
                _sites = (IList<Site>)HttpContext.Current.Cache[Sitelist];

                _sites.Remove(site);
            }


        }

        public void Remove(Group group)
        {
            throw new NotImplementedException();
        }

        public void Add(IList<Group> list)
        {
            throw new NotImplementedException();
        }

        public void Add(Site site)
        {
            if (HttpContext.Current.Cache[Sitelist] != null)
            {
                _sites = (IList<Site>)HttpContext.Current.Cache[Sitelist];
                Remove(site);
                _sites.Add(site);
            }
        }

        public void Add(Group group)
        {
            throw new NotImplementedException();
        }

        public void Add(IList<Site> sites)
        {
            if (HttpContext.Current.Cache[Sitelist] != null)
            {
                _sites = (IList<Site>)HttpContext.Current.Cache[Sitelist];
                foreach (var site in sites)
                {
                    Remove(site);
                    _sites.Add(site);
                }
            }
        }

        public Site GetSiteById(int id)
        {
            if (HttpContext.Current.Cache[Sitelist] != null)
            {
                _sites = (IList<Site>)HttpContext.Current.Cache[Sitelist];
                return _sites.FirstOrDefault(x => x.Id == id);
            }

            return null;
        }

        public Site GetSiteByItemId(string uniqId)
        {
            if (HttpContext.Current.Cache[Sitelist] != null)
            {
                _sites = (IList<Site>)HttpContext.Current.Cache[Sitelist];

                return _sites.FirstOrDefault(x => x.Channels.Any(y => y.Items.Any(i => i.UniqId == uniqId)));


            }

            return null;
        }

        public Group GetGroupById(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Site> GetAllSites()
        {
            if (HttpContext.Current.Cache[Sitelist] != null)
            {
                _sites = (IList<Site>)HttpContext.Current.Cache[Sitelist];
                return _sites;
            }

            return null;
        }

        public IList<Group> GetAllGroups()
        {
            throw new NotImplementedException();
        }


        public Channel GetChannelById(int id)
        {
            if (HttpContext.Current.Cache[Sitelist] != null)
            {
                _sites = (IList<Site>)HttpContext.Current.Cache[Sitelist];
                return _sites.SelectMany(parent => parent.Channels).FirstOrDefault(x => x.Id == id);

            }

            return null;
        }

        public void Clear()
        {
            if (HttpContext.Current.Cache[Sitelist] != null)
            {
                _sites = (IList<Site>)HttpContext.Current.Cache[Sitelist];
                _sites.Clear();

            }
        }


        public void Remove(Channel channel)
        {
            if (HttpContext.Current.Cache[Sitelist] != null)
            {
                _sites = (IList<Site>)HttpContext.Current.Cache[Sitelist];
                Channel ch = _sites.SelectMany(parent => parent.Channels).FirstOrDefault(x => x.Id == channel.Id);
                if (ch != null) ch.Items.Clear();
            }
        }
    }
}