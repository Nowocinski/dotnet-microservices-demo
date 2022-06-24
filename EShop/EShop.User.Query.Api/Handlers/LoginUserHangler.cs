using EShop.Infrastructure.Authentication;
using EShop.Infrastructure.Command.User;
using EShop.Infrastructure.Event.User;
using EShop.User.DataProvider.Extension;
using EShop.User.DataProvider.Services;
using MassTransit;

namespace EShop.User.Query.Api.Handlers
{
    using EShop.Infrastructure.Security;
    public class LoginUserHangler : IConsumer<LoginUser>
    {
        private readonly IUserService _userService;
        private readonly IEncrypter _encrypter;
        private readonly IAuthenticationHandler _authHandler;
        public LoginUserHangler(IUserService userService, IEncrypter encrypter, IAuthenticationHandler authHandler)
        {
            _userService = userService;
            _encrypter = encrypter;
            _authHandler = authHandler;
        }
        public async Task Consume(ConsumeContext<LoginUser> context)
        {
            var user = await _userService.GetUserByUsername(context.Message.UserName);
            JwtAuthToken token = new JwtAuthToken();

            if (user != null)
            {
                var isAllowed = user.ValidatePassword(context.Message, _encrypter);

                if (isAllowed)
                    token = _authHandler.Create(user.UserId.ToString());
            }

            await context.RespondAsync<JwtAuthToken>(token);
        }
    }
}
