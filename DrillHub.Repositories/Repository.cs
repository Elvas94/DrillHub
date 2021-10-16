using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DrillHub.DataAccess;
using DrillHub.Infrastructure;
using DrillHub.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DrillHub.Repositories
{
    public class Repository<TEntity, TKey> : EntityRepository<TEntity>, IRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey> where TKey : struct
    {
        public Repository(DrillHubContext context) : base(context) { }

        public virtual ValueTask<TEntity> GetByKeyAsync(
            TKey id,
            params Expression<Func<TEntity, object>>[] includedPaths)
        {
            return Context.Set<TEntity>().IncludeByPath(includedPaths).FindAsync(id);
        }

        public virtual void InsertOrUpdate(TEntity item)
        {
            var dbItem = base.GetByKey(item.Id);
            if (dbItem == null)
            {
                Context.Set<TEntity>().Add(item);
            }
            else
            {
                Context.Entry(item).State = EntityState.Modified;
            }
        }
    }
}
