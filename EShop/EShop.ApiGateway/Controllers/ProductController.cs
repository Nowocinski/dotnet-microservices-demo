using EShop.Infrastructure.Command.Product;
using EShop.Infrastructure.Event.Product;
using EShop.Infrastructure.Query.Product;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Fallback;

namespace EShop.ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IBusControl _busControl;
        private readonly IRequestClient<GetProductById> _requestClient;
        private readonly AsyncFallbackPolicy<IActionResult> _fallbackPolicy;
        public ProductController(IBusControl busControl, IRequestClient<GetProductById> requestClient)
        {
            _busControl = busControl;
            _requestClient = requestClient;
            _fallbackPolicy = Policy<IActionResult>.Handle<Exception>().FallbackAsync(Content("We are experincing issue. Please try after sometime."));
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid productId)
        {
            return await _fallbackPolicy.ExecuteAsync(async () =>
            {
                var prdct = new GetProductById()
                {
                    ProductId = productId
                };
                var product = await _requestClient.GetResponse<ProductCreated>(prdct);
                return Accepted(product);
            });
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Add([FromBody] CreateProduct product)
        {
            var uri = new Uri("rabbitmq://localhost/create_product");
            var endPoint = await _busControl.GetSendEndpoint(uri);
            await endPoint.Send(product);

            return Accepted("Product Created");
        }
    }
}
