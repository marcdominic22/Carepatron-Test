using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContextSeed
    {
        // The Order of the function calls is important as models rely on others to already be created
        public static async Task SeedDataAsync(ApplicationDbContext context, bool isDevelopment)
        {
            var client = new Client(){
               Id = 1,
               FirstName = "John",
               LastName = "Smith",
               Email = "john@gmail.com",
               PhoneNumber = "+18202820232"
            };

            context.Add(client);

            await context.SaveChangesAsync();
        }
    }
}