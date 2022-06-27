using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.Api.Context;

namespace Order.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController
    {
        private readonly DataBaseContext _context;
        public OrderController(DataBaseContext context)
        {
            _context = context;
        }

        [HttpGet("GetOrders")]
        public async Task<object> Get()
        {
            return await _context.Orders.ToListAsync();
        }
    }
}
