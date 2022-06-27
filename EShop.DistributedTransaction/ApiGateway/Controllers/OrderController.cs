using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IRequestClient<Order.Api.Models.Order> _requestClient;
        public OrderController(IRequestClient<Order.Api.Models.Order> requestClient)
        {
            _requestClient = requestClient;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(Order.Api.Models.Order order)
        {
            try
            {
                var result = await _requestClient.GetResponse<Order.Api.Responses.OrderPlaced>(order);
                return Accepted(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
