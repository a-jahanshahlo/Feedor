using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RSSFeed.Startup))]
namespace RSSFeed
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
