using DrillHub.Infrastructure;
using DrillHub.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace DrillHub.Repositories
{
   public static class ServiceLocator
   {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddTransient(typeof(IEntityRepository<>), typeof(EntityRepository<>));
        }
   }
}
