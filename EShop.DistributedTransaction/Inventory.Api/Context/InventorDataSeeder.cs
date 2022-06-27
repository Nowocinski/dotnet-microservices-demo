namespace Inventory.Api.Context
{
    public class InventorDataSeeder
    {
        public static void PerpPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<DataBaseContext>());
            }
        }

        private static void SeedData(DataBaseContext context)
        {
            if (!context.Inventors.Any())
            {
                Console.WriteLine("Seeding data to inventor.");
                context.Inventors.AddRange(
                new Models.Inventor
                {
                    ProductId = Guid.NewGuid(),
                    Name = "Woda",
                    Quantity = 10
                },
                new Models.Inventor
                {
                    ProductId = Guid.NewGuid(),
                    Name = "Kajzerka",
                    Quantity = 10
                },
                new Models.Inventor
                {
                    ProductId = Guid.NewGuid(),
                    Name = "Sok",
                    Quantity = 10
                });
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("We already have data in inventor.");
            }
        }
    }
}
