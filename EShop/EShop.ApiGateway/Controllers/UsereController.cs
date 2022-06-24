using EShop.Infrastructure.Authentication;
using EShop.Infrastructure.Command.User;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace EShop.ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsereController : ControllerBase
    {
        private readonly IBusControl _busControl;
        private readonly IRequestClient<LoginUser> _loginRequestClient;
        public UsereController(IBusControl busControl, IRequestClient<LoginUser> loginRequestClient)
        {
            _busControl = busControl;
            _loginRequestClient = loginRequestClient;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CreateUser user)
        {
            var uri = new Uri("rabbitmq://localhost/add_user");
            var endPoint = await _busControl.GetSendEndpoint(uri);
            await endPoint.Send(user);

            return Accepted("User Created");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUser loginUser)
        {
            var userResponse = await _loginRequestClient.GetResponse<JwtAuthToken>(loginUser);
            return Accepted(userResponse.Message);
        }
    }
}
