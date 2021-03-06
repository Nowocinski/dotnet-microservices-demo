using EShop.Infrastructure.Query.Product;
using EShop.Product.DataProvider.Services;
using MassTransit;

namespace EShop.Product.Query.Api.Handlers
{
    public class GetProductByIdHandler : IConsumer<GetProductById>
    {
        //private static int EXCEPTION_COUNT = 0;
        private readonly IProductService _productService;
        public GetProductByIdHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task Consume(ConsumeContext<GetProductById> context)
        {
            //if (EXCEPTION_COUNT < 4)
            //{
            //    EXCEPTION_COUNT++;
            //    throw new Exception("something was wrong!...");
            //}
            //await Task.Delay(30000);
            var product = await _productService.GetProduct(context.Message.ProductId);
            await context.RespondAsync(product);
        }
    }
}
