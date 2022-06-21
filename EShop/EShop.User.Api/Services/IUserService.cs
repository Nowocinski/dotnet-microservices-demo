using EShop.Infrastructure.Command.User;
using EShop.Infrastructure.Event.User;

namespace EShop.User.Api.Services
{
    public interface IUserService
    {
        Task<UserCreated> AddUser(CreateUser user);
        Task<UserCreated> GetUser(CreateUser user);
    }
}
