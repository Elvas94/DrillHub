using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DrillHub.ConsoleRunner
{
    public  class DependencyInjector
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly object Lock = new object();
        private static DependencyInjector _instance;

        public static DependencyInjector Instance
        {
            get
            {
                lock (Lock)
                {
                    return _instance ??= new DependencyInjector();
                }
            }
        }

        private DependencyInjector()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DrillHubContext"].ConnectionString;

            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<DataAccess.DrillHubContext>(options =>
                options.UseSqlServer(connectionString));

            Model.ServiceLocator.RegisterServices(services);
            Repositories.ServiceLocator.RegisterServices(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        public T Resolve<T>() => _serviceProvider.GetRequiredService<T>();
    }
}
