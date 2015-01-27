using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using RSSFeed.Data.Context;
using RSSFeed.Domain;

namespace RSSFeed.Service.Interface
{
    public class EfChannelService : IChannelService
    {
        readonly IUnitOfWork _uow;
        readonly IDbSet<Channel> _items;

        public EfChannelService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
            _items = _uow.Set<Channel>();
        }
        public IQueryable<Channel> AsQueryable()
        {
            return _items.Where(x => x.IsDeleted == false).AsQueryable();
        }

        public IEnumerable<Channel> Find(Expression<Func<Channel, bool>> predicate)
        {
            return _items.Include(x=>x.Site).Where(predicate).AsEnumerable();

        }

        public Channel Single(Expression<Func<Channel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Channel SingleOrDefault(Expression<Func<Channel, bool>> predicate)
        {
            return _items.SingleOrDefault(predicate);
        }

        public Channel First(Expression<Func<Channel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IList<Channel> GetAll()
        {
            return _items.ToList();
        }

        public Channel GetByID(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public void Insert(Channel item)
        {
           // _items.Attach(item);
            _items.Add(item);
        }

        public void Delete(int item)
        {
            Channel group = _items.Find(item);
            _items.Attach(group);
            _items.Remove(group);
        }

        public void Update(Channel item)
        {
            _items.Attach(item);
            _uow.Update(item);
        }

        public void Save()
        {
            _uow.SaveChanges();
        }


        public Channel Create()
        {
            return    _items.Create();
        }


        public void Delete(Channel item)
        {
            _items.Attach(item);
            _items.Remove(item);
        }
    }
}