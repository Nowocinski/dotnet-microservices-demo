using Microsoft.AspNetCore.Mvc;

namespace EShop.Product.Query.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
