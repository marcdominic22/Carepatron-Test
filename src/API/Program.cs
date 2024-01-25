using System.Threading.Tasks;
using API.Extensions;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Hosting;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = await CreateHostBuilder(args).Build().MigrateAndSeedDataAsync();

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
