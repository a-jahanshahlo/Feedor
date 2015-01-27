using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using RSSFeed.Data.Context;
using RSSFeed.Domain;
using RSSFeed.Service.Interface;

namespace RSSFeed.WebUI.Controllers
{
    public class ItemController : Controller
    {
        private readonly ISiteService _service;
        private readonly IChannelService _channelService;
        private readonly IItemService _itemService;
        private readonly IUnitOfWork _unitOfWork = new MainContext();
        private readonly IGroupService _groupService;
        private readonly ICache _cache;
        private readonly ISyndicationFeed _syndicationFeed;

        public ItemController()
        {
            _cache = new CacheService();
            _channelService = new EfChannelService(_unitOfWork);
            _itemService = new EfItemService(_unitOfWork);
            _service = new EfSiteService(new ModelStateWrapper(this.ModelState), _unitOfWork);
            _groupService = new EfGroupService(_unitOfWork);
            _syndicationFeed = new SyndicationFeedService();
        }
        //
        // GET: /Item/
        public ActionResult Index(int? page)
        {
            var items = _itemService.AsQueryable(); 

            var pageNumber = page ?? 1; 
            var selecteditems = items.OrderBy(x=>x.Id).ToPagedList(pageNumber, 20); 


            return View(selecteditems);
        }
        [HttpPost]
        public ActionResult AddItem(string id)
        {

            Site site = _cache.GetSiteByItemId(id);
            if (site == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound); 
            Item selectedItem = site.Channels.SelectMany(x => x.Items).FirstOrDefault(x => x.UniqId == id);
            Channel channel = site.Channels.FirstOrDefault(x => x.Items.Any(y => y.UniqId == id));
            Item item = _itemService.SingleOrDefault(x => x.UniqId == selectedItem.UniqId);
            if (item == null)
            {
                item = _itemService.Create();
                item.UniqId = selectedItem.UniqId;
                item.PubDate = selectedItem.PubDate;
                item.Authors = selectedItem.Authors;
                item.Channel = selectedItem.Channel;
                item.Description = selectedItem.Description;
                item.IsVisited = selectedItem.IsVisited;
                item.Links = selectedItem.Links;
                item.Title = selectedItem.Title;
                item.IsDeleted = false;
                Channel ch = _channelService.SingleOrDefault(x => x.Id == channel.Id);
                ch.Items.Add(item);
                _itemService.Save();
            }
            if (selectedItem != null)
            {
                item.IsDeleted = false;
                _itemService.Update(item);

                Channel ch = _channelService.SingleOrDefault(x => x.Id == channel.Id);
                ch.Items.Add(item);

                _itemService.Save();
                return new HttpStatusCodeResult(HttpStatusCode.OK, id); 
            }

            return new HttpStatusCodeResult(HttpStatusCode.NotFound); 
        }
        [HttpPost]
        public ActionResult RemoveItem(string id)
        {
           
            Site site = _cache.GetSiteByItemId(id);
            if (site == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound); 
            Item selectedItem = site.Channels.SelectMany(x => x.Items).FirstOrDefault(x => x.UniqId == id);
            Item item = _itemService.SingleOrDefault(x => x.UniqId == selectedItem.UniqId);
           Channel channel = site.Channels.FirstOrDefault(x=>x.Items.Any(y=>y.UniqId==id));
            if (item==null)
            {
                 item = _itemService.Create();    
                item.UniqId = selectedItem.UniqId;
                item.PubDate = selectedItem.PubDate;
                item.Authors = selectedItem.Authors;
                item.Channel = selectedItem.Channel;
                item.Description = selectedItem.Description;
                item.IsVisited = selectedItem.IsVisited;
                item.Links = selectedItem.Links;
                item.Title = selectedItem.Title;
                item.IsDeleted = true;        
                Channel ch = _channelService.SingleOrDefault(x => x.Id == channel.Id);
                ch.Items.Add(item); _itemService.Save();
            }
 
            if (selectedItem != null)
            {

                item.IsDeleted = true;
                _itemService.Update(item);



                _itemService.Save();
                return new HttpStatusCodeResult(HttpStatusCode.OK, id); 
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotFound); 
        }

        public ActionResult Remove(string id)
        {

    
     
            Item item = _itemService.SingleOrDefault(x => x.UniqId == id);
        
            if (item != null)
            {

                item.IsDeleted = true;
                Channel ch = _channelService.SingleOrDefault(x => x.Id ==item.Channel.Id);
                ch.Items.Add(item); _itemService.Save();
                return RedirectToAction("Index", "Item");
            }

            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }
    }
}