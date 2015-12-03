using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Accounts
{

    public interface IAccount
    {
        string ClientId { get; }
        string AccountId { get; }
        double Balance { get; }
        string CurrencyId { get; }
    }

    public class Account : IAccount
    {
        public string ClientId { get; set; }
        public string AccountId { get; set; }
        public double Balance { get; set; }
        public string CurrencyId { get; set; }

        public static Account Create(string clientId, string accountId, double balance, string currencyId)
        {
            return new Account
            {
                ClientId = clientId,
                AccountId = accountId,
                Balance = balance,
                CurrencyId = currencyId
            };
        }
    }

    public interface IAccountsRepository
    {
        Task RegisterAccount(IAccount src);
        Task<IEnumerable<IAccount>> GetAccountsAsync(string clientId);

    }
}
