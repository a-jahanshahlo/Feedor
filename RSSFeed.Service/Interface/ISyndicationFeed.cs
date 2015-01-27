using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;
using RSSFeed.Domain;

namespace RSSFeed.Service.Interface
{
    public interface ISyndicationFeed
    {
        XmlReader ReadXml(string path);
        IList<Site> LoadRange(IList<Site> sites);
        SyndicationFeed LoadFeed(XmlReader xmlReader);
        Channel MapToChannel(XmlReader xmlReader);
        Site MapToSite(Site site);
        IList<Item> MapToItems(IEnumerable<SyndicationItem> syndicationLink);
        SyndicationFeed CanReadAtom(XmlReader xmlReader);
        SyndicationFeed CanReadRss2(XmlReader xmlReader);
        IEnumerable<SyndicationItem> ReadFeed(SyndicationFeed syndicationFeed);
        bool TryParseFeed(string url);
        void FilterItemByDate(ref Channel sourceChannel,Channel destChannel);
    }
}