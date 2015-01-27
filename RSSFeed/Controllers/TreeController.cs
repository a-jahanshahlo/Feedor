using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RSSFeed.Data.Context;
using RSSFeed.Domain;
using RSSFeed.Domain.Poco;
using RSSFeed.Service.Interface;
using Newtonsoft.Json;

namespace RSSFeed.WebUI.Controllers
{
    public class TreeController : Controller
    {

        private readonly IJsTree _jsTree;
        private IGroupService _groupService;
        private readonly ISiteService _siteService;
        private IChannelService _channelService;
        private IUnitOfWork _unitOfWork;
        private readonly ISyndicationFeed _syndicationFeed;
        private readonly ICache _cache;
        public TreeController()
        {
            _cache = new CacheService();
            _unitOfWork = new MainContext();
            _groupService = new EfGroupService(_unitOfWork);
            _siteService = new EfSiteService(new ModelStateWrapper(this.ModelState), _unitOfWork);
            _channelService = new EfChannelService(_unitOfWork);
            _jsTree = new EfJsTree();


            _syndicationFeed = new SyndicationFeedService();
        }


        public ActionResult GetTreeJson()
        {
            IList<Site> siteListCache = _cache.GetAllSites();
            IList<Site> siteList;
            if (siteListCache.Count <= 0)
            {
                var sites = _siteService.AsQueryable().OrderBy(x => x.Id).Take(1).ToList();
                siteList = _syndicationFeed.LoadRange(sites);
                _cache.Clear();
                _cache.Add(siteList);
                foreach (var site in sites)
                {
                    _siteService.Update(site);

                }
                _siteService.Save();
            }
            else
            {
                siteList = siteListCache.OrderBy(x => x.Id).Take(20).ToList();
            }

            _jsTree.Add(siteList);

            return Json(_jsTree.JsTreeNodesList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetMoreList()
        {
            int skip = _cache.GetAllSites() == null ? 0 : _cache.GetAllSites().Count;
            var sites = _siteService.AsQueryable().OrderBy(x => x.Id).Skip(skip).Take(1).ToList();
            var siteList = _syndicationFeed.LoadRange(sites);
            _cache.Add(siteList);

            IList<JsTreeNode> jsTreeNodes = _jsTree.Insert(siteList);

            return Json(jsTreeNodes, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult GetAjaxList(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                id = id.Replace("c", "");
            }
            int val = int.Parse(id);
            Channel channel = _cache.GetChannelById(val) ?? new Channel();
            string s = JsonConvert.SerializeObject(channel, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return new ContentResult() { Content = s, ContentType = "application/json" };


        }




    }
}