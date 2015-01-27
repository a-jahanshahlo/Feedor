using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using RSSFeed.Data.Context;
using RSSFeed.Domain;
using RSSFeed.Service.Interface;
using RSSFeed.WebUI.ViewModel;

namespace RSSFeed.WebUI.Controllers
{

    public class SitesController : Controller
    {
        private readonly ISiteService _service;
        private IChannelService _channelService;
        private readonly IUnitOfWork _unitOfWork = new MainContext();
        private readonly IGroupService _groupService;
        private readonly ICache _cache;
        private readonly ISyndicationFeed _syndicationFeed;
        public SitesController()
        {
            _cache = new CacheService();
            _service = new EfSiteService(new ModelStateWrapper(this.ModelState), _unitOfWork);
            _groupService = new EfGroupService(_unitOfWork);
            _channelService = new EfChannelService(_unitOfWork);
            _syndicationFeed = new SyndicationFeedService();

        }
        //
        // GET: /SiteGroup/
        public ActionResult Index(int? groupId)
        {
            IEnumerable<Site> enumerable = groupId.HasValue ? _service.AsQueryable().Where(x => x.Group.Id == groupId).ToList() : _service.GetAll();
            ViewBag.groupId = groupId.HasValue ? groupId.Value : -1;
            return View(enumerable);
        }
        public ActionResult Create(int? groupId)
        {
            var siteViewModel = new SiteViewModel();
            siteViewModel.MapTo(_service.Create());
            if (groupId.HasValue)
            {
                siteViewModel.ParentId = groupId.Value;
            }
            return View(siteViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SiteViewModel siteViewModel)
        {
            if (ModelState.IsValid)
            {
                Site site = siteViewModel.FromMap();
                site.Group = _groupService.GetByID(siteViewModel.ParentId);
                _service.Insert(site);
                _service.Save();
                return RedirectToAction("Index", "Sites", new { id = siteViewModel.ParentId });
            }
            return View(siteViewModel);
        }
        public ActionResult Edit(int id)
        {
            return View(_service.SingleOrDefault(x => x.Id == id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Site group)
        {
            if (ModelState.IsValid)
            {
                _service.Update(group);
                _service.Save();
                return RedirectToAction("Index", "Sites");
            }
            return View(group);
        }
        public ActionResult Delete(int id)
        {
            return View(_service.SingleOrDefault(x => x.Id == id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Site group)
        {
            if (ModelState.IsValid)
            {
                _service.Delete(group);
                _service.Save();
                return RedirectToAction("Index", "Sites");
            }
            return View(group);
        }
        public ActionResult Details(int id)
        {
            return View(_service.SingleOrDefault(x => x.Id == id));
        }
        public ActionResult Remove(int id)
        {
            Channel byId = _channelService.GetByID(id);
            byId.IsDeleted = true;
            _channelService.Update(byId);
            _channelService.Save();

            return RedirectToAction("List", "Sites");
        }

        public ActionResult List(int? page)
        {
            var items = _service.AsQueryable();

            var pageNumber = page ?? 1;
            var selecteditems = items.OrderBy(x => x.Id).ToPagedList(pageNumber, 20);


            return View(selecteditems);
        }
        public ActionResult NewUrl()
        {
            var newUrlViewModel = new NewUrlViewModel();
            return View(newUrlViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewUrl(NewUrlViewModel newUrlViewModel)
        {
            if (!_syndicationFeed.TryParseFeed(newUrlViewModel.Url))
            {
                ModelState.AddModelError("UrlInvalid","The entered URL is invalid to load Feed");
                return View(newUrlViewModel);
            }

            if (ModelState.IsValid && _service.AddNewUrl(newUrlViewModel.Url))
            {
                _service.Save();
                _cache.Clear();
                return RedirectToAction("Index", "Sites");
            }
            return View(newUrlViewModel);
        }

    }
}