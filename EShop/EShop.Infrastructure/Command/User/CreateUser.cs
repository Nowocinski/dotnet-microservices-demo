﻿using MongoDB.Bson.Serialization.Attributes;

namespace EShop.Infrastructure.Command.User
{
    public class CreateUser
    {
        [BsonId]
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string EmailId { get; set; }
        public string ContactNo { get; set; }
        public string Password { get; set; }
    }
}
