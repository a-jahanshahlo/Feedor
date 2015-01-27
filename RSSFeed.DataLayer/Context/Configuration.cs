using System.Data.Entity.Migrations;

namespace RSSFeed.Data.Context
{
    public class Configuration : DbMigrationsConfiguration<MainContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MainContext context)
        {
            base.Seed(context);
        }
    }
    //public class ConfigurationUserAccount : DbMigrationsConfiguration<MainContext>
    //{
    //    public ConfigurationUserAccount()
    //    {
    //        AutomaticMigrationsEnabled = true;
    //        AutomaticMigrationDataLossAllowed = true;
    //    }

    //    protected override void Seed(ApplicationDbContext context)
    //    {
    //        base.Seed(context);
    //    }
    //}
}
