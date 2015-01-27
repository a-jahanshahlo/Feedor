using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using RSSFeed.Domain;
using RSSFeed.Service.Interface;

namespace RSSFeed.WebUI.ViewModel
{
    public class NewUrlViewModel
    {
        [Display(Name = " آدرس")]
        public string Url { get; set; }
    }
    public class SiteViewModel : Site, IMapService<Site>
    {

        public int ParentId { get; set; }

        public void MapTo(Site item)
        {
            this.Id = item.Id;
            this.IsDeleted = item.IsDeleted;
     
            this.LastVisited = item.LastVisited;
            this.SiteName = item.SiteName;
            this.SiteUrl = item.SiteUrl;
        }

        public Site FromMap()
        {
          return new Site(){Id = Id,IsDeleted = IsDeleted,LastVisited = LastVisited,SiteName = SiteName,SiteUrl = SiteUrl};
        }
    }
    public class ChannelViewModel : Channel,IMapService<Channel>
    {

        public int ParentId {    get; set; }

        public void MapTo(Channel item)
        {
            this.Id = item.Id;
            this.IsDeleted = item.IsDeleted;
            this.Items = item.Items;
            this.Link = item.Link;
            this.Site = item.Site;
            this.Title = item.Title;
            this.Description = item.Description;
           // this.ParentId = item.Site.Id;

        }

        public Channel FromMap()
        {
          return new Channel(){Description = Description,Id = Id,IsDeleted = IsDeleted,Items = Items,Link = Link,Site = Site,Title = Title};
        }
    }
}