using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DrillHub.DataAccess
{
    public class TemporaryDbContextFactory: IDesignTimeDbContextFactory<DrillHubContext>
    {
        public DrillHubContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"))
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json"))
                .Build();

            return new DrillHubContext(new DbContextOptionsBuilder<DrillHubContext>()
                .UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    builder =>
                    {
                        builder.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
                    })
                .Options);
        }
    }
}
