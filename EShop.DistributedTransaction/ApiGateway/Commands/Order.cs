namespace Order.Api.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public Guid InventorId { get; set; }
        public DateTime Created { get; set; }
    }
}
