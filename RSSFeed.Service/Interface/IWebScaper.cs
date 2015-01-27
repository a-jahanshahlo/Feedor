using System;
using System.Text;
using HtmlAgilityPack;

namespace RSSFeed.Service.Interface
{
    public interface IWebScaper
    {
        string GetPageTitle(string url);
        string GetPageLanguage(string url);
    }

    public class WebScraperService:IWebScaper
    {
        private HtmlAgilityPack.HtmlWeb _web;
        private HtmlAgilityPack.HtmlDocument _htmlDoc;
        private readonly IUrlService _urlervice;
        public WebScraperService()
        {
            _web = new HtmlWeb();
           
           // _htmlDoc = new HtmlAgilityPack.HtmlDocument();
            _urlervice = new UrlService();
       
        }
        public string GetPageTitle(string url)
        {
            if (!_urlervice.IsValidUri(url)) return string.Empty;
            var uri = new Uri(url);
            var path = _urlervice.GetAbsolutePath(uri);
            _htmlDoc = _web.Load(path);

            _htmlDoc.OptionCheckSyntax = true;
            _htmlDoc.OptionFixNestedTags = true;
            _htmlDoc.OptionAutoCloseOnEnd = true;
            _htmlDoc.OptionDefaultStreamEncoding = Encoding.UTF8;

            var title = _htmlDoc.DocumentNode.SelectSingleNode("/html/head/title");
            return title.InnerText;
        }

        public string GetPageLanguage(string url)
        {
       
            if (!_urlervice.IsValidUri(url)) return string.Empty;
            var uri = new Uri(url);
            var path = _urlervice.GetAbsolutePath(uri);
            _htmlDoc = _web.Load(path);

            _htmlDoc.OptionCheckSyntax = true;
            _htmlDoc.OptionFixNestedTags = true;
            _htmlDoc.OptionAutoCloseOnEnd = true;
            _htmlDoc.OptionDefaultStreamEncoding = Encoding.UTF8;
           
            var html = _htmlDoc.DocumentNode.SelectSingleNode("/html");
        
            return html.GetAttributeValue("lang","");
        }
    }
}