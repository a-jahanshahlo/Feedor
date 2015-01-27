using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using RSSFeed.Domain;

namespace RSSFeed.Service.Interface
{
    public class SyndicationFeedService : ISyndicationFeed
    {
        private Site _site;
        private IWebScaper _webScaper;
        public SyndicationFeedService(Site site)
        {
            _site = site;
            _webScaper = new WebScraperService();
        }
        public SyndicationFeedService()
        {
            _webScaper = new WebScraperService();
        }
        public void GetSite(Site site)
        {
            _site = site;
        }
        public XmlReader ReadXml(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            if (!TryParseFeed(path)) return null;

            XmlReader xmlReader = XmlReader.Create(path);
            if (xmlReader.ReadState == ReadState.Initial)
                xmlReader.MoveToContent();
            return xmlReader;
        }
        public bool TryParseFeed(string url)
        {
            try
            {
                SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(url));

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public SyndicationFeed LoadFeed(XmlReader xmlReader)
        {
            return SyndicationFeed.Load(xmlReader);
        }
        public Channel MapToChannel(XmlReader xmlReader)
        {

            var channel = new Channel() { LastVisited = DateTime.UtcNow };
            SyndicationFeed readAtom = CanReadAtom(xmlReader);
            if (readAtom != null)
            {
                var items = ReadFeed(readAtom);

                channel.Language = readAtom.Language;
                channel.LastUpdatedTime = readAtom.LastUpdatedTime;
                channel.Copyright = readAtom.Copyright == null ? string.Empty : readAtom.Copyright.Text;
                channel.Description = readAtom.Description == null ? string.Empty : readAtom.Description.Text;
                channel.Title = readAtom.Title == null ? string.Empty : readAtom.Title.Text;

                channel.Items = MapToItems(items);

                return channel;

            }
            SyndicationFeed canReadRss2 = CanReadRss2(xmlReader);
            if (canReadRss2 != null)
            {
                var items = ReadFeed(canReadRss2);
                channel.Items = MapToItems(items);

                channel.Language = canReadRss2.Language;
                channel.LastUpdatedTime = canReadRss2.LastUpdatedTime;
                channel.Copyright = canReadRss2.Copyright == null ? string.Empty : canReadRss2.Copyright.Text;
                channel.Description = canReadRss2.Description == null ? string.Empty : canReadRss2.Description.Text;
                channel.Title = canReadRss2.Title == null ? string.Empty : canReadRss2.Title.Text;

                return channel;

            }

            return null;
        }
        public IList<Item> MapToItems(IEnumerable<SyndicationItem> syndicationLink)
        {
            IList<Item> items = new List<Item>();
            foreach (var link in syndicationLink)
            {
                var item = new Item();
                foreach (var syndLink in link.Links)
                {
                    var link1 = new Link();
                    link1.Title = syndLink.Title ?? string.Empty;
                    link1.Url = syndLink.Uri == null ? string.Empty : syndLink.Uri.OriginalString;
                    item.Links.Add(link1);
                }
                foreach (var author in link.Authors)
                {

                    item.Authors.Add(new PubPerson() { Email = author.Email ?? string.Empty, Name = author.Name ?? string.Empty, Url = author.Uri ?? string.Empty });
                }
                item.UniqId = link.Id;
                item.PubDate = link.PublishDate;
                item.Title = link.Title == null ? string.Empty : link.Title.Text;
                item.Description = link.Summary == null ? string.Empty : link.Summary.Text;
                items.Add(item);
            }

            return items;
        }
        public SyndicationFeed CanReadAtom(XmlReader xmlReader)
        {
            var atom = new Atom10FeedFormatter();
            // try to read it as an atom feed
            if (atom.CanRead(xmlReader))
            {
                atom.ReadFrom(xmlReader);
                return atom.Feed;
            }
            return null;
        }
        public SyndicationFeed CanReadRss2(XmlReader xmlReader)
        {
            var rss = new Rss20FeedFormatter();
            // try reading it as an rss feed
            if (rss.CanRead(xmlReader))
            {
                rss.ReadFrom(xmlReader);

                return rss.Feed;
            }
            return null;
        }
        public IEnumerable<SyndicationItem> ReadFeed(SyndicationFeed syndicationFeed)
        {
            return syndicationFeed.Items.ToList();
        }



        public IList<Site> LoadRange(IList<Site> sites)
        {
            IList<Site> siteList = new List<Site>();
            foreach (var item in sites)
            {
                Site ss = new Site() { Id = item.Id, Language = item.Language, SiteName = item.SiteName, SiteUrl = item.SiteUrl, Group = item.Group, LastUpdatedTime = item.LastUpdatedTime, LastVisited = item.LastVisited };
                foreach (var channel in item.Channels)
                {
                    var site = string.Format("{0}{1}", item.SiteUrl, channel.Link);
                    if (!string.IsNullOrEmpty(site))
                    {
                        var xmlReader = ReadXml(site);
                        if (xmlReader == null) continue;

                        Channel mapToChannel = MapToChannel(xmlReader);
                        FilterItemByDate(ref mapToChannel, channel);
                        mapToChannel.Id = channel.Id;
                        ss.Channels.Add(mapToChannel);


                    }
                }
                MapToSite(ss);
                siteList.Add(ss);
                MapToSite(item);

            }

            return siteList;
        }


        public Site MapToSite(Site site)
        {
            if (string.IsNullOrEmpty(site.Language))
                site.Language = _webScaper.GetPageLanguage(site.SiteUrl);
            site.LastUpdatedTime = DateTime.UtcNow;
            if (string.IsNullOrEmpty(site.SiteName))
                site.SiteName = _webScaper.GetPageTitle(site.SiteUrl);
            return site;

        }





        public void FilterItemByDate(ref Channel sourceChannel, Channel destChannel)
        {
            //DateTimeOffset dateOffset1, dateOffset2;
            //TimeSpan difference;
            //dateOffset1 = DateTimeOffset.Now;
            //dateOffset2 = DateTimeOffset.UtcNow;
            //difference = dateOffset1 - dateOffset2;
            var items = sourceChannel.Items.Where(x => x.PubDate <= destChannel.LastUpdatedTime).ToList();
            foreach (var item in items)
            {
                sourceChannel.Items.Remove(item);
            }
        }
    }
}