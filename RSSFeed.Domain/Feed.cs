using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RSSFeed.Domain
{
    #region comment code

    //public class GroupMap : EntityTypeConfiguration<Group>
    //{
    //    public GroupMap()
    //    {
    //        // Primary Key
    //        this.HasKey(t => t.Id);

    //        // Properties
    //        this.Property(t => t.Id)
    //            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

    //        // Table & Column Mappings
    //        this.ToTable("Groups");
    //        this.Property(t => t.Id).HasColumnName("Id");
    //        this.Property(t => t.GroupName).HasColumnName("GroupName");
    //    }
    //}
    //public class ChannelMap : EntityTypeConfiguration<Channel>
    //{
    //    public ChannelMap()
    //    {
    //        // Primary Key
    //        this.HasKey(t => t.Id);

    //        // Properties
    //        this.Property(t => t.Id)
    //            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

    //        // Table & Column Mappings
    //        this.ToTable("Channels");
    //        this.Property(t => t.Id).HasColumnName("Id");
    //        this.Property(t => t.SiteId).HasColumnName("SiteId");
    //        this.Property(t => t.Name).HasColumnName("Name");

    //        // Relationships
    //        this.HasOptional(t => t.Site)
    //            .WithMany(t => t.Channels)
    //            .HasForeignKey(d => d.SiteId);

    //    }
    //}

    //public class ItemMap : EntityTypeConfiguration<Item>
    //{
    //    public ItemMap()
    //    {
    //        // Primary Key
    //        this.HasKey(t => t.Id);

    //        // Properties
    //        this.Property(t => t.Id)
    //            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

    //        // Table & Column Mappings
    //        this.ToTable("Items");
    //        this.Property(t => t.Id).HasColumnName("Id");
    //        this.Property(t => t.ChannelId).HasColumnName("ChannelId");
    //        this.Property(t => t.Name).HasColumnName("Name");

    //        // Relationships
    //        this.HasOptional(t => t.Channel)
    //            .WithMany(t => t.Items)
    //            .HasForeignKey(d => d.ChannelId);

    //    }
    //}
    //public class SiteMap : EntityTypeConfiguration<Site>
    //{
    //    public SiteMap()
    //    {
    //        // Primary Key
    //        this.HasKey(t => t.Id);

    //        // Properties
    //        this.Property(t => t.Id)
    //            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

    //        // Table & Column Mappings
    //        this.ToTable("Sites");
    //        this.Property(t => t.Id).HasColumnName("Id");
    //        this.Property(t => t.GroupId).HasColumnName("GroupId");
    //        this.Property(t => t.Name).HasColumnName("Name");

    //        // Relationships
    //        this.HasOptional(t => t.Group)
    //            .WithMany(t => t.Sites)
    //            .HasForeignKey(d => d.GroupId);

    //    }
    //}
    #endregion

    public class Favorite : DelEntity
    {
         [Display(Name = " تاریخ ویرایش")]
        public DateTime? ModifyDate { get; set; }
        [Display(Name = " تاریخ درج")]
        public DateTime? AddedDate { get; set; }



    }
    public class FavoriteSite : Favorite
    {
        [Index("IX_FavoriteSite", 0, IsUnique = true)]
        public virtual Site Site { get; set; }
    }
    public class FavoriteItem : Favorite
    {

        [Index("IX_FavoriteItem", 0, IsUnique = true)]
        public virtual Item Item { get; set; }
    }
    /// <summary>
    /// This class define for grouping sites (e.g. Sport|Social ....)
    /// </summary>
    public  class Group : DelEntity
    {
        public Group()
        {
            Sites = new List<Site>();
        }
         [Display(Name = "نام گروه")]
        public string GroupName { get; set; }
        public virtual IList<Site> Sites { get; set; }
    }

    public class Site : DelEntity
    {
        public Site()
        {
            Channels = new List<Channel>();

        }
        [Display(Name = "نام سایت")]
        public string SiteName { get; set; }
        [Index("IX_SiteUrl", 0, IsUnique = true)]
        [MaxLength(253)]
        [Display(Name = "آدرس سایت")]
        public string SiteUrl { get; set; }
        [Display(Name = "زبان سایت")]
        public string Language { get; set; }
        [Display(Name = "آخرین بازدید")]
        public DateTimeOffset? LastVisited { get; set; }
        [Display(Name = " آخرین بروز رسانی")]
        public DateTimeOffset? LastUpdatedTime { get; set; }
        public virtual Group Group { get; set; }
        public virtual IList<Channel> Channels { get; set; }

    }

    public class Channel : DelEntity
    {
        public Channel()
        {
            Items = new List<Item>();
          
        }
        [Display(Name = " عنوان")]
        public string Title { get; set; }
        [Display(Name = " لینک")]
        public string Link { get; set; }
        [Display(Name = " توضیحات")]
        public string Description { get; set; }
        [Display(Name = " حق کپی")]
        public string Copyright { get; set; }
        [Display(Name = " زبان")]
        public string Language { get; set; }
        [Display(Name = " آخرین تاریخ مشاهده")]
        public DateTime? LastVisited { get; set; }
        [Display(Name = " تاریخ بروز رسانی")]
        public DateTimeOffset? LastUpdatedTime { get; set; }
        public virtual Site Site { get; set; }
        public virtual IList<Item> Items { get; set; }

    }

    public class Item : DelEntity
    {
        public Item()
        {
            Authors = new List<PubPerson>();
            Links = new List<Link>();
         
        }
        [Index("IX_UniqId", 0, IsUnique = true)]
        [MaxLength(250)]
        public string UniqId { get; set; }
        [Display(Name = "عنوان ")]
        public string Title { get; set; }

        public virtual IList<PubPerson> Authors { get; set; }
        [Display(Name = " تاریخ انتشار")]
        public DateTimeOffset? PubDate { get; set; }
        [AllowHtml]
        [Display(Name = "توضبحات ")]
        public string Description { get; set; }
        public bool IsVisited { get; set; }
        public virtual Channel Channel { get; set; }
        public virtual IList<Link> Links { get; set; }
    }
    public class Link : DelEntity
    {
        [Display(Name = " عنوان")]
        public string Title { get; set; }
        [Display(Name = " آدرس")]
        public string Url { get; set; }
        public virtual Item Item { get; set; }
    }
    public class PubPerson : DelEntity
    {
        [Display(Name = " ایمیل")]
        public string Email { get; set; }
        [Display(Name = " نام")]
        public string Name { get; set; }
        [Display(Name = " آدرس")]
        public string Url { get; set; }
        public virtual Item Item { get; set; }
    }
}
