using EShop.Infrastructure.Command.Product;
using EShop.Infrastructure.Event.Product;
using EShop.Infrastructure.Query.Product;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Wrap;

namespace EShop.ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IBusControl _busControl;
        private readonly IRequestClient<GetProductById> _requestClient;
        private static readonly int MAX_RETRY_COUNT = 1;
        #region Policies
        // Do obsługi wyłączenia serwera, kiedy zaczyna rzucać wyjątkami
        private static AsyncCircuitBreakerPolicy<IActionResult> _circuitBreaker = Policy<IActionResult>.Handle<Exception>().
                                                                                    AdvancedCircuitBreakerAsync(0.5, TimeSpan.FromSeconds(30),
                                                                                                                    2, TimeSpan.FromMinutes(1));
        // Do obsługi zwracania wyjątków
        private readonly AsyncFallbackPolicy<IActionResult> _fallbackPolicy;
        // Do obsługi ponownego wysłania polecenia w przypadku błędu
        private static AsyncRetryPolicy<IActionResult> _retryPolicy;
        // Do łączenia polityki wyjątków
        private static AsyncPolicyWrap<IActionResult> _wrapPolicy;
        #endregion
        public ProductController(IBusControl busControl, IRequestClient<GetProductById> requestClient)
        {
            _busControl = busControl;
            _requestClient = requestClient;
            _fallbackPolicy = Policy<IActionResult>.Handle<Exception>().FallbackAsync(Content("We are experincing issue. Please try after sometime."));
            _retryPolicy = Policy<IActionResult>.Handle<Exception>()
                ./*RetryAsync*/WaitAndRetryAsync(MAX_RETRY_COUNT, retryCount => TimeSpan.FromSeconds(Math.Pow(3,retryCount)/3));
            _wrapPolicy = Policy.WrapAsync(_circuitBreaker, _retryPolicy);
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid productId)
        {
            var circuitState = _circuitBreaker.CircuitState;
            return await /*_wrapPolicy*/_fallbackPolicy.WrapAsync(_wrapPolicy).ExecuteAsync(async () =>
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
