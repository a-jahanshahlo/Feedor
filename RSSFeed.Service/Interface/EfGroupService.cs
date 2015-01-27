using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using RSSFeed.Data.Context;
using RSSFeed.Domain;

namespace RSSFeed.Service.Interface
{

    public class EfGroupService : IGroupService
    {
        IUnitOfWork _uow;
        readonly IDbSet<Group> _items;

        public EfGroupService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
            _items = _uow.Set<Group>();
        }
        public IQueryable<Group> AsQueryable()
        {
            return _items.Include(x => x.Sites.Select(i => i.Channels)).Where(x => x.IsDeleted == false).AsQueryable();
            
        }

        public IEnumerable<Group> Find(Expression<Func<Group, bool>> predicate)
        {
            throw new NotImplementedException();
            
        }

        public Group Single(Expression<Func<Group, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Group SingleOrDefault(Expression<Func<Group, bool>> predicate)
        {
            return _items.SingleOrDefault(predicate);
        }

        public Group First(Expression<Func<Group, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IList<Group> GetAll()
        {
            return _items.Include(x => x.Sites.Select(i=>i.Channels)).ToList();
        }

        public Group GetByID(int id)
        {
            return _items.Find(id);
        }

        public void Insert(Group item)
        {
           // Group attach = _items.Attach(item);
            _items.Add(item);
        }

        public void Delete(int item)
        {
            Group group = _items.Find(item);
            _items.Attach(group);
            _items.Remove(group);
        }

        public void Update(Group item)
        {
            _items.Attach(item);
            _uow.Update(item);
        }

        public void Save()
        {
            _uow.SaveChanges();
        }


        public Group Create()
        {
         return    _items.Create();
        }


        public void Delete(Group item)
        {
            _items.Attach(item);
            _items.Remove(item);
        }
    }
}