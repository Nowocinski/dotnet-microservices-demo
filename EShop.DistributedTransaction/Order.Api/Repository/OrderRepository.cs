using Order.Api.Context;

namespace Order.Api.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataBaseContext _context;
        public OrderRepository(DataBaseContext context)
        {
            _context = context;
        }
        public async Task CreateOrder(Models.Order order)
        {
            await _context.AddAsync(order);
            await _context.SaveChangesAsync();
        }
    }
}
