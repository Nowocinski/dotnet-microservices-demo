using Microsoft.EntityFrameworkCore;

namespace Wallet.Api.Context
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Wallet>(column =>
            {
                column.HasKey(key => key.UserId);
            });
        }

        public DbSet<Models.Wallet> Wallets { get; set; }
    }
}
