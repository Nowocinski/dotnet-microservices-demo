﻿using MongoDB.Bson.Serialization.Attributes;

namespace EShop.Infrastructure.Command.Product
{
    public class CreateProduct
    {
        [BsonId]
        //[BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public float ProductPrice { get; set; }
        public Guid CategoryId { get; set; }
    }
}
