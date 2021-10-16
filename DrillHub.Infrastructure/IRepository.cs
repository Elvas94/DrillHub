using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DrillHub.Infrastructure
{
    public interface IRepository<TEntity, TKey> : IEntityRepository<TEntity>
        where TEntity : class, IAggregateRoot<TKey> where TKey : struct
    {
        ValueTask<TEntity> GetByKeyAsync(TKey id, params Expression<Func<TEntity, object>>[] includedPaths);
        void InsertOrUpdate(TEntity entity);
    }
}
