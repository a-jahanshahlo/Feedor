using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RSSFeed.Data.Context;
using RSSFeed.Domain;
using RSSFeed.Service.Interface;

namespace RSSFeed.WebUI.Controllers
{
    public class SiteGroupController : Controller
    {
        private readonly IGroupService _service;
        private readonly IUnitOfWork _unitOfWork = new MainContext();

        public SiteGroupController()
        {
            _service = new EfGroupService(_unitOfWork);
        }
        //
        // GET: /SiteGroup/
        public ActionResult Index()
        {
            return View(_service.GetAll());
        }
        public ActionResult Create()
        {
            return View(_service.Create());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Group group)
        {
            if (ModelState.IsValid)
            {
                _service.Insert(group);
                _service.Save();
                return RedirectToAction("Index", "SiteGroup");
            }
            return View(group);
        }
        public ActionResult Edit(int id)
        {
            return View(_service.SingleOrDefault(x=>x.Id==id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Group group)
        {
            if (ModelState.IsValid)
            {
                _service.Update(group);
                _service.Save();
                return RedirectToAction("Index", "SiteGroup");
            }
            return View(group);
        }
        public ActionResult Delete(int id)
        {
            return View(_service.SingleOrDefault(x => x.Id == id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Group group)
        {
            if (ModelState.IsValid)
            {
                _service.Delete(group);
                _service.Save();
                return RedirectToAction("Index", "SiteGroup");
            }
            return View(group);
        }
        public ActionResult Details(int id)
        {
            return View(_service.SingleOrDefault(x => x.Id == id));
        }

	}
}