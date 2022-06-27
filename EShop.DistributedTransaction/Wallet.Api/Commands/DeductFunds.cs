namespace Wallet.Api.Commands
{
    public class DeductFunds
    {
        public Guid UserId { get; set; }
        public int DebitAmount { get; set; }
    }
}
