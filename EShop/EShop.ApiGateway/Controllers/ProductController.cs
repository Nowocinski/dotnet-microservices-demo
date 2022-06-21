using EShop.Infrastructure.Command.Product;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace EShop.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IBusControl _busControl;
        public ProductController(IBusControl busControl)
        {
            _busControl = busControl;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.CompletedTask;
            return Accepted("Get product Method called");
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateProduct product)
        {
            var uri = new Uri("rabbitmq://localhost/create_product");
            var endPoint = await _busControl.GetSendEndpoint(uri);
            await endPoint.Send(product);

            return Accepted("Product Created");
        }
    }
}
