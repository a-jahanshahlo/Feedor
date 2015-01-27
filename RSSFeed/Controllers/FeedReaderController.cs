using Newtonsoft.Json;
using RSSFeed.Data.Context;
using RSSFeed.Domain;
using RSSFeed.Service.Interface;
using RSSFeed.WebUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RSSFeed.WebUI.Controllers
{
    public class FeedReaderController : Controller
    {
        private readonly ISiteService _site;
        private readonly IChannelService _service;
        private readonly IUnitOfWork _unitOfWork = new MainContext();
        private ISyndicationFeed _syndicationFeed;
        public FeedReaderController()
        {
            _service = new EfChannelService(_unitOfWork);
            _site = new EfSiteService(new ModelStateWrapper(this.ModelState), _unitOfWork);


            _syndicationFeed = new SyndicationFeedService();

        }
        //
        // GET: //
        public ActionResult Index(int? from, int? to)
        {
            int skip = from == null ? 0 : from.Value;
            int count = to == null ? 10 : to.Value;
            var items = _site.AsQueryable().OrderBy(x => x.Id).Skip(skip).Take(count).ToList();
            IList<Site> siteList = new List<Site>();
            foreach (var item in items)
            {
                foreach (var channel in item.Channels)
                {
                    var site = string.Format("{0}{1}", item.SiteUrl, channel.Link);
                    if (!string.IsNullOrEmpty(site))
                    {
                        var xmlReader = _syndicationFeed.ReadXml(site);
                        //Site mapToSite = _syndicationFeed.MapToSite(xmlReader);
                        //siteList.Add(mapToSite);
                    }
                }

            }

            string s = JsonConvert.SerializeObject(siteList, Formatting.Indented,
    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return new ContentResult() { Content = s, ContentType = "application/json" };


        }
        public ActionResult Create(int SiteId)
        {
            var viewModel = new ChannelViewModel();
            viewModel.MapTo(_service.Create());
            viewModel.ParentId = SiteId;
            return View(viewModel);

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