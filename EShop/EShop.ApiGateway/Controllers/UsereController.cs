using EShop.Infrastructure.Command.User;
using EShop.Infrastructure.Event.User;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace EShop.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    public class UsereController : Controller
    {
        private readonly IBusControl _busControl;
        private readonly IRequestClient<LoginUser> _loginRequestClient;
        public UsereController(IBusControl busControl, IRequestClient<LoginUser> loginRequestClient)
        {
            _busControl = busControl;
            _loginRequestClient = loginRequestClient;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] CreateUser user)
        {
            var uri = new Uri("rabbitmq://localhost/add_user");
            var endPoint = await _busControl.GetSendEndpoint(uri);
            await endPoint.Send(user);

            return Accepted("User Created");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginUser loginUser)
        {
            var userResponse = await _loginRequestClient.GetResponse<UserCreated>(loginUser);
            return Accepted(userResponse.Message);
        }
    }
}
