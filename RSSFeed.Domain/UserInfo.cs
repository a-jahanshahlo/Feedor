using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RSSFeed.Domain
{
    public class UserInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        [DisplayName(" نام نمایشی پروفایل")]
        public string AliasName { get; set; }
        [DisplayName("تلفن ")]
        public string Phone { get; set; }
        [DisplayName(" نام")]
        public string FirstName { get; set; }
        [DisplayName(" نام خانوادگی")]
        public string LastName { get; set; }
        [DisplayName(" ایمیل")]
        public string Email { get; set; }
        [DisplayName(" آدرس وب سایت")]
        public string Web { get; set; }
        [DisplayName(" توضیح کوتاه درباره خود")]
        public string Description { get; set; }
        [DisplayName("کاربری ")]
        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}