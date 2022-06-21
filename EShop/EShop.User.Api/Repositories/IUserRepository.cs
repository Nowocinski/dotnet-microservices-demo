using EShop.Infrastructure.Command.User;
using EShop.Infrastructure.Event.User;

namespace EShop.User.Api.Repositories
{
    public interface IUserRepository
    {
        Task<UserCreated> AddUser(CreateUser user);
        Task<UserCreated> GetUser(CreateUser user);
    }
}
