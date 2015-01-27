using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;
using RSSFeed.Data.Context;
using RSSFeed.Domain;
using RSSFeed.Service.Interface;
using Site = RSSFeed.Domain.Site;

namespace RSSFeed.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IItemService _service;
        private readonly IUnitOfWork _unitOfWork=new MainContext();
        private ISyndicationFeed _syndicationFeed;
        public HomeController()
        {

            _syndicationFeed = new SyndicationFeedService();
           _service=new EfItemService(_unitOfWork);
        }
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}