using EShop.Infrastructure.Command.User;
using EShop.Infrastructure.Event.User;
using EShop.Infrastructure.Security;
using EShop.User.DataProvider.Extension;
using EShop.User.DataProvider.Services;
using MassTransit;

namespace EShop.User.Query.Api.Handlers
{
    public class LoginUserHangler : IConsumer<LoginUser>
    {
        private readonly IUserService _userService;
        private readonly IEncrypter _encrypter;
        public LoginUserHangler(IUserService userService, IEncrypter encrypter)
        {
            _userService = userService;
            _encrypter = encrypter;
        }
        public async Task Consume(ConsumeContext<LoginUser> context)
        {
            var userResult = new UserCreated();
            var user = await _userService.GetUserByUsername(context.Message.UserName);
            if (user != null)
            {
                var isAllowed = user.ValidatePassword(context.Message, _encrypter);
                if (isAllowed)
                {
                    userResult = user;
                }
            }
            await context.RespondAsync<UserCreated>(userResult);
        }
    }
}
