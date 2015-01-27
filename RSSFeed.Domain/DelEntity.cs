namespace RSSFeed.Domain
{
    public class DelEntity : Entity, IDel
    {
        public bool IsDeleted { get; set; }
    }
}