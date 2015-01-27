using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using RSSFeed.Data.Context;
using RSSFeed.Domain.opml;
using RSSFeed.Domain.Poco;
using RSSFeed.Service.Interface;
using RSSFeed.WebUI.ActionResults;

namespace RSSFeed.WebUI.Controllers
{
    public class FileController : Controller
    {
        private readonly IFile _file;
        private readonly ISiteService _siteService;
        private IOpml _opml;
        public FileController()
        {
            IUnitOfWork unitOfWork = new MainContext();
            _file = new FileService();

            _siteService = new EfSiteService(new ModelStateWrapper(this.ModelState), unitOfWork);
            _opml = new OpmlService(_siteService);
        }
        //
        // GET: /File/
        public FilePathResult Image()
        {
            string filename = Request.Url.AbsolutePath.Replace("/home/image", "");
            string contentType = "";
            var filePath = new FileInfo(Server.MapPath("~/App_Data") + filename);

            var index = filename.LastIndexOf(".") + 1;
            var extension = filename.Substring(index).ToUpperInvariant();

            // Fix for IE not handling jpg image types
            contentType = string.Compare(extension, "JPG") == 0 ? "image/jpeg" : string.Format("image/{0}", extension);

            return File(filePath.FullName, contentType);
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {
            var r = _file.CreateList();
            _file.ReadFiles(Request.Files);
            var count = _file.UploadedFilesLength();
            _file.CopyTo(r, count);

            try
            {
                foreach (var file in _file.GetHttpPostedFile())
                {
                    
                    _opml.ImportOpml(file);
                   
                }
                return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}", "application/json");

            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
            }

        }


        public ActionResult DownloadOpml()
        {
            return new XmlResult<string>() {Data = _opml.ExportOpml()};

        }
    }
}