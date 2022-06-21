using EShop.Infrastructure.Command.User;
using EShop.Infrastructure.Event.User;
using EShop.User.Api.Repositories;

namespace EShop.User.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserCreated> AddUser(CreateUser user)
        {
            return await _userRepository.AddUser(user);
        }

        public async Task<UserCreated> GetUser(CreateUser user)
        {
            return await _userRepository.GetUser(user);
        }
    }
}
