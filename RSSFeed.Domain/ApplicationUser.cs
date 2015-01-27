using Microsoft.AspNet.Identity.EntityFramework;

namespace RSSFeed.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {

        }


        public virtual UserInfo UserInfo { get; set; }
    }
}