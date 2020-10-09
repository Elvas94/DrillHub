using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DrillHub.WebApi.Infrastructure.Extensions
{
    public static class StartupMigrationsExtension
    {
        public static IApplicationBuilder UseStartupMigrations<T>(this IApplicationBuilder app) where T : DbContext
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<T>();
                var lastCommandTimeout = context.Database.GetCommandTimeout();
                context.Database.SetCommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
                context.Database.Migrate();
                context.Database.SetCommandTimeout(lastCommandTimeout);
            }

            return app;
        }
    }
}
