using Microsoft.EntityFrameworkCore;

namespace Order.Api.Context
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Order>(column =>
            {
                column.HasKey(key => key.Id);
            });
        }

        public DbSet<Models.Order> Orders { get; set; }
    }
}
