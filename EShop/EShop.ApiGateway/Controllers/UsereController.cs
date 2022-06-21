using EShop.Infrastructure.Command.User;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace EShop.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    public class UsereController : Controller
    {
        private readonly IBusControl _busControl;
        public UsereController(IBusControl busControl)
        {
            _busControl = busControl;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateUser user)
        {
            var uri = new Uri("rabbitmq://localhost/add_user");
            var endPoint = await _busControl.GetSendEndpoint(uri);
            await endPoint.Send(user);

            return Accepted("User Created");
        }
    }
}
