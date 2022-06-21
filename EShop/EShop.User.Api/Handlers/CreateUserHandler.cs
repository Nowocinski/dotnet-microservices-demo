using EShop.Infrastructure.Command.User;
using EShop.User.Api.Services;
using MassTransit;

namespace EShop.User.Api.Handlers
{
    public class CreateUserHandler : IConsumer<CreateUser>
    {
        private readonly IUserService _userService;
        public CreateUserHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task Consume(ConsumeContext<CreateUser> context)
        {
            var createUser = await _userService.AddUser(context.Message);
        }
    }
}
