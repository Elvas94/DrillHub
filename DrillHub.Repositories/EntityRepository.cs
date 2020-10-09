using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DrillHub.DataAccess;
using DrillHub.Infrastructure;
using DrillHub.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DrillHub.Repositories
{
    public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : class, IЕntity
    {
        protected readonly DbContext Context;


        public EntityRepository(DrillHubContext context)
        {
            Context = context;
        }
        public virtual TEntity GetByKey(params object[] keyValues)
        {
            return Context.Set<TEntity>().Find(keyValues);
        }

        public virtual IQueryable<TEntity> Query()
        {
            return Context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Query(
            params Expression<Func<TEntity, object>>[] includedPaths)
        {
            return Context.Set<TEntity>().IncludeByPath(includedPaths);
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> condition)
        {
            return Context.Set<TEntity>().FirstOrDefault(condition);
        }

        public virtual TEntity FirstOrDefault(
            Expression<Func<TEntity, bool>> condition,
            params Expression<Func<TEntity, object>>[] includedPaths)
        {
            return Context.Set<TEntity>().IncludeByPath(includedPaths).FirstOrDefault(condition);
        }

        public virtual IQueryable<TEntity> Search(Expression<Func<TEntity, bool>> condition)
        {
            return Context.Set<TEntity>().Where(condition);
        }

        public virtual IQueryable<TEntity> Search(
            Expression<Func<TEntity, bool>> condition,
            params Expression<Func<TEntity, object>>[] includedPaths)
        {
            return Context.Set<TEntity>().IncludeByPath(includedPaths).Where(condition);
        }

        public virtual int Count(Expression<Func<TEntity, bool>> condition)
        {
            return Context.Set<TEntity>().Count(condition);
        }

        public virtual void Insert(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public virtual void DeleteByKey(params object[] keyValues)
        {
            var entity = GetByKey(keyValues);
            if (entity != null) Delete(entity);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public virtual int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public virtual void Transaction(Action action)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    action();
                    dbContextTransaction.Commit();
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public virtual TResult Transaction<TResult>(Func<TResult> action)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var result = action();
                    dbContextTransaction.Commit();
                    return result;
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public virtual void Detach(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Detached;
        }

        #region IDisposable Support
        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                Context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
