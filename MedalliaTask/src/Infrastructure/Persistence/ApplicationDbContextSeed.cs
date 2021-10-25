using MedalliaTask.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace MedalliaTask.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Seed, if necessary
            if (!context.ShopItems.Any())
            {
                var Apple = new ShopItem { Name = "Apple", Price = 0.50 }; 
                var Banana = new ShopItem { Name = "Banana", Price = 0.70 }; 
                var Oranges = new ShopItem { Name = "Oranges", Price = 0.45 }; 
                var Ananas = new ShopItem { Name = "Ananas", Price = 1.00 }; 
                
                context.ShopItems.AddRange(Apple, Banana, Oranges, Ananas );

                await context.SaveChangesAsync();

                context.SpecialOffer.AddRange(
                    new SpecialOffer{ShopItem = Banana, Price = 1, Amount = 2},
                    new SpecialOffer{ShopItem = Oranges, Price = 0.90, Amount = 3}
                );

                await context.SaveChangesAsync();
            }
        }
    }
}