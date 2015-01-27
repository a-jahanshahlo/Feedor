using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using RSSFeed.Domain;

namespace RSSFeed.Data.Context
{

    public class MainContext : IdentityDbContext<ApplicationUser>, IUnitOfWork
    {
        //This overload needed to find custom connectionString in WEB Layer at web.config 
        //Custom ConnectionString Must Declare in web.config in WebLayer not at this layer!!
        public MainContext()
            : base("MainContextConnection")
        {

        }   
        public DbSet<PubPerson> PubPersons { get; set; }
        public DbSet<FavoriteSite> FavoriteSites { get; set; }
        public DbSet<FavoriteItem> FavoriteItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Channel> Channels { get; set; }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {

            return base.Set<TEntity>();
        }

        public new int SaveChanges()
        {
            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }






        public DbEntityEntry<TEntity> Update<TEntity>(TEntity val) where TEntity : class
        {

            var entity = Entry(val);
            entity.State = EntityState.Modified;
            return entity;
        }

        public void Dispose()
        {

        }

        //  public System.Data.Entity.DbSet<RSSFeed.WebUI.ViewModel.SiteViewModel> SiteViewModels { get; set; }

        //public System.Data.Entity.DbSet<RSSFeed.WebUI.ViewModel.ChannelViewModel> ChannelViewModels { get; set; }
    }
}