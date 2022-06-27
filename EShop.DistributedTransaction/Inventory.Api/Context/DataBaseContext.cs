using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Context
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Inventor>(column =>
            {
                column.HasKey(key => key.ProductId);
            });
        }

        public DbSet<Models.Inventor> Inventors { get; set; }
    }
}
