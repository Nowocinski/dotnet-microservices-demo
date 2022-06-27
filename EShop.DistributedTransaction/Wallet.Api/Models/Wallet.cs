namespace Wallet.Api.Models
{
    public class Wallet
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public int Fund { get; set; }
    }
}
