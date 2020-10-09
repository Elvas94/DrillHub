using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DrillHub.Infrastructure
{
    public interface IEntityRepository<TEntity>:IDisposable where TEntity: IЕntity
    {
        TEntity GetByKey(params object[] keyValues);

        IQueryable<TEntity> Query();
        IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includedPaths);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> condition);
        TEntity FirstOrDefault(
            Expression<Func<TEntity, bool>> condition,
            params Expression<Func<TEntity, object>>[] includedPaths);

        IQueryable<TEntity> Search(Expression<Func<TEntity, bool>> condition);
        IQueryable<TEntity> Search(
            Expression<Func<TEntity, bool>> condition,
            params Expression<Func<TEntity, object>>[] includedPaths);

        int Count(Expression<Func<TEntity, bool>> condition);

        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void DeleteByKey(params object[] keyValues);
        void DeleteRange(IEnumerable<TEntity> entities);

        int SaveChanges();

        void Transaction(Action action);
        TResult Transaction<TResult>(Func<TResult> action);

        void Detach(TEntity entity);
    }
}
