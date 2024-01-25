using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Infrastructure.Persistence;

namespace API.Extensions
{
    public static class WebHostExtensions
    {
        public static async Task<IHost> MigrateAndSeedDataAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationDbContext>();

            bool isDevelopment = false;
#if DEBUG
            Console.WriteLine("DEBUG is defined");
            isDevelopment = true;
#endif

            // Seed Data
            await ApplicationDbContextSeed.SeedDataAsync(context, isDevelopment);

            return host;
        }
    }
}