using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RSSFeed.Data.Context;
using RSSFeed.Domain;
using RSSFeed.Service.Interface;
using RSSFeed.WebUI.ViewModel;

namespace RSSFeed.WebUI.Controllers
{

    public class ChannelController : Controller
    {
        private readonly ISiteService _site;
        private readonly IChannelService _service;
        private readonly IUnitOfWork _unitOfWork = new MainContext();
        private readonly ICache _cache;
        public ChannelController()
        {
            _service = new EfChannelService(_unitOfWork);
            _site = new EfSiteService(new ModelStateWrapper(this.ModelState), _unitOfWork);
            _cache = new CacheService();
        }
        //
        // GET: /SiteGroup/
        public ActionResult Index(int id)
        {
            ViewBag.SiteId = id;
            Site site = _site.GetByID(id);
            ViewBag.SiteName = site.SiteName;
            ViewBag.GroupName = site.Group == null ? string.Empty : site.Group.GroupName;

            return View(_service.Find(x => x.Site.Id == id));
        }
        public ActionResult Create(int SiteId)
        {
            var viewModel = new ChannelViewModel();
            viewModel.MapTo(_service.Create());
            viewModel.ParentId = SiteId;
            return View(viewModel);

        }
        [HttpPost]
        public ActionResult UpdateChannel(int id)
        {
            Channel channelById = _cache.GetChannelById(id);
            _cache.Remove(channelById);
            Channel byId = _service.GetByID(id);
            byId.LastUpdatedTime = DateTime.UtcNow;
            _service.Update(byId);
            _service.Save();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ChannelViewModel item)
        {
            if (ModelState.IsValid)
            {
                Channel channel = item.FromMap();
                channel.Site = _site.GetByID(item.ParentId);
                _service.Insert(channel);
                _service.Save();
                return RedirectToAction("Index", "Channel", new { id = item.ParentId });
            }
            return View(item);
        }
        public ActionResult Edit(int id)
        {
            return View(_service.SingleOrDefault(x => x.Id == id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Channel group)
        {
            if (ModelState.IsValid)
            {
                _service.Update(group);
                _service.Save();
                return RedirectToAction("Index", "Channel");
            }
            return View(group);
        }
        public ActionResult Delete(int id)
        {
            return View(_service.SingleOrDefault(x => x.Id == id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Channel group)
        {
            if (ModelState.IsValid)
            {
                _service.Delete(group);
                _service.Save();
                return RedirectToAction("Index", "Channel");
            }
            return View(group);
        }
        public ActionResult Details(int id)
        {
            return View(_service.SingleOrDefault(x => x.Id == id));
        }
    }
}