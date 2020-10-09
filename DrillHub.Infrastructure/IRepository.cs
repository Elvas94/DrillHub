using System;
using System.Linq.Expressions;

namespace DrillHub.Infrastructure
{
    public interface IRepository<TEntity, TKey> : IEntityRepository<TEntity> where TEntity : class, IAggregateRoot<TKey> where TKey : struct
    {
        TEntity GetByKey(TKey id, params Expression<Func<TEntity, object>>[] includedPaths);
        void InsertOrUpdate(TEntity entity);
    }
}
