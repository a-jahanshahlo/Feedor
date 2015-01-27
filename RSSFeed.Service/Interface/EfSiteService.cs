using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using RSSFeed.Data.Context;
using RSSFeed.Domain;

namespace RSSFeed.Service.Interface
{

    public class EfSiteService : ISiteService
    {
        private readonly IValidationDictionary _validationDictionary;
        readonly IUnitOfWork _uow;
        readonly IDbSet<Site> _items;
        private readonly IUrlService _newSiteService;
        private readonly IChannelService _channelService;
        public EfSiteService(IValidationDictionary validationDictionary, IUnitOfWork unitOfWork)
        {
            _newSiteService = new UrlService();
            _channelService = new EfChannelService(unitOfWork);
            _validationDictionary = validationDictionary;
            _uow = unitOfWork;
            _items = _uow.Set<Site>();
        }
        public IQueryable<Site> AsQueryable()
        {
            return _items.Include(x => x.Group).Include(x => x.Channels).Where(x=>x.IsDeleted==false).Where(x=>x.Channels.Any(y=>y.IsDeleted==false)).AsQueryable();
        }

        public IEnumerable<Site> Find(Expression<Func<Site, bool>> predicate)
        {
            return _items.Include(x => x.Channels).Where(predicate);

        }

        public Site Single(Expression<Func<Site, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Site SingleOrDefault(Expression<Func<Site, bool>> predicate)
        {
            return _items.SingleOrDefault(predicate);
        }

        public Site First(Expression<Func<Site, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IList<Site> GetAll()
        {
            return _items.Include(x => x.Group).ToList();
        }

        public Site GetByID(int id)
        {
            return _items.Include(x => x.Group).First(x => x.Id == id);
        }

        public void Insert(Site item)
        {
            // Group attach = _items.Attach(item);
            _items.Add(item);
        }

        public void Delete(int item)
        {
            Site site = _items.Find(item);
            _items.Attach(site);
            _items.Remove(site);
        }

        public void Update(Site item)
        {
            _items.Attach(item);
            _uow.Update(item);
        }

        public void Save()
        {
            _uow.SaveChanges();
        }

        public bool AddNewUrl(string uri)
        {
            if (!_newSiteService.IsValidUri(uri))
            {
                _validationDictionary.AddError("IsValidUri", "Url is not valid");
                return false;
            }
            Site makeNewSite = MakeNewSite(uri);
            Insert(makeNewSite);

            return true;
        }


        public Site Create()
        {
            Site site = _items.Create();
           // site.LastVisited = DateTime.UtcNow;
            return site;
        }


        public void Delete(Site item)
        {
            _items.Attach(item);
            _items.Remove(item);
        }


        public Site MakeNewSite(string uri)
        {
            var newuri = new Uri(uri);
            Site site = Create();
            Channel channel = _channelService.Create();
            site.SiteUrl = newuri.Scheme + "://" + newuri.Host + _newSiteService.GetPort(newuri);

            channel.Link = newuri.AbsolutePath;
            channel.Site = site;
            site.Channels.Add(channel);
            return site;

        }
    }
}