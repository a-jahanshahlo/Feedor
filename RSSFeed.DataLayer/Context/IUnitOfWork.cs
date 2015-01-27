using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace RSSFeed.Data.Context
{
    public interface IUnitOfWork : IDisposable
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        DbEntityEntry<TEntity> Update<TEntity>(TEntity val) where TEntity : class;
    }
}
