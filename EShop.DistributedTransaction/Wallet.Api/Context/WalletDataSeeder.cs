namespace Wallet.Api.Context
{
    public class WalletDataSeeder
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
            if (!context.Wallets.Any())
            {
                Console.WriteLine("Seeding data to wallets.");
                context.Wallets.AddRange(new Models.Wallet
                {
                    UserId = Guid.NewGuid(),
                    UserName = "Rafał",
                    Fund = 100
                });
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("We already have data in wallets.");
            }
        }
    }
}
