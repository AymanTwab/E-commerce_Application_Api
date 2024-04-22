using Microsoft.Extensions.Logging;
using Store.Data.Context;
using Store.Data.Entities;
using System.Text.Json;

namespace Store.Repository
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreDbContext context,ILoggerFactory loggerFactory)
        {
            try
            {
                if (context.Categories != null && !context.Categories.Any()) 
                {
                    var categoriesData = File.ReadAllText("../Store.Repository/SeedData/Category.json");
                    var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);
                    if (categories != null )
                        await context.Categories.AddRangeAsync(categories);
                }

                if (context.Products != null && !context.Products.Any())
                {
                    var productsData = File.ReadAllText("../Store.Repository/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    if (products != null)
                        await context.Products.AddRangeAsync(products);
                }
                
                if (context.DeliveryMethods != null && !context.DeliveryMethods.Any())
                {
                    var deliveryMethodsData = File.ReadAllText("../Store.Repository/SeedData/delivery.json");
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);
                    if (deliveryMethods != null)
                        await context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
