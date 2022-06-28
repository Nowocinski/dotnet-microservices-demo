using MassTransit;

namespace EventBus.Activities
{
    using Wallet.Api.Commands;
    public class WalletActivity : IActivity<TransactMoney, TransactMoneyLog>
    {
        public async Task<CompensationResult> Compensate(CompensateContext<TransactMoneyLog> context)
        {
            try
            {
                var addFunds = new AddFund
                { UserId = context.Log.UserId, CreditAmount = context.Log.Amount };
                var endpoint = await context.GetSendEndpoint(new Uri("rabbitmq://localhost/add_funds"));
                await endpoint.Send(addFunds);

                return context.Compensated();
            }
            catch (Exception)
            {
                return context.Failed();
            }
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<TransactMoney> context)
        {
            try
            {
                var deductFunds = new DeductFunds() { UserId = context.Arguments.UserId, DebitAmount = context.Arguments.Amount };
                var endpoint = await context.GetSendEndpoint(new Uri("rabbitmq://localhost/deduct_funds"));
                await endpoint.Send(deductFunds);
                return context.CompletedWithVariables<TransactMoneyLog>(new TransactMoneyLog
                {
                    UserId = context.Arguments.UserId,
                    Amount = context.Arguments.Amount
                }, new { });
            }
            catch (Exception)
            {
                return context.Faulted();
            }
        }
    }

    public class TransactMoney
    {
        public Guid UserId { get; set; }
        public int Amount { get; set; }
    }

    public class TransactMoneyLog
    {
        public Guid UserId { get; set; }
        public int Amount { get; set; }
        public string Message { get; set; }
    }
}
