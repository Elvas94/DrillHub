using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DrillHub.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            //NLog.Config.ConfigurationItemFactory.Default.JsonConverter = new NewtonsoftSerializer();

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}
