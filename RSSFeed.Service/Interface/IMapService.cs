namespace RSSFeed.Service.Interface
{
    public interface IMapService<T>
    {
        int ParentId { get; set; }
        void MapTo(T item);
        T FromMap();
    }
}