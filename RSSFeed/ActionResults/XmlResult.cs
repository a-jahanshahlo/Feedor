using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace RSSFeed.WebUI.ActionResults
{
    public class XmlResult<T> : ActionResult 
    {
        public T Data { private get; set; }
        
        public override void ExecuteResult(ControllerContext context)
        {
            HttpContextBase httpContextBase = context.HttpContext;
            httpContextBase.Response.Buffer = true;
            httpContextBase.Response.Clear();
            
            string fileName ="MyFeeds"+ DateTime.Now.ToString("ddmmyyyyhhss") + ".opml";
            httpContextBase.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            httpContextBase.Response.ContentType = "text/xml";

            using (var writer = new StringWriter())
            {
                var xml = new XmlSerializer(typeof(T));
                xml.Serialize(writer, Data);
                httpContextBase.Response.Write(writer);
            }
        }
    }
}