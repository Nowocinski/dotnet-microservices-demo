namespace Inventory.Api.Models
{
    public class Inventor
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
