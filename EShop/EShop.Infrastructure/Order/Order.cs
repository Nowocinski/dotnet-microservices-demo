﻿using MongoDB.Bson.Serialization.Attributes;

namespace EShop.Infrastructure.Order
{
    public class Order
    {
        [BsonId]
        public Guid Id { get; set; }
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal Amount
        {
            get
            {
                return Items.Sum(item => item.Price * item.Quantity);
            }
        }
    }

    public class OrderItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
