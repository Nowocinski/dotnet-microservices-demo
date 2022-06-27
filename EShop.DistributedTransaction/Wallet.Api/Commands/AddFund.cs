namespace Wallet.Api.Commands
{
    public class AddFund
    {
        public Guid UserId { get; set; }
        public int CreditAmount { get; set; }
    }
}
