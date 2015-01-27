using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using RSSFeed.Data.Context;
using RSSFeed.Domain;

namespace RSSFeed.Service.Interface
{
    public class EfItemService : IItemService
    {
        IUnitOfWork _uow;
        readonly IDbSet<Item> _items;

        public EfItemService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
            _items = _uow.Set<Item>();
        }


        public void AddNew(Domain.Item item)
        {
            _items.Add(item);
        }

        public IQueryable<Item> AsQueryable()
        {
            return _items.Include(x=>x.Channel).Include(x=>x.Channel.Site).Where(x=>x.IsDeleted==false).AsQueryable();
        }

        public IEnumerable<Item> Find(Expression<Func<Item, bool>> predicate)
        {
            return _items.Where(predicate);

        }

        public Item Single(Expression<Func<Item, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Item SingleOrDefault(Expression<Func<Item, bool>> predicate)
        {
            return _items.SingleOrDefault(predicate);
        }

        public Item First(Expression<Func<Item, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IList<Item> GetAll()
        {
            return _items.ToList();
        }

        public Item GetByID(int id)
        {
            return _items.First(x => x.Id == id);
        }

        public void Insert(Item item)
        {
            // Group attach = _items.Attach(item);
            _items.Add(item);
        }

        public void Delete(int item)
        {
            Item it = _items.Find(item);
            _items.Attach(it);
            _items.Remove(it);
        }

        public void Update(Item item)
        {
            _items.Attach(item);
            _uow.Update(item);
        }

        public void Save()
        {
            _uow.SaveChanges();
        }




        public Item Create()
        {
            Item item = _items.Create();

            return item;
        }


        public void Delete(Item item)
        {
            _items.Attach(item);
            _items.Remove(item);
        }


 
    }
}