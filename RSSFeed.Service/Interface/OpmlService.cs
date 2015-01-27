using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using RSSFeed.Domain;
using RSSFeed.Domain.opml;

namespace RSSFeed.Service.Interface
{
    public class OpmlService : IOpml
    {

        private readonly ISiteService _siteService;
        private readonly ISyndicationFeed _syndicationFeed;
        private readonly ICache _cache;
        public OpmlService(ISiteService siteService)
        {
            _siteService = siteService;
            _cache = new CacheService();
            _syndicationFeed = new SyndicationFeedService();
        }
        public void ImportOpml(HttpPostedFileBase fileBase)
        {

            foreach (var outline in Opml.ParseOpml(fileBase.InputStream))
            {
                try
                {
                    if (!_syndicationFeed.TryParseFeed(outline.XMLUrl)) continue;

                    _siteService.AddNewUrl(outline.XMLUrl);
                    _siteService.Save();
                }
                catch (Exception)
                {
                }

            }

            _cache.Clear();

        }

        public string ExportOpml()
        {
            var opml = new Opml {Body = new Body(), Head = new Head()};

            foreach (var source in _siteService.AsQueryable().ToList())
            {
                foreach (var channel in source.Channels)
                {
                    opml.Body.Outlines.Add(new Outline() { HTMLUrl = source.SiteUrl, Title = source.SiteName, Text = source.SiteName, XMLUrl = source.SiteUrl + channel.Link });

                }
            }

            return opml.ToString();
        }



    }
}