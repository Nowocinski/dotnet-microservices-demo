using MongoDB.Bson.Serialization.Attributes;

namespace EShop.Infrastructure.Event.User
{
    public class UserCreated
    {
        [BsonId]
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string EmailId { get; set; }
        public string ContactNo { get; set; }
        public string Password { get; set; }
    }
}
