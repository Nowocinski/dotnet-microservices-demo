using EShop.Infrastructure.Command.User;
using EShop.Infrastructure.Event.User;
using MongoDB.Driver;

namespace EShop.User.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<CreateUser> _collection;
        public UserRepository(IMongoDatabase database)
        {
            _database = database;
            _collection = _database.GetCollection<CreateUser>("user");
        }

        public async Task<UserCreated> AddUser(CreateUser user)
        {
            await _collection.InsertOneAsync(user);
            return new UserCreated
            {
                ContactNo = user.ContactNo,
                EmailId = user.EmailId,
                Password = user.Password,
                Username = user.Username
            };
        }

        public async Task<UserCreated> GetUser(CreateUser user)
        {
            var userResult = _collection.AsQueryable().Where(x => x.Username == user.Username).FirstOrDefault();
            await Task.CompletedTask;
            return new UserCreated()
            {
                ContactNo = userResult.ContactNo,
                EmailId = userResult.EmailId,
                Password = userResult.Password,
                Username = userResult.Username,
                UserId = userResult.UserId
            };
        }
    }
}
